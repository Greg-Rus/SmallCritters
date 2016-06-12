using UnityEngine;
using System.Collections;

public class CameraVerticalFollow : MonoBehaviour {
	public GameObject frog;
    public float smoothTime = 0.3f;
    public float shakeOffset = 0.1f;

    private Vector3 newCameraPosition;
    private Vector3 velocity = Vector3.zero;
    private float cameraXpoistion;

    void Start ()
    {
        cameraXpoistion = this.transform.position.x;
    }
	
	void Update ()
    {
		FollowFrog();
	}
	private void FollowFrog()
	{
        if (frog != null)
        {
            newCameraPosition = this.transform.position;
			newCameraPosition.y = frog.transform.position.y;
            transform.position = Vector3.SmoothDamp(transform.position, newCameraPosition, ref velocity, smoothTime);
        }
    }

    public void ShakeCamera()
    {
        StartCoroutine(UpdateCameraShakes());
    }
    private IEnumerator UpdateCameraShakes()
    {
        for (int i = 0; i < 10; ++i)
        {
            DisplaceCamera();
            yield return null;
        }
        RecenterCamera();
    }

    private void DisplaceCamera()
    {
        newCameraPosition.x = cameraXpoistion + shakeOffset;
        newCameraPosition.y = frog.transform.position.y + shakeOffset;
        transform.position = newCameraPosition;
        shakeOffset = -shakeOffset;
    }

    private void RecenterCamera()
    {
        newCameraPosition.x = cameraXpoistion;
        newCameraPosition.y = frog.transform.position.y;
        transform.position = newCameraPosition;
    }
}
