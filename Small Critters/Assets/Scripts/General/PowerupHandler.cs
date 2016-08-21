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
    public Animator powerupUIAnimator;
    public bool usedBonusIncentive = false;
    public float totalTimeOnPowerup = 0;
    public int totalNumberOfPowerups = 0;
    public int maxAmmoThisRun = 0;
    public ParticleSystem powerupFullEffect;

    private float powerupStartTime;
    

    private IAudio myAudio;

    void Start()
    {
        myAudio = ServiceLocator.getService<IAudio>();
    }

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
            powerupUIAnimator.SetTrigger("TroubleShooter");
            ++totalNumberOfPowerups;
            powerupStartTime = Time.timeSinceLevelLoad;
        }
        powerupFullEffect.Play();
        myAudio.PlaySound(Sound.StartPowerup);
        currentStarPoints = 0;
        currentAmmo += maxAmmo;
        if (currentAmmo > maxAmmoThisRun)
        {
            maxAmmoThisRun = currentAmmo;
        }
        uiHandler.UpdateAmmoCount(currentAmmo);
        
        
    }
    private void EndPowerupMode()
    {
        powerupModeOn = false;
        uiHandler.PowerupMode(powerupModeOn);
        costumeSwitcher.TakeOffCostume();
        currentStarPoints = 0f;
        totalTimeOnPowerup += Time.timeSinceLevelLoad - powerupStartTime;
    }

    public void SetBonus()
    {
        usedBonusIncentive = true;
        StartPowerupMode();
        frogController.FillHP();
    }
}
