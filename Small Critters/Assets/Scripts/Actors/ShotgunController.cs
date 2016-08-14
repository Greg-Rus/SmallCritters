using UnityEngine;
using System.Collections;

public class ShotgunController : MonoBehaviour {
    public Transform muzzle;
    public GameObject aimZoneObject;
    public float range;
    public LayerMask shootableLayers;
    public GameObject pellet;
    public float pelletSpeed;
    public GameObject[] pellets;
    public Transform shotgunTransform;
    public float spread;
    public IPowerup powerup;

    private Animator animator;
    private TrailRenderer[] pelletTrailRenderers;
    private Rigidbody2D[] pelletRigidbodies;
    private Transform[] pelletTransforms;
    private IAudio audio;

    void Start () {
        GetPelletComponenets();
        powerup = ServiceLocator.getService<IPowerup>();
        animator = GetComponentInParent<Animator>();
        audio = ServiceLocator.getService<IAudio>();
    }
	
    public void Shoot()
    {
        FirePellets();
        StopCoroutine(CleanUpPelletsAfterSeconds(2f));
        StartCoroutine(CleanUpPelletsAfterSeconds(2f));
        powerup.OnShotFired();
        animator.SetTrigger("Shoot");
        audio.PlaySound(Sound.ShotgunBlastAndCock);
    }

    private void FirePellets()
    {
        for (int i = 0; i < pellets.Length; ++i)
        {
            pellets[i].SetActive(true);
            pelletTransforms[i].position = muzzle.transform.position;
            pelletTrailRenderers[i].Clear();
            Vector3 direction = (muzzle.transform.up * range) + (muzzle.transform.right * Random.Range(-spread, spread));
            direction = direction.normalized;
            pelletRigidbodies[i].velocity = Vector3.zero;
            pelletRigidbodies[i].AddForce(direction * pelletSpeed, ForceMode2D.Impulse);

        }
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
            pelletTrailRenderers[i].sortingOrder = 50;
            pelletTrailRenderers[i].sortingLayerName = "Frog";
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

    public void AimAtPosition(Vector3 position)
    {
        Vector3 vectorToPosition = (position - shotgunTransform.transform.position).normalized;
        float rot_z = Mathf.Atan2(vectorToPosition.y, vectorToPosition.x) * Mathf.Rad2Deg;
        shotgunTransform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }


}
