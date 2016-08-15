using UnityEngine;
using System.Collections;
using System;
using GoogleMobileAds.Api;

public class AdHandler : MonoBehaviour {
    public string adUnitId;
    InterstitialAd interstitial;
    UIHandler myUIHandler;
    // Use this for initialization
    void Awake ()
    {
        myUIHandler = GetComponent<UIHandler>();
    }

    public void ShowInterstitialAd()
    {
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
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        interstitial.Show();
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        interstitial.Destroy();
        myUIHandler.AdWatched();
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        Debug.Log("Close event registered");
        Debug.LogError("Close event registered");
        interstitial.Destroy();
        myUIHandler.AdWatched();
    }

}
