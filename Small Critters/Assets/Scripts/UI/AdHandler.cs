using UnityEngine;
using System.Collections;
using System;
using GoogleMobileAds.Api;

public class AdHandler : MonoBehaviour {
    public GameObject pleaseWaitPanel;
    public string adUnitId;
    InterstitialAd interstitial;
    UIHandler myUIHandler;
    bool isAdFinished = true;
    IAudio myAudio;
    // Use this for initialization
    void Awake ()
    {
        myUIHandler = GetComponent<UIHandler>();
    }

    void Start()
    {
        myAudio = ServiceLocator.getService<IAudio>();
    }

    public void ShowInterstitialAd()
    {
        isAdFinished = false;
        myAudio.PauseAudio();
        interstitial = new InterstitialAd(adUnitId);
        interstitial.OnAdLoaded += HandleOnAdLoaded;
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        interstitial.OnAdClosed += HandleOnAdClosed;
        
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
            .AddTestDevice("212F4933DB3419310DFEF94DC783D96D")  // K10000
            .Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
        pleaseWaitPanel.SetActive(true);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        interstitial.Show();
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        isAdFinished = true;
        interstitial.Destroy();
        StartCoroutine(ProcessAdWatched());
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        isAdFinished = true;
        interstitial.Destroy();
        StartCoroutine(ProcessAdWatched());
    }

    private bool PollAdDisplayState()
    {
        return isAdFinished;
    }

    IEnumerator ProcessAdWatched()
    {
        yield return new WaitUntil(PollAdDisplayState);
        myAudio.UnPauseAudio();
        pleaseWaitPanel.SetActive(false);
        myUIHandler.AdWatched();
    }
}
