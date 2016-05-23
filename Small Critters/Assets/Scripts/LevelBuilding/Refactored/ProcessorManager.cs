using UnityEngine;
using System.Collections;
using System;

public class ProcessorManager : MonoBehaviour, IResetable {
	SpriteRenderer mySpriteRenderer;
    Animator myAnimator;
	public ProcessorState state;
	public float stateExitTime; //Time.timeSinceLevelLoad + some state timer
	public float stateStayTimeCompletion; // 0.0-1.0 %
    private Action<GameObject> storeInPool;
	//private ParticleSystem steamRing;
	
	// Use this for initialization
	void Awake () {
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		gameObject.layer = 8;
		//steamRing = GetComponent<ParticleSystem>();
        myAnimator = GetComponent<Animator>();

    }
    void OnEnable()
    {
        myAnimator.ResetTrigger("SteamStart");
        myAnimator.ResetTrigger("SteamEnd");
    }

    public void TintProcessorSprite(Color startColor, Color targetColor, float percent)
	{
		mySpriteRenderer.color = Color.Lerp(startColor, targetColor, percent);
	}
	public void SetProcessorSpriteColor(Color color)
	{
		mySpriteRenderer.color = color;
	}
	public void SetHazadrousLayer()
	{
		gameObject.layer = 15;
        //steamRing.Simulate(0f, false, true);
        //steamRing.Play();
        //if (myAnimator.isActiveAndEnabled)
        //{
        //    myAnimator.SetTrigger("SteamStart");
        //}
        myAnimator.SetTrigger("SteamStart");
    }
	public void SetSafeLayer()
	{
        //steamRing.Pause();
        //steamRing.Clear();
        //steamRing.Stop();
        //if (myAnimator.isActiveAndEnabled)
        //{
        //    myAnimator.SetTrigger("SteamEnd");
        //}
        myAnimator.SetTrigger("SteamEnd");
        gameObject.layer = 8;
    }

    public void Reset(Action<GameObject> storeInPool)
    {
        this.storeInPool = storeInPool;
        
        myAnimator.SetTrigger("Reset");
        StartCoroutine(WaitForNextUpdate(storeInPool));
    }

    private IEnumerator WaitForNextUpdate(Action<GameObject> storeInPool)
    {
        //myAnimator.ResetTrigger("SteamStart");
        //myAnimator.ResetTrigger("SteamEnd");
        //float timeStart = Time.timeSinceLevelLoad;
        //myAnimator.SetTrigger("Reset");
        while (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            yield return true;
        }
        myAnimator.ResetTrigger("SteamStart");
        myAnimator.ResetTrigger("SteamEnd");
        storeInPool(transform.gameObject);
        //Debug.Log("Stored with pause: " + (Time.timeSinceLevelLoad - timeStart));
        //Debug.Log( myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        //Debug.Log("At: " + transform.position.x + ", " + transform.position.y);
        
    }

    public void PostResetDebug()
    {
        
    }
}
