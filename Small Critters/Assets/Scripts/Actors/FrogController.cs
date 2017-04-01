using UnityEngine;
using System.Collections;
using System;

public class FrogController : MonoBehaviour {
    public DeathParticleSystemHandler particleSystemHandler;
    public delegate void FrogDeath(string causeOfDeath);
    public delegate void FoodPickup(float HP);
    public FrogDeath OnFrogDeath;
    public FoodPickup OnFoodPickup;
    public Animator myAnimator;
    public SpritesFader frogFader;
    public float HP = 0;
    public float troubleshooterDuration = 1;
    public CostumeSwitcher TroubleshooterCostume;
    public CameraVerticalFollow mainCamera;
    public float HPprogressPerFly = 0.1f;

    private bool invulnerable = false;
    private bool isAlive = true;
    private IAudio myAudio;

    void Start () {
		GetRequiredComponents();
    }
	
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.collider.CompareTag("Hazard"))
		{
            TakeHit(coll.collider.name);
		}
        if (coll.collider.CompareTag("Food"))
        {
            myAnimator.SetTrigger("Lick");
            myAudio.PlaySound(Sound.EatFly);
            if (HP != 1f)
            {
                if (HP < 1f) HP += HPprogressPerFly;
                if (HP > 1f) HP = 1f;
                OnFoodPickup(HP);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard")) 
        {
            Debug.Log("Triger :" + other.name);
            TakeHit(other.name);
        }
    }

    private void GetRequiredComponents()
	{
        OnFrogDeath += particleSystemHandler.OnDeath;
        OnFrogDeath += ServiceLocator.getService<IGameProgressReporting>().RunEnd;
        mainCamera = Camera.main.GetComponent<CameraVerticalFollow>();
        myAudio = ServiceLocator.getService<IAudio>();
    }

    private void TakeHit(string hitSource)
    {
        if (!invulnerable)
        {
            --HP;
            if (HP < 0)
            {
                Die(hitSource);
            }
            else
            {
                OnFoodPickup(HP);
                invulnerable = true;
                frogFader.StartFadeSequence(RecoverFromHit);
            }
            mainCamera.ShakeCamera();
            myAudio.PlaySound(Sound.PlayerHit);
        }
        if (hitSource == "ColdFog")
        {
            Die(hitSource);
            mainCamera.ShakeCamera();
        }
    }

    private void RecoverFromHit()
    {
        invulnerable = false;
    }

	public void Die(string causeOfDeath)
	{
        Debug.Log(causeOfDeath);
        if (isAlive)
        {
            isAlive = false;
            gameObject.SetActive(false);
            OnFrogDeath(causeOfDeath);
            myAudio.PlaySound(Sound.PlayerKilled);
        }
    }

    public void FillHP()
    {
        HP = 1f;
        OnFoodPickup(HP);
    }
}
