using UnityEngine;
using System.Collections;

public class PowerupHandler : MonoBehaviour, IPowerup
{
    public float currentStarPoints = 0f;
    public float maxStarPoints;
    public int maxAmmo = 5;
    public int currentAmmo;
    public UIHandler uiHandler;
    public CostumeSwitcher costumeSwitcher;
    public FrogController frogController;
    public bool powerupModeOn { get; private set; }

    public void UpdatePoints(float points)
    {
        currentStarPoints += points;
        
        if (currentStarPoints >= maxStarPoints)
        {
            StartPowerupMode();
        }
        uiHandler.UpdatePowerup(currentStarPoints / maxStarPoints);
    }

    public void OnShotFired()
    {
        --currentAmmo;
        if (currentAmmo == 0)
        {
            EndPowerupMode();
        }
        else uiHandler.UpdateAmmoCount(currentAmmo);
    }

    private void StartPowerupMode()
    {
        if (!powerupModeOn)
        {
            powerupModeOn = true;
            uiHandler.PowerupMode(powerupModeOn);
            costumeSwitcher.PutOnCostume();
        }
        currentStarPoints = 0;
        currentAmmo += maxAmmo;
        uiHandler.UpdateAmmoCount(currentAmmo);
        
    }
    private void EndPowerupMode()
    {
        powerupModeOn = false;
        uiHandler.PowerupMode(powerupModeOn);
        costumeSwitcher.TakeOffCostume();
        currentStarPoints = 0f;
    }

    public void SetBonus()
    {
        StartPowerupMode();
        frogController.FillHP();
    }
}
