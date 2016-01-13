using UnityEngine;
using System.Collections;

public class HeatVentController : MonoBehaviour {

	
	public GameObject lightShaft;
	public GameObject lightTip;
	public GameObject ventDoor;
	public BoxCollider2D killArea;
	public float length = 4.5f;
	//public Vector3 direction = Vector3.right;
	public float minFlickerSpeed = 5f;
	public float maxFlickerSpeed = 10f;
	public ParticleSystem flame;
	
	float shaftHeight;
	float shaftWidth;
	float tipHeight;
	float tipWidth;
	
	float maxShaftScale = 1f;
	float minShaftScale = 0.7f;
	float thicknessFrom;
	float thicknessTo;
	float tipWidthFrom;
	float tipWidthTo;
	public float flickerSpeed = 2f; 
	float flickerTime;
	float lightTipHalfWidth;
	Vector3 tipStartPosition;
	
	public float closedTime = 2f;
	public float ventDoorTime= 0.5f;
	public float warmupTime = 1f;
	public float ventingTime = 2f;
	
	public float stateExitTime;
	private float stateDuration;
	public float StateExitTime
	{
		get{return stateExitTime;}
		set{stateExitTime = value; stateDuration = stateExitTime - Time.timeSinceLevelLoad;}
	}
	
	public HeatVentState state = HeatVentState.Start;
	private HeatVentFSM fsm;

	
	
	// Use this for initialization
	void Awake () {
		lightTipHalfWidth = lightTip.GetComponent<SpriteRenderer>().bounds.size.x * 0.5f;

		thicknessFrom = maxShaftScale;
		thicknessTo = minShaftScale;
		flame.startLifetime = length* 0.1f;
		
		tipStartPosition = lightTip.transform.localPosition;
		
		fsm = new HeatVentFSM();
	}
	
	// Update is called once per frame
	void Update () {
		fsm.UpdateVentingPhase(this);
	}
	
	public void Configure(float length, float[] timers, float cycleCompletion)
	{
		this.length = length;
		fsm.SetStateTimes(timers);
		fsm.SetCycleCompletion(this, cycleCompletion);
		flame.startLifetime = length* 0.1f;
	}
	
	public void UpdateVentigState()
	{
		UpdateVentHeatAura(length);
		ExpandKillArea();
	}
	
	private void UpdateVentHeatAura(float shaftLength)
	{
		UpdateFlickerCycle();
		UpdateShaftThickness(shaftLength);
		UpdateTipWidth(shaftLength);
	}
	
	private void UpdateFlickerCycle()
	{
		if(lightShaft.transform.localScale.y <= minShaftScale)
		{
			thicknessFrom = minShaftScale;
			thicknessTo = maxShaftScale;
			tipWidthFrom = 1f;
			tipWidthTo = 2f;
			flickerTime = 0f;
			flickerSpeed = Random.Range(minFlickerSpeed, maxFlickerSpeed);
		}
		if(lightShaft.transform.localScale.y >= maxShaftScale)
		{
			thicknessFrom = maxShaftScale;
			thicknessTo = minShaftScale;
			tipWidthFrom = 2f;
			tipWidthTo = 1f;
			flickerTime = 0f;
			flickerSpeed = Random.Range(minFlickerSpeed, maxFlickerSpeed);
		}
		
		flickerTime+= Time.deltaTime * flickerSpeed;
	}
	
	private void UpdateShaftThickness(float shaftLength)
	{
		float newThickness =  Mathf.Lerp(thicknessFrom, thicknessTo, flickerTime);
		Vector3 newShaftThickness = new Vector3(shaftLength*2f, newThickness, lightShaft.transform.localScale.z);
		lightShaft.transform.localScale = newShaftThickness;
		Vector3 newTipThickness = new Vector3(lightTip.transform.localScale.x, newThickness, lightTip.transform.localScale.z);
		lightTip.transform.localScale = newTipThickness;
	}
	
	
	private void UpdateTipWidth(float shaftLength)
	{
		float newWidth =  Mathf.Lerp(tipWidthFrom, tipWidthTo, flickerTime);
		Vector3 newTipThickness = new Vector3(newWidth, lightTip.transform.localScale.y, lightTip.transform.localScale.z);
		lightTip.transform.localScale = newTipThickness;

		Vector3 offset = transform.right * ( shaftLength + (lightTipHalfWidth * newWidth));
		Vector3 newTipPosition =  tipStartPosition + offset;

		lightTip.transform.localPosition = newTipPosition;
	}
	
	public void UpdateVentDoorOpening()
	{
		float completionPercent = GetStateCompletionPercent();
		float ventDoorX = Mathf.Lerp(-4f, -4.5f, completionPercent);
		Vector3 newVentDoorPosition = new Vector3(ventDoorX,0f,0f);
		//Debug.Log (completionPercent);
		ventDoor.transform.localPosition = newVentDoorPosition;
		
		float newlightShaftLength = Mathf.Lerp(0f, length, completionPercent);
		UpdateVentHeatAura(newlightShaftLength);
	}
	
	public void UpdateVentDoorClosing()
	{
		UpdateVentHeatAura(length);
		float completionPercent = GetStateCompletionPercent();	
		float ventDoorX = Mathf.Lerp(-4.5f, -4f, completionPercent);
		Vector3 newVentDoorPosition = new Vector3(ventDoorX,0f,0f);
		ventDoor.transform.localPosition = newVentDoorPosition;
		
		float newlightShaftLength = Mathf.Lerp(length, 0f, completionPercent);
		UpdateVentHeatAura(newlightShaftLength);
		
		ShrinkKillArea();
	}
	
	public void UpdateWarmingUp()
	{
		UpdateVentHeatAura(length);
	}

	public void ExpandKillArea() //Not entirely happy with this, but the alternative is to equip all particle with colliders.
	{
		//Vector2 colliderSize = killArea.size;

		if(killArea.size.x < length)
		{
			Vector2 newKillAreaSize = new Vector2(killArea.size.x +  Time.deltaTime * flame.startSpeed * 1f, 1f); //Sadly the exact velocity of the particle is not exposed in the API. 0.7 is the current best guesstimate
			killArea.size = newKillAreaSize;
			Vector2 newKillAreaOffset = new Vector2(newKillAreaSize.x * 0.5f, 0f);
			killArea.offset = newKillAreaOffset;
		}
	}
	
	public void ShrinkKillArea() //Not entirely happy with this, but the alternative is to equip all particle with colliders.
	{
		Vector2 colliderSize = killArea.size;
		if(killArea.size.x > 1f)
		{
			float areaDelta = Time.deltaTime * flame.startSpeed * 1f;
			Vector2 newKillAreaSize = new Vector2(killArea.size.x -  areaDelta, 1f);
			killArea.size = newKillAreaSize;
			Vector2 newKillAreaOffset = new Vector2(killArea.offset.x + areaDelta * 0.5f, 0f);
			killArea.offset = newKillAreaOffset;
		}
		else
		{
			killArea.enabled = false;
		}
	}

	private float GetStateCompletionPercent()
	{
		float timeLeft = stateExitTime - Time.timeSinceLevelLoad;
		float stateTimeElapsed = stateDuration - timeLeft;
		float completionPercent = stateTimeElapsed / stateDuration;
		return completionPercent;
	}
	
	public void SetHazadrousLayer()
	{
		//gameObject.layer = 15;
		flame.Play();
		killArea.size = new Vector2(1f,1f);
		killArea.offset = new Vector2(0f,0f);
		killArea.enabled = true;
	}
	public void SetSafeLayer()
	{
		//gameObject.layer = 8;
		flame.Stop();
	}
}
