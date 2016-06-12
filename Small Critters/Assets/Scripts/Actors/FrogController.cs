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
    private bool invulnerable = false;
    public CostumeSwitcher TroubleshooterCostume;
    public CameraVerticalFollow mainCamera;
    public float HPprogressPerFly = 0.1f;

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
            if (HP < 1f) HP += HPprogressPerFly;
            if (HP > 1f) HP = 1f;
            OnFoodPickup(HP);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard")) 
        {
            TakeHit(other.name);
        }
    }

    private void GetRequiredComponents()
	{
        OnFrogDeath += particleSystemHandler.OnDeath;
        OnFrogDeath += ServiceLocator.getService<IGameProgressReporting>().RunEnd;
        mainCamera = Camera.main.GetComponent<CameraVerticalFollow>();
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
		gameObject.SetActive(false);
        OnFrogDeath(causeOfDeath);
    }

    public void FillHP()
    {
        HP = 1f;
        OnFoodPickup(HP);
    }
}
