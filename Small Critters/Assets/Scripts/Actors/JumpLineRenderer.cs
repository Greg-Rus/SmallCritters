using UnityEngine;
using System.Collections;

public class JumpLineRenderer : MonoBehaviour {
	private LineRenderer lineRenderer;
	public GameObject jumpMarker;
	private RaycastHit2D hit;
	private JumpMarkerSensor jumpMarkerSensor;
	private JumpPathSensor jumpPathSensor;
	public bool isStarted = false;
    private PowerupHandler powerup;
    public Sprite jumpSprite;
    public Sprite targetSprite;
    public SpriteRenderer markerRenderer;
    private bool targetSpriteActive;
    private ShotgunController shotgun;

	void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.sortingOrder = 50;
		jumpMarkerSensor = GetComponentInChildren<JumpMarkerSensor>();
		jumpPathSensor = GetComponentInChildren<JumpPathSensor>();
        powerup = ServiceLocator.getService<PowerupHandler>();
        shotgun = GetComponentInChildren<ShotgunController>(true);
    }
	void Start () {
		jumpMarker.SetActive(false);
    }

	public void setupJumpLine(Vector2 jumpStartPosition)
	{
		jumpMarker.SetActive(true);
		jumpMarker.transform.position = this.transform.position;
		jumpMarker.transform.rotation = this.transform.rotation;
		lineRenderer.SetVertexCount(2);
		lineRenderer.SetPosition(0, this.transform.position);
		isStarted = true;

	}
	public void updateJumpLine(Vector3 dragVector)
	{
		jumpMarker.transform.position = this.transform.position + dragVector;
		jumpMarker.transform.rotation = gameObject.transform.rotation;
		lineRenderer.SetPosition(0, this.transform.position);
		lineRenderer.SetPosition(1, jumpMarker.transform.position);
        if (powerup.powerupModeOn)
        {
            if (CanFireAtTarget(dragVector))
            {
                if (!targetSpriteActive)
                {
                    ShowTargetSprite(true);
                }
                shotgun.AimAtPosition(jumpMarker.transform.position);
            }
            else if (!CanFireAtTarget(dragVector) && targetSpriteActive)
            {
                ShowTargetSprite(false);
            }
        }

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
		jumpMarker.SetActive(false);
		lineRenderer.SetVertexCount(0);
		isStarted = false;
	}
	
	public bool willDieIfJumps(Vector3 dragVector)
	{
		bool result = jumpMarkerSensor.checkForHazardsInLandingZone() || jumpPathSensor.checkForHazardsInJumpPath(dragVector);
		return result;
	}
    public bool CanFireAtTarget(Vector3 dragVector)
    {
        bool result = jumpMarkerSensor.CheckForTargetsInLandingZone();
        return result;
    }
    public void ShowTargetSprite(bool showSprite)
    {
        if (showSprite)
        {
            markerRenderer.sprite = targetSprite;
            targetSpriteActive = true;
        }
        else
        {
            markerRenderer.sprite = jumpSprite;
            targetSpriteActive = false;
        }
    }
}
