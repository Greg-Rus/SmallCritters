using UnityEngine;
using System.Collections;

public class FrogController : MonoBehaviour {
	private Imovement movementScript;
	private FrogInputHandler inputScript;
	public GameObject deadFrogSprite;
	public GameObject frogExplosionPlayer;
	public GameController myGameController;
	
	
	// Use this for initialization
	void Start () {
		getRequiredComponents();
		setupInputScript();
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.collider.tag == "Hazard")
		{
			die ();
		}
	}
	// Update is called once per frame
	private void getRequiredComponents()
	{
		movementScript = GetComponent<Imovement>();
		inputScript = GetComponent<FrogInputHandler>();
		myGameController = GetComponent<GameController>();
	}

	private void setupInputScript()
	{
		inputScript.frogMovement = movementScript;
	}
	
	public void die()
	{
		Instantiate(frogExplosionPlayer, this.transform.position, Quaternion.identity);
		Instantiate(deadFrogSprite, this.transform.position,Quaternion.identity);
		Destroy(gameObject);
		myGameController.onFrogDeath();
	}

}
