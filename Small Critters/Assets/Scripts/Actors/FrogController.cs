using UnityEngine;
using System.Collections;
using System;

public class FrogController : MonoBehaviour {
	private Imovement movementScript;
	private FrogInputHandler inputScript;
    public DeathParticleSystemHandler particleSystemHandler;
    public delegate void FrogDeath(string causeOfDeath);
    public delegate void FoodPickup(float HP);
    public FrogDeath OnFrogDeath;
    public FoodPickup OnFoodPickup;
    public Animator myAnimator;
    public SpritesFader frogFader;
    public float HP = 0;
    public float powerupProgress = 0f;
    public float powerupProgressPerStar = 0.2f;
    public float troubleshooterDuration = 1;
    private bool invulnerable = false;
    public CostumeSwitcher TroubleshooterCostume;
    

    // Use this for initialization
    void Start () {
		GetRequiredComponents();
    }
	
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.collider.CompareTag("Hazard") && !invulnerable)
		{
            TakeHit(coll.collider.name);
           // Die (coll.collider.name);
		}
        if (coll.collider.CompareTag("Food"))
        {
            myAnimator.SetTrigger("Lick");
            if (HP < 1f) HP += 0.2f;
            OnFoodPickup(HP);
        }
        //if (coll.collider.CompareTag("Star"))
        //{
        //    if (powerupProgress < 1f) powerupProgress += powerupProgressPerStar;
        //    if (powerupProgress == 1f)
        //    {
        //        powerupProgress = 0;
        //        StartCoroutine(TroubleshooterMode());
        //    }
        //    //OnFoodPickup(HP);
        //}
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard") && !invulnerable) 
        {
            TakeHit(other.name);
            //Die(other.name);
        }
        //if (other.CompareTag("Star"))
        //{
        //    if (powerupProgress < 1f) powerupProgress += powerupProgressPerStar;
        //    if (powerupProgress == 1f)
        //    {
        //        powerupProgress = 0;
        //        StartCoroutine(TroubleshooterMode());
        //    }
        //}
    }

    // Update is called once per frame
    private void GetRequiredComponents()
	{
		movementScript = GetComponent<Imovement>();
		inputScript = GetComponent<FrogInputHandler>();
        OnFrogDeath += particleSystemHandler.OnDeath;
        
        //myGameController = GetComponent<GameController>();
    }

    private void TakeHit(string hitSource)
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
        
    }

    private void RecoverFromHit()
    {
        invulnerable = false;
    }

	public void Die(string causeOfDeath)
	{
		//Instantiate(frogExplosionPlayer, this.transform.position, Quaternion.identity);
		//Instantiate(deadFrogSprite, this.transform.position,Quaternion.identity);

		//Destroy(gameObject);
		gameObject.SetActive(false);
        OnFrogDeath(causeOfDeath);
        //Debug.Log(causeOfDeath);
        //myGameController.onFrogDeath();
        //OnFrogDeath();

    }

    //private IEnumerator TroubleshooterMode()
    //{
    //    TroubleshooterCostume.PutOnCostume();
    //    yield return new WaitForSeconds(troubleshooterDuration);
    //    TroubleshooterCostume.TakeOffCostume();
    //}
}
