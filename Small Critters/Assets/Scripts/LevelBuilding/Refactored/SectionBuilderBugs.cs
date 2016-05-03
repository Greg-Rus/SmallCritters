using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SectionBuilderBugs : ISectionBuilder
{
    public SectionBuilderType type { get; set; }
    LevelData levelData;
    GameObjectPoolManager poolManager;
    ScoreHandler scoreHandler;
    IBeeSectionDifficulty beeDifficultyManager;
    BugsDifficultyManager bugDifficultyManager;
    GameObject bee;
    GameObject fly;
    GameObject fireBeetle;
    String nextBug;
    List<GameObject> currentRow;

    public SectionBuilderBugs(LevelData levelData, GameObjectPoolManager poolManager)
    {
        this.levelData = levelData;
        this.poolManager = poolManager;
        beeDifficultyManager = ServiceLocator.getService<IBeeSectionDifficulty>();
        bugDifficultyManager = ServiceLocator.getService<BugsDifficultyManager>();
        scoreHandler = ServiceLocator.getService<ScoreHandler>();
        type = SectionBuilderType.bugs;
        LoadPrefabs();
        ConfigurePrefabs();
        InitiatePools();
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
    private void ConfigurePrefabs()
    {
        BeeController beeController = bee.GetComponent<BeeController>();
        beeController.poolManager = poolManager;
        beeController.scoreHandler = scoreHandler;

        FlyController flyController = fly.GetComponent<FlyController>();
        flyController.scoreHandler = scoreHandler;

        FireBeetleController fireBeetleController = fireBeetle.GetComponent<FireBeetleController>();
        fireBeetleController.scoreHandler = scoreHandler;
    }


    public void buildNewRow(List<GameObject> row)
    {
        currentRow = row;
        DeployBugAtPosition(1.5f); //Next to left wall
        DeployBugAtPosition(levelData.navigableAreaWidth); //Next to rifht wall

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

    private GameObject DeployBugAtPosition(string bugName, float xCoordinate)
    {
        GameObject newBug = poolManager.retrieveObject(bugName);
        Vector3 newBugPosition = new Vector3(xCoordinate, levelData.levelTop, 0f);
        newBug.transform.position = newBugPosition;
        if (xCoordinate > levelData.levelWidth * 0.5f)
        {
            newBug.transform.Rotate(new Vector3(0f, 0f, 180f)); // Bee faces right by default
        }
        currentRow.Add(newBug);
        return newBug;
    }

    //private void DeployBeeAtPosition(float xCoordiante) //TODO refactor the Dploy functions to avoid code duplication
    //{
    //    GameObject newBee = poolManager.retrieveObject("Bee");
    //    Vector3 newBeePosition = new Vector3(xCoordiante, levelData.levelTop, 0f);
    //    newBee.transform.position = newBeePosition;
    //    ConfigureBeeController(newBee);
    //    if (xCoordiante > levelData.levelWidth * 0.5f)
    //    {
    //        newBee.transform.Rotate(new Vector3(0f, 0f, 180f)); // Bee faces right by default
    //    }
    //    currentRow.Add(newBee);
    //}

    private void ConfigureBeeController(GameObject bee)
    {
        BeeController newBeeController = bee.GetComponent<BeeController>();
        newBeeController.Reset();
        newBeeController.poolManager = poolManager;
        newBeeController.chargeDistance = beeDifficultyManager.GetChargeDistance();
        newBeeController.chargeSpeed = beeDifficultyManager.GetChargeSpeed();
        newBeeController.chargeTime = beeDifficultyManager.GetChargeTime();
        newBeeController.flySpeed = beeDifficultyManager.GetFlySpeed();
    }

    private void ConfigureFlyController(GameObject fly)
    {
        FlyController newFlyController = fly.GetComponent<FlyController>();
        newFlyController.flyZoneBottomLeft = new Vector2(1.5f, levelData.levelTop - 0.5f);
        newFlyController.flyZoneTopRight = new Vector2(levelData.levelWidth, levelData.levelTop + 0.5f);
        newFlyController.SelectDestination();
    }

    private void ConfigureFireBeetleController(GameObject fireBeetle)
    {
        //TODO

    }


}
