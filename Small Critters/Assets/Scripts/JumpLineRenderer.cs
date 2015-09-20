using UnityEngine;
using System.Collections;

public class JumpLineRenderer : MonoBehaviour {
	private LineRenderer lineRenderer;
	//public GameObject jumpMarkerPrefab;
	public GameObject jumpMarker;
	private RaycastHit2D hit;
	private JumpMarkerSensor jumpMarkerSensor;
	private JumpPathSensor jumpPathSensor;
	// Use this for initialization
	void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
		jumpMarkerSensor = GetComponentInChildren<JumpMarkerSensor>();
		jumpPathSensor = GetComponentInChildren<JumpPathSensor>();
	}
	void Start () {
		//lineRenderer = GetComponent<LineRenderer>();
		jumpMarker.SetActive(false);

	}
	
	// Update is called once per frame
	public void setupJumpLine(Vector2 jumpStartPosition)
	{
		jumpMarker.SetActive(true);
		jumpMarker.transform.position = this.transform.position;
		jumpMarker.transform.rotation = this.transform.rotation;
		lineRenderer.SetVertexCount(2);
		lineRenderer.SetPosition(0, this.transform.position);

	}
	public void updateJumpLine(Vector3 dragVector)
	{
		jumpMarker.transform.position = this.transform.position + dragVector;
		jumpMarker.transform.rotation = gameObject.transform.rotation;
		lineRenderer.SetPosition(0, this.transform.position);
		lineRenderer.SetPosition(1, jumpMarker.transform.position);
		if(willDieIfJumps(dragVector))
		{
			lineRenderer.SetColors(Color.red,Color.red);
		}
		else
		{
			lineRenderer.SetColors(Color.white,Color.white);
		}
	}
	public void stopDrawingJumpLine()
	{
		jumpMarkerSensor.reset();
		jumpMarker.SetActive(false);
		lineRenderer.SetVertexCount(0);
	}
	
	public bool willDieIfJumps(Vector3 dragVector)
	{
		bool result = jumpMarkerSensor.checkForHazardsInLandingZone() || jumpPathSensor.checkForHazardsInJumpPath(dragVector);
		//Debug.Log ("Will die: " + result);
		return result;
	}
	
}
