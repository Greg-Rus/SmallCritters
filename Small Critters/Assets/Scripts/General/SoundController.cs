using UnityEngine;
using System.Collections;



public class SoundController : MonoBehaviour, IAudio {
    
    public AudioSource myAudio;

    public AudioClip mainMusic;
    public AudioClip powerupMusic;
    public AudioClip shotgunFireAndCock;
    public AudioClip shotgunFire;
    public AudioClip shotgunCock;
    public AudioClip[] jumps;
    public AudioClip beeStunHit;
    public AudioClip beeCharge;
    public AudioClip fullHeart;
    public AudioClip starPickup;
    public AudioClip beatleCharge;
    public AudioClip beatleSpit;
    public AudioClip eatFly;
    public AudioClip startPowerup;
    public AudioClip killedByFire;
    public AudioClip killedByImpact;
    public AudioClip playerHit;
    public AudioClip playerKilled;
    public AudioClip powerupJump;
   
    public SoundController instance;

    private IPowerup powerupStatus;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        powerupStatus = ServiceLocator.getService<IPowerup>();
    }

    public void PlayShotgunFire()
    {
        StartCoroutine(ShotgunFireAndCock());
    }

    public void PlaySound(Sound sound)
    {
        switch (sound)
        {
            case Sound.Jump: PlayJumpSound(); break;
            case Sound.ShotgunBlastAndCock: PlayShotgunFire(); break;
            case Sound.BeatleSpit: myAudio.PlayOneShot(beatleSpit); break;
            case Sound.BeeCharge: myAudio.PlayOneShot(beeCharge); break;
            case Sound.BeeStunHit: myAudio.PlayOneShot(beeStunHit); break;
            case Sound.FullHeart: myAudio.PlayOneShot(fullHeart); break;
            case Sound.StarPickup: myAudio.PlayOneShot(starPickup); break;
            case Sound.StartPowerup: myAudio.PlayOneShot(startPowerup); break;
            case Sound.KilledByFire: myAudio.PlayOneShot(killedByFire); break;
            case Sound.KilledByImpact: myAudio.PlayOneShot(killedByImpact); break;
            case Sound.PlayerHit: myAudio.PlayOneShot(playerHit); break;
            case Sound.PlayerKilled: myAudio.PlayOneShot(playerKilled); break;
            case Sound.EatFly: myAudio.PlayOneShot(eatFly); break;
            case Sound.Silence: myAudio.Stop();break;
            default: break;
        }
    }

    private void PlayJumpSound()
    {
        if (!powerupStatus.powerupModeOn)
        {
            myAudio.PlayOneShot(jumps[UnityEngine.Random.Range(0, jumps.Length)]);
        }
        else
        {
            myAudio.PlayOneShot(powerupJump);
        }
        
    }

    public void PlayEnemyDeathSound(string causeOfDeath)
    {
        switch (causeOfDeath)
        {
            case "Blade": myAudio.PlayOneShot(killedByImpact); break;
            case "Flame": myAudio.PlayOneShot(killedByFire); break;
            case "Sting": myAudio.PlayOneShot(killedByImpact); break;
            case "Processor": myAudio.PlayOneShot(killedByFire); break;
            case "Pellet": myAudio.PlayOneShot(killedByImpact); break;
            case "FlameBall": myAudio.PlayOneShot(killedByFire); break;
        }
    }

    IEnumerator ShotgunFireAndCock()
    {
        myAudio.PlayOneShot(shotgunFire);
        yield return new WaitForSeconds(0.2f);
        myAudio.PlayOneShot(shotgunCock);
    }
}
