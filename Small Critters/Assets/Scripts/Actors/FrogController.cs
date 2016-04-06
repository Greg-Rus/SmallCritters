using UnityEngine;
using System.Collections;
using System;

public class FrogController : MonoBehaviour {
	private Imovement movementScript;
	private FrogInputHandler inputScript;
	public GameObject deadFrogSprite;
	public GameObject frogExplosionPlayer;
	//public GameController myGameController;
	//public event EventHandler FrogDeath;
    public delegate void FrogDeath(string causeOfDeath);
    public FrogDeath OnFrogDeath;
    // Use this for initialization
    void Start () {
		getRequiredComponents();
		setupInputScript();
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.collider.tag == "Hazard")
		{
			Die (coll.collider.name);
		}
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard")) 
        {
            //Debug.Log("Triggered Hazard");
            Die(other.name);
        }
    }

    // Update is called once per frame
    private void getRequiredComponents()
	{
		movementScript = GetComponent<Imovement>();
		inputScript = GetComponent<FrogInputHandler>();
		//myGameController = GetComponent<GameController>();
	}

	private void setupInputScript()
	{
		inputScript.frogMovement = movementScript;
	}
	
	public void Die(string causeOfDeath)
	{
		Instantiate(frogExplosionPlayer, this.transform.position, Quaternion.identity);
		Instantiate(deadFrogSprite, this.transform.position,Quaternion.identity);
		//Destroy(gameObject);
		gameObject.SetActive(false);
        OnFrogDeath(causeOfDeath);
        //myGameController.onFrogDeath();
        //OnFrogDeath();

    }
	
	//private void OnFrogDeath()
	//{
	//	if (FrogDeath != null)
	//	{
	//		FrogDeath(this, EventArgs.Empty);
	//	}
	//}

}
