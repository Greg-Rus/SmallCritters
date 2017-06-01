using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogParticleSystemInjector : MonoBehaviour {
    public ParticleSystem healthParticleEffect;
    public ParticleSystem powerupParticleEffect;
	// Use this for initialization
	void Start () {
        ServiceLocator.getService<IPowerup>().powerupFullEffect = powerupParticleEffect;
        ServiceLocator.getService<IUI>().heartFilledEffect = healthParticleEffect;
	}

}
