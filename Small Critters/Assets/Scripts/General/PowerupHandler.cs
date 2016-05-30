using UnityEngine;
using System.Collections;

public class PowerupHandler : MonoBehaviour {
    public float currentStarPoints = 0;
    public float maxStarPoints;

    public int maxAmmo = 5;
    public int currentAmmo;

    public UIHandler uiHandler;
	// Use this for initialization
	void Start () {
	
	}

    public void UpdatePoints(float points)
    {
        currentStarPoints += points;
        uiHandler.UpdatePowerup(currentStarPoints / maxStarPoints);
        if (currentStarPoints == maxStarPoints)
        {
            uiHandler.PowerupMode(true);
            currentAmmo = maxAmmo;
            uiHandler.UpdateAmmoCount(currentAmmo); ;
        }
    }

    private void OnShotFired()
    {
        --currentAmmo;
        if (currentAmmo == 0)
        {
            uiHandler.PowerupMode(false);
        }
        else uiHandler.UpdateAmmoCount(currentAmmo); ;
    }
}
