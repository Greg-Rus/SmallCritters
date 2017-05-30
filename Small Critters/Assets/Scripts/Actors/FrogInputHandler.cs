using UnityEngine;
using System.Collections;

public class FrogInputHandler : MonoBehaviour {
	public bool inputStarted = false;
	public Vector3 startPointerScreenPositoin;
	public Vector3 draggedPointerScreenPosition;
	public Vector3 dragVector;
	public float minimalDragDistance;
    private float minimalDragScreenDistance;
	private Imovement frogMovement;
	public JumpLineRenderer jumpLineRenderer;
	public float maxJumpSwipe;
	public float maxJumpLength;
	private float swipeToJumpConversionRatio;
	private Vector3 worldStartPoint;
	private Vector3 worldDraggedPoint;
    public float swipeDirectionModifier;
    private ShotgunController shotgun;
    private IPowerup powerupHandler;
    private bool reloading = false;
    public LayerMask fireingTargetMask;

    void Awake ()
    {
        frogMovement = GetComponent<Imovement>() as Imovement ;
        shotgun = GetComponentInChildren<ShotgunController>(true);
        swipeToJumpConversionRatio = maxJumpLength / maxJumpSwipe;
        minimalDragScreenDistance = (Camera.main.WorldToScreenPoint(new Vector3(minimalDragDistance, 0f, 0f)) - Camera.main.WorldToScreenPoint(Vector3.zero)).sqrMagnitude;
	}

    void Start()
    {
        powerupHandler = ServiceLocator.getService<IPowerup>();
        swipeDirectionModifier = PlayerPrefs.GetInt("SwipeControlls");
    }

	void Update()
	{
#if UNITY_STANDALONE
        sqrMouseDistanceFromFrog = (Input.mousePosition - Camera.main.WorldToScreenPoint(this.transform.position)).sqrMagnitude;
        if ((sqrMouseDistanceFromFrog >= minimalDragScreenDistance) && (!frogMovement.midJump))
        {
            if (!jumpLineRenderer.isStarted)
            {
                jumpLineRenderer.setupJumpLine(this.transform.position);
            }
            pointVector = calcualteDragVector(Camera.main.WorldToScreenPoint(this.transform.position), Input.mousePosition);
            frogMovement.rotateToDirection(pointVector);
            jumpLineRenderer.updateJumpLine(pointVector);
        }
        else
        {
            if (jumpLineRenderer.isStarted)
            {
                jumpLineRenderer.stopDrawingJumpLine();
            }
        }
#endif
        if (!frogMovement.midJump && Time.timeScale > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPointerScreenPositoin = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                draggedPointerScreenPosition = Input.mousePosition;
                dragVector = CalcualteDragVector(startPointerScreenPositoin, draggedPointerScreenPosition);
                if ((draggedPointerScreenPosition - startPointerScreenPositoin).sqrMagnitude > minimalDragScreenDistance)
                {
                    if (!jumpLineRenderer.isStarted)
                    {
                        jumpLineRenderer.setupJumpLine(this.transform.position);
                    }
                    frogMovement.rotateToDirection(dragVector);
                    jumpLineRenderer.updateJumpLine(dragVector);
                }
                else
                {
                    if (jumpLineRenderer.isStarted)
                    {
                        jumpLineRenderer.stopDrawingJumpLine();
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (dragVector.sqrMagnitude > Mathf.Pow(minimalDragDistance, 2f))
                {
                    frogMovement.rotateToDirection(dragVector);
                    if (powerupHandler.powerupModeOn && jumpLineRenderer.CanFireAtTarget(dragVector) && !reloading)
                    {
                        shotgun.Shoot();
                        reloading = true;
                    }
                    else
                    {
                        frogMovement.makeMove(dragVector);
                    }
                }
                else //if(dragVector.sqrMagnitude <= Mathf.Pow(minimalDragDistance, 2f) && powerupHandler.powerupModeOn && !reloading)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, fireingTargetMask);
                    if (hit)
                    {
                        StartCoroutine(RotateAndFire());
                    }
                }
                jumpLineRenderer.stopDrawingJumpLine();
            }
        }
	}

	Vector3 CalcualteDragVector(Vector3 start, Vector3 end)
	{
		CalculateSwipesWorldCoordinates(start, end);
		
		dragVector = (worldDraggedPoint - worldStartPoint) * swipeDirectionModifier;
		if(dragVector.magnitude <= maxJumpSwipe)
		{
			return dragVector * swipeToJumpConversionRatio;
		}
		else
		{
			return dragVector.normalized * maxJumpSwipe * swipeToJumpConversionRatio;
		}
	}
	void CalculateSwipesWorldCoordinates(Vector3 start, Vector3 end)
	{
		worldStartPoint = Camera.main.ScreenToWorldPoint(start);
		worldStartPoint.z = 0f;
		worldDraggedPoint = Camera.main.ScreenToWorldPoint(end);
		worldDraggedPoint.z = 0f;
	}

    public void SwipeDirectionChange(SwipeDirection direction)
    {
        swipeDirectionModifier = (direction == SwipeDirection.Forward) ? 1f : -1f;
    }

    public void ReloadComplete()
    {
        reloading = false;
    }

    IEnumerator RotateAndFire()
    {
        Vector3 targetWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetWorldPosition.z = 0f;
        Vector3 frogWorldPosition = transform.position;
        frogWorldPosition.z = 0f;
        frogMovement.rotateToDirection(targetWorldPosition - frogWorldPosition);

        yield return new WaitForFixedUpdate();

        if (powerupHandler.powerupModeOn && !reloading)
        {
            shotgun.Shoot();
            reloading = true;
        }
    }
}
