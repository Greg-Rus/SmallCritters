using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SectionBuilderBugs : ISectionBuilder
{
    public SectionBuilderType type { get; set; }
    LevelData levelData;
    GameObjectPoolManager poolManager;
    IBeeSectionDifficulty beeDifficultyManager;
    IFireBeetleDifficultyManager fireBeetleDifficultyManager;
    IBugsSectionDifficulty bugDifficultyManager;
    GameObject bee;
    GameObject fly;
    GameObject fireBeetle;
    List<GameObject> currentRow;
    List<Vector2> flyPositions;
    List<Vector2> beePositions;
    List<Vector2> beetlePositions;
                                                  //Fly,Bee,Beetle
    private int[][] bugCountsPerDecyl = { new int[] { 5, 1, 0 },
                                          new int[] { 5, 2, 0 },
                                          new int[] { 4, 1, 1 },
                                          new int[] { 4, 2, 1 },
                                          new int[] { 4, 2, 2 },
                                          new int[] { 3, 3, 2 },
                                          new int[] { 3, 3, 2 },
                                          new int[] { 3, 4, 2 },
                                          new int[] { 3, 4, 3 },
                                          new int[] { 2, 4, 3 }}; //Minimum bug counts for each 10% of difficulty

    public SectionBuilderBugs(LevelData levelData, GameObjectPoolManager poolManager)
    {
        this.levelData = levelData;
        this.poolManager = poolManager;
        beeDifficultyManager = ServiceLocator.getService<IBeeSectionDifficulty>();
        fireBeetleDifficultyManager = ServiceLocator.getService<IFireBeetleDifficultyManager>();
        bugDifficultyManager = ServiceLocator.getService<IBugsSectionDifficulty>();
        type = SectionBuilderType.bugs;
        LoadPrefabs();
        InitiatePools();
        flyPositions = new List<Vector2>(5); //TODO This can be calculated based on bugCountsPerDecyl contents
        beePositions = new List<Vector2>(4);
        beetlePositions = new List<Vector2>(3);
    }

    private void LoadPrefabs()
    {
        bee = Resources.Load("Bee") as GameObject;
        fly = Resources.Load("Fly") as GameObject;
        fireBeetle = Resources.Load("FireBeetle") as GameObject;
    }
    private void InitiatePools()
    {
        poolManager.addPool(bee, 20, 10);
        poolManager.addPool(fly, 20, 10);
        poolManager.addPool(fireBeetle, 20, 10);
    }

    public void buildNewRow(List<GameObject> row)
    {
        if (levelData.newSectionStart == levelData.levelTop + 1)
        {
            PlanBugPlacement();
        }
        currentRow = row;
        DeployBugsAtRow();
    }

    private void DeployBugsAtRow()
    {
        DeployBugType("Fly", flyPositions);
        DeployBugType("Bee", beePositions);
        DeployBugType("FireBeetle", beetlePositions);
    }

    private void DeployBugType(string bugType, List<Vector2> bugPositions)
    {
        for (int i = 0; i < bugPositions.Count; ++i)
        {
            if (bugPositions[i].y == levelData.levelTop + 1)
            {
                DeployBugTypeAtPosition(bugType, bugPositions[i].x);
            }
        }
    }

    private void PlanBugPlacement()
    {
        flyPositions.Clear();
        beePositions.Clear();
        beetlePositions.Clear();

        int[] bugCounts = GetBugCountBasedOnDifficulty();
        PickLocationsForBugType(bugCounts[0], flyPositions);
        PickLocationsForBugType(bugCounts[1], beePositions);
        PickLocationsForBugType(bugCounts[2], beetlePositions);
    }

    private void PickLocationsForBugType(int count, List<Vector2> bugList)
    {
        Vector2 newLocation;
        for (int i = 0; i < count; ++i)
        {
            do
            {
                newLocation = SelectLocationInSection();
            } while (flyPositions.Contains(newLocation) ||
                     beePositions.Contains(newLocation) ||
                     beetlePositions.Contains(newLocation));
            bugList.Add(newLocation);
        }
    }

    private int[] GetBugCountBasedOnDifficulty()
    {
        int i = (int)(bugDifficultyManager.difficultyPercent * 10);
        return bugCountsPerDecyl[i];
    }

    private Vector2 SelectLocationInSection()
    {
        int xPos = RandomLogger.GetRandomRange(1, levelData.navigableAreaWidth);
        int yPos = RandomLogger.GetRandomRange(levelData.newSectionStart, levelData.newSectionEnd);
        return new Vector2(xPos, yPos);
    }

    private void DeployBugAtPosition(float xCoordinate)
    {
        switch (bugDifficultyManager.GetBugType())
        {
            case BugType.Bee: ConfigureBeeController(DeployBugAtPosition("Bee", xCoordinate)); break;
            case BugType.FireBeetle: ConfigureFireBeetleController(DeployBugAtPosition("FireBeetle", xCoordinate)); break;
            case BugType.Fly: ConfigureFlyController(DeployBugAtPosition("Fly", xCoordinate)); break;
        }
    }

    private void DeployBugTypeAtPosition(string bugName, float xCoordinate)
    {
        switch (bugName)
        {
            case "Fly": ConfigureFlyController(DeployBugAtPosition("Fly", xCoordinate)); break;
            case "Bee": ConfigureBeeController(DeployBugAtPosition("Bee", xCoordinate)); break;
            case "FireBeetle": ConfigureFireBeetleController(DeployBugAtPosition("FireBeetle", xCoordinate)); break;
        }
    }

    private GameObject DeployBugAtPosition(string bugName, float xCoordinate)
    {
        GameObject newBug = poolManager.retrieveObject(bugName);
        Vector3 newBugPosition = new Vector3(xCoordinate + levelData.bugSpawnAreaOffest, levelData.levelTop+1, 0f);
        newBug.transform.position = newBugPosition;
        newBug.transform.Rotate(new Vector3(0f, 0f, RandomLogger.GetRandomRange(0, 360)));
        currentRow.Add(newBug);
        return newBug;
    }

    private void ConfigureBeeController(GameObject bee)
    {
        BeeController newBeeController = bee.GetComponent<BeeController>();
        newBeeController.data = new BeeData();
        newBeeController.data.chargeDistance = beeDifficultyManager.GetChargeDistance();
        newBeeController.data.chargeSpeed = beeDifficultyManager.GetChargeSpeed();
        newBeeController.data.chargeTime = beeDifficultyManager.GetChargeTime();
        newBeeController.data.flySpeed = beeDifficultyManager.GetFlySpeed();
        newBeeController.data.stunTime = beeDifficultyManager.GetStunTime();
    }

    private void ConfigureFlyController(GameObject fly)
    {
        FlyController newFlyController = fly.GetComponent<FlyController>();
        float flyBodyRadius = fly.GetComponent<CircleCollider2D>().radius;
        newFlyController.flyZoneBottomLeft = new Vector2(1.5f + flyBodyRadius, levelData.newSectionStart - 0.5f);
        newFlyController.flyZoneTopRight = new Vector2(levelData.levelWidth - flyBodyRadius, levelData.newSectionEnd + 0.5f);
        newFlyController.SelectDestination();
    }

    private void ConfigureFireBeetleController(GameObject fireBeetle)
    {
        FireBeetleController newFireBeetleController = fireBeetle.GetComponent<FireBeetleController>();
        newFireBeetleController.data.attackDistanceMax = fireBeetleDifficultyManager.GetAttackDistanceMax();
        newFireBeetleController.data.attackDistanceMin = fireBeetleDifficultyManager.GetAttackDistanceMin();
        newFireBeetleController.data.rotationSpeed = fireBeetleDifficultyManager.GetRotationSpeed();
        newFireBeetleController.data.shotCooldownTime = fireBeetleDifficultyManager.GetShotCooldownTime();
        newFireBeetleController.data.walkSpeed = fireBeetleDifficultyManager.GetWalkSpeed();
    }
}
