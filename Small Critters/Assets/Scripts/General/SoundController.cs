using UnityEngine;
using System.Collections;

public enum Sound {Silence, Jump, ShotgunBlast, BeeStunHit, FullHeart, StarPickup, BeatleSpit, BeeCharge };

public class SoundController : MonoBehaviour {
    
    public AudioSource myAudio;
    public AudioClip shotgunFireAndCock;
    public AudioClip[] jumps;
    public AudioClip beeStunHit;
    public AudioClip fullHeart;
    public AudioClip starPickup;
    public AudioClip beatleSpit;
    public AudioClip beeCharge;
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

    public void PlaySound(Sound sound)
    {
        switch (sound)
        {
            case Sound.Jump: PlayJumpSound(); break;
            case Sound.ShotgunBlast: myAudio.PlayOneShot(shotgunFireAndCock); break;
            case Sound.BeatleSpit: myAudio.PlayOneShot(beatleSpit); break;
            case Sound.BeeCharge: myAudio.PlayOneShot(beeCharge); break;
            case Sound.BeeStunHit: myAudio.PlayOneShot(beeStunHit); break;
            case Sound.FullHeart: myAudio.PlayOneShot(fullHeart); break;
            case Sound.StarPickup: myAudio.PlayOneShot(starPickup); break;
            case Sound.Silence: myAudio.Stop();break;
            default: break;
        }
    }

    private void PlayJumpSound()
    {
        myAudio.PlayOneShot(jumps[UnityEngine.Random.Range(0,jumps.Length)]);
    }
}
