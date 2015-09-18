using UnityEngine;
using System.Collections;

public class JumpLineRenderer : MonoBehaviour {
	private LineRenderer lineRenderer;
	public GameObject jumpMarkerPrefab;
	private GameObject jumpMarker;
	private RaycastHit2D hit;
	private JumpMarkerSensor jumpMarkerSensor;
	// Use this for initialization
	void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}
	void Start () {
		//lineRenderer = GetComponent<LineRenderer>();
		jumpMarker = Instantiate(jumpMarkerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		jumpMarker.SetActive(false);
		jumpMarkerSensor = jumpMarker.GetComponent<JumpMarkerSensor>();
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
		if(willDieOnLanding(dragVector))
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
	
	public bool willDieOnLanding(Vector3 dragVector)
	{
		return jumpMarkerSensor.checkForHazards(dragVector);
	}
	
}
