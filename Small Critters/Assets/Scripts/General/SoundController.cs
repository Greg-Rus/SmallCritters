using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {
    
    public AudioSource myAudio;
    public AudioClip shotgunFireAndCock;
    public static SoundController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlayShotgunFire()
    {
        myAudio.PlayOneShot(shotgunFireAndCock);
    }
}
