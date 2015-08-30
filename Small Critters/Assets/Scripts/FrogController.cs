using UnityEngine;
using System.Collections;

public class FrogController : MonoBehaviour {
	private Imovement movementScript;
	private FrogInputHandler inputScript;
	private Animator myAnimator;
	private Rigidbody2D myRigidBody;
	public GameObject deadFrogSprite;
	public GameObject frogExplosionPlayer;
	private SpriteRenderer mySpriteRenderer;
	
	
	// Use this for initialization
	void Start () {
		getRequiredComponents();
		setupInputScript();
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		Debug.Log(coll.gameObject.name);
		if (coll.gameObject.tag == "InstaGib")
		{
			
			Debug.Log("should die");

			die ();
		}
	}
	// Update is called once per frame
	private void getRequiredComponents()
	{
		movementScript = GetComponent<FrogMovementGrid>();
		inputScript = GetComponent<FrogInputHandler>();
		myAnimator = GetComponent<Animator>();
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		myRigidBody = GetComponent<Rigidbody2D>();
	}

	private void setupInputScript()
	{
		inputScript.frogMovement = movementScript;
	}
	
	private void die()
	{
		Instantiate(frogExplosionPlayer, this.transform.position, Quaternion.identity);
		Instantiate(deadFrogSprite, this.transform.position,Quaternion.identity);
		Destroy(gameObject);
	}

}
