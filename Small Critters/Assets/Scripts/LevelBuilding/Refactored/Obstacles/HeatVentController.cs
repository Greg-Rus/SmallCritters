using UnityEngine;
using System.Collections;

public class HeatVentController : MonoBehaviour {

	
	public GameObject lightShaft;
	public GameObject lightTip;
	public float lenght = 4.5f;
	public Vector3 direction = Vector3.right;
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
	public float flameLengthOffset = 0.3f;
	Vector3 tipStartPosition;
	
	public float closedTime = 2f;
	public float ventDoorTime= 0.5f;
	public float warmupTime = 1f;
	public float ventingTime = 2f;
	
	public float stateExitTime;
	public HeatVentState state = HeatVentState.Closed;
	
	
	// Use this for initialization
	void Start () {
		Vector3 newScale = new Vector3(lenght*2f,1f,1f);
		lightShaft.transform.localScale = newScale;
		lightTipHalfWidth = lightTip.GetComponent<SpriteRenderer>().bounds.size.x * 0.5f;

		thicknessFrom = maxShaftScale;
		thicknessTo = minShaftScale;
		flame.startLifetime = flameLengthOffset + lenght* 0.1f;
		
		tipStartPosition = lightTip.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateVentigState();
	}
	
	public void configure()
	{
		
	}
	
	private void UpdateVentigState()
	{
		UpdateFlickerCycle();
		UpdateShaftThickness();
		UpdateTipWidth();
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
	
	private void UpdateShaftThickness()
	{
		float newThickness =  Mathf.Lerp(thicknessFrom, thicknessTo, flickerTime);
		Vector3 newShaftThickness = new Vector3(lenght*2f, newThickness, lightShaft.transform.localScale.z);
		lightShaft.transform.localScale = newShaftThickness;
		Vector3 newTipThickness = new Vector3(lightTip.transform.localScale.x, newThickness, lightTip.transform.localScale.z);
		lightTip.transform.localScale = newTipThickness;
	}
	
	
	private void UpdateTipWidth()
	{
		float newWidth =  Mathf.Lerp(tipWidthFrom, tipWidthTo, flickerTime);
		Vector3 newTipThickness = new Vector3(newWidth, lightTip.transform.localScale.y, lightTip.transform.localScale.z);
		lightTip.transform.localScale = newTipThickness;

		Vector3 offset = transform.right * ( lenght + (lightTipHalfWidth * newWidth));
		Vector3 newTipPosition =  tipStartPosition + offset;

		lightTip.transform.position = newTipPosition;
	}
}
