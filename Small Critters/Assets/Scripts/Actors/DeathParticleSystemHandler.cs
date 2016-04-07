using UnityEngine;
using System.Collections;

public class DeathParticleSystemHandler : MonoBehaviour {

    public GameObject deathByFireParticles;
    public GameObject deathByForceParticles;
    public GameObject deathByColdParticles;

    public void OnDeath(string causeOfDeath)
    {
        //Debug.Log(causeOfDeath);
        switch (causeOfDeath)
        {
            case "Flame" : SpawnParticleSystem(deathByFireParticles); break;
            case "Processor": SpawnParticleSystem(deathByFireParticles); break;
            case "ColdFog": SpawnParticleSystem(deathByColdParticles); break;
            default: SpawnParticleSystem(deathByForceParticles); break;
        }

        //if (causeOfDeath == "Flame" || causeOfDeath == "Processor")
        //{
        //    SpawnParticleSystem(deathByFireParticles);
        //}
        
        //else
        //{
        //    SpawnParticleSystem(deathByForceParticles);
        //}
    }

    private void SpawnParticleSystem(GameObject system)
    {
        GameObject newParticleSystem = Instantiate(system, this.transform.position, Quaternion.identity) as GameObject;
        Destroy(newParticleSystem, 3f);
    }


}
