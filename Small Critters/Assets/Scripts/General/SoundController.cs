using UnityEngine;
using System.Collections;



public class SoundController : MonoBehaviour, IAudio {
    
    public AudioSource myAudio;
    public AudioSource myMusic;

    public AudioClip mainMusic;
    public AudioClip powerupMusic;
    private AudioClip currentMusic;
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
    public AudioClip notification;
    public AudioClip summaryText;
    public AudioClip summaryMultiplyer;
    public AudioClip summaryScore;
   
    //public SoundController instance;

    private IPowerup powerupStatus;
    public bool isMusicOn;
    public bool isSoundFXOn;

    //void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //}

    void Start()
    {
        currentMusic = mainMusic;
        powerupStatus = ServiceLocator.getService<IPowerup>();
        
        if (PlayerPrefs.GetInt("Music") == (int)Toggled.On)
        {
            SetMusicOn(true);
        }
        else
        {
            SetMusicOn(false);
        }

        if (PlayerPrefs.GetInt("SoundFX") == (int)Toggled.On)
        {
            SetSoundFXOn(true);
        }
        else
        {
            SetSoundFXOn(false);
        }
        

    }

    public void PlayShotgunFire()
    {
        StartCoroutine(ShotgunFireAndCock());
    }

    public void PlaySound(Sound sound)
    {
        if (isSoundFXOn)
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
                case Sound.PlayerKilled: myAudio.Stop(); myMusic.Stop(); myAudio.PlayOneShot(playerKilled); break;
                case Sound.EatFly: myAudio.PlayOneShot(eatFly); break;
                case Sound.Silence: myAudio.Stop(); break;
                case Sound.Notification: myAudio.PlayOneShot(notification); break;
                case Sound.SummaryText: myAudio.PlayOneShot(summaryText); break;
                case Sound.SummaryMutliplyer: break;//myAudio.pitch = 1.5f; myAudio.PlayOneShot(summaryText); break;
                case Sound.SummaryScore: break;// myAudio.pitch = 2f; myAudio.PlayOneShot(summaryText); break;
                default: break;
            }
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
        if (isSoundFXOn)
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
    }

    public void PauseAudio()
    {
        myAudio.Pause();
        myMusic.Pause();
    }
    public void UnPauseAudio()
    {
        myAudio.UnPause();
        myMusic.UnPause();
    }
    public void SetMusicOn(bool state)
    {
        if (state == true)
        {
            isMusicOn = true;
            myMusic.clip = currentMusic;
            myMusic.Play();
        }
        else
        {
            isMusicOn = false;
            myMusic.Stop();
        }
        
    }
    public void SetSoundFXOn(bool state)
    {
        if (state == true)
        {
            //myAudio.Play();
            isSoundFXOn = true;
        }
        else
        {
            isSoundFXOn = false;
        }
        
    }

    public void PlayPowerupMusic(bool powerUpOn)
    {
        if (powerUpOn)
        {
            currentMusic = powerupMusic;
            myMusic.clip = currentMusic;
            myMusic.Play();
        }
        else
        {
            currentMusic = mainMusic;
            myMusic.clip = currentMusic;
            myMusic.Play();
        }
    }

    IEnumerator ShotgunFireAndCock()
    {
        myAudio.PlayOneShot(shotgunFire);
        yield return new WaitForSeconds(0.2f);
        myAudio.PlayOneShot(shotgunCock);
    }
}
