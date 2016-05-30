using UnityEngine;
using System.Collections;

public class ShotgunController : MonoBehaviour {
    public Transform muzzle;
    public float targetAcquisition;
    public float targetAcquisitionSpeed;
    public GameObject aimZoneObject;
    private Transform aimZone;
    public Vector3 aimZoneStartScale;
    public Vector3 aimZoneFinalScale;
    public float range;
    public LayerMask shootableLayers;
    public GameObject pellet;
    public float pelletSpeed;
    public GameObject[] pellets;
    private TrailRenderer[] pelletTrailRenderers;
    private Rigidbody2D[] pelletRigidbodies;
    private Transform[] pelletTransforms;
    public float spread;
    public PowerupHandler powerupHandler;
    // Use this for initialization
    void Start () {
        aimZone = aimZoneObject.transform;
        GetPelletComponenets();
    }
	
	// Update is called once per frame
	void Update () {
        CheckForEnemy();
	}

    private void CheckForEnemy()
    {
        RaycastHit2D target = Physics2D.Raycast(muzzle.transform.position, muzzle.transform.up, range, shootableLayers);
        //Debug.DrawRay(transform.position, Vector2.up * range, Color.red);
        if (target.collider != null)
        {
            if (targetAcquisition == 0)
            {
                aimZoneObject.SetActive(true);
            }
            UpdateTargetAcquisition(targetAcquisitionSpeed);
            if (targetAcquisition >= 1f)
            {
                Shoot();
            }
        }
        else if (targetAcquisition > 0)
        {
            UpdateTargetAcquisition(-targetAcquisitionSpeed * 2f);
            if (targetAcquisition < 0)
            {
                targetAcquisition = 0f;
                aimZoneObject.SetActive(false);
            }
        }
    }

    private void UpdateTargetAcquisition(float speed)
    {
        targetAcquisition += Time.deltaTime * speed;
        UpdateAimZone();
    }

    private void Shoot()
    {
        FirePellets();
        StopCoroutine(CleanUpPelletsAfterSeconds(2f));
        StartCoroutine(CleanUpPelletsAfterSeconds(2f));
        aimZoneObject.SetActive(false);
        targetAcquisition = 0f;
        powerupHandler.OnShotFired();
    }

    private void UpdateAimZone()
    {
        aimZone.localScale = Vector3.Lerp(aimZoneStartScale, aimZoneFinalScale, targetAcquisition);
    }

    private void FirePellets()
    {
        for (int i = 0; i < pellets.Length; ++i)
        {
            pellets[i].SetActive(true);
            pelletTransforms[i].position = muzzle.transform.position;
            pelletTrailRenderers[i].Clear();
            Vector3 direction = muzzle.(transform.up * range) + (muzzle.transform.right * Random.Range(-spread, spread));
            direction = direction.normalized;
            pelletRigidbodies[i].velocity = Vector3.zero;
            pelletRigidbodies[i].AddForce(direction * pelletSpeed, ForceMode2D.Impulse);

        }
        //pellet.SetActive(true);
        //pellet.transform.position = transform.position;
        //pellet.GetComponent<TrailRenderer>().Clear();
        //pellet.GetComponent<Rigidbody2D>().AddForce(transform.up * pelletSpeed, ForceMode2D.Impulse);
    }

    private void GetPelletComponenets()
    {
        pelletTrailRenderers = new TrailRenderer[pellets.Length];
        pelletRigidbodies = new Rigidbody2D[pellets.Length];
        pelletTransforms = new Transform[pellets.Length];

        for (int i = 0; i < pellets.Length; ++i)
        {
            pelletTrailRenderers[i] = pellets[i].GetComponent<TrailRenderer>();
            pelletRigidbodies[i] = pellets[i].GetComponent<Rigidbody2D>();
            pelletTransforms[i] = pellets[i].transform;
        }
    }

    private IEnumerator CleanUpPelletsAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        for (int i = 0; i < pellets.Length; ++i)
        {
            pellets[i].SetActive(false);
        }
    }


}
