using UnityEngine;
using System.Collections;

public class PowerupHandler : MonoBehaviour {
    public float currentStarPoints = 0;
    public float maxStarPoints;

    public int maxAmmo = 5;
    public int currentAmmo;

    public UIHandler uiHandler;
    public CostumeSwitcher costumeSwitcher;
	// Use this for initialization
	void Start () {

    }

    public void UpdatePoints(float points)
    {
        currentStarPoints += points;
        uiHandler.UpdatePowerup(currentStarPoints / maxStarPoints);
        if (currentStarPoints == maxStarPoints)
        {
            StartPowerupMode();
        }
    }

    public void OnShotFired()
    {
        --currentAmmo;
        if (currentAmmo == 0)
        {
            EndPowerupMode();
        }
        else uiHandler.UpdateAmmoCount(currentAmmo);
        Debug.Log(currentAmmo);
    }

    private void StartPowerupMode()
    {
        uiHandler.PowerupMode(true);
        currentAmmo = maxAmmo;
        uiHandler.UpdateAmmoCount(currentAmmo);
        costumeSwitcher.PutOnCostume();
    }
    private void EndPowerupMode()
    {
        uiHandler.PowerupMode(false);
        costumeSwitcher.TakeOffCostume();
    }
}
