using UnityEngine;
using System.Collections;

public class CameraVerticalFollow : MonoBehaviour {
	public GameObject frog;
	private Vector3 newCameraPosition;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    // Use this for initialization
    void Start () {


    }
	
	// Update is called once per frame
	void Update () {
		followFrog();
	}
	private void followFrog()
	{
		if(frog != null)
		{
			newCameraPosition = this.transform.position;
			newCameraPosition.y = frog.transform.position.y;
			//this.transform.position = newCameraPosition;

            //Vector3 targetPosition = frog.transform.TransformPoint(new Vector3(0, 0, -10));
            transform.position = Vector3.SmoothDamp(transform.position, newCameraPosition, ref velocity, smoothTime);
        }

	}

    public void ShakeCamera()
    {
        StartCoroutine(UpdateCameraShakes());
    }
    private IEnumerator UpdateCameraShakes()
    {
        float OriginalCameraPostionX = this.transform.position.x;
        float offset = 0.1f;
        for (int i = 0; i < 10; ++i)
        {
            newCameraPosition.x = OriginalCameraPostionX + offset;
            newCameraPosition.y = frog.transform.position.y + offset;
            transform.position = newCameraPosition;
            offset = -offset;
            yield return null;
        }
    }
}
