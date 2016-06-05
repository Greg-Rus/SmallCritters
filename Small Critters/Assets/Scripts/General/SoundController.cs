using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {
    
    public AudioSource myAudio;
    public AudioClip shotgunFireAndCock;
    public static SoundController instance;
    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start () {
	
	}

    // Update is called once per frame
    //void Update () {

    //}
    public void PlayShotgunFire()
    {
        myAudio.PlayOneShot(shotgunFireAndCock);
    }
}
