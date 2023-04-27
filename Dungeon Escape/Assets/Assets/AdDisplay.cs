using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdDisplay : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public string myGameIdAndroid = "5235558";
    public string myGameIdIOS = "5235559";
    public string adUnitIdAndroid = "Interstitial_Android";
    public string adUnitIdIOS = "Interstitial_iOS";
    public string myAdUnitId;
    public string myAdStatus = "";
    public bool adStarted;
    public bool adCompleted;

    private bool testMode = false;

    // Start is called before the first frame update
    void Awake()
    {
        #if UNITY_IOS
	                Advertisement.Initialize(myGameIdIOS, testMode, this);
	                myAdUnitId = adUnitIdIOS;
        #else
                Advertisement.Initialize(myGameIdAndroid, testMode, this);
                myAdUnitId = adUnitIdAndroid;
        #endif

    }

    void Start()
    {
        // Load the ad when the scene is started
        Advertisement.Load(myAdUnitId, this);
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        myAdStatus = message;
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        HandlePossibilityOfNotWorkingAd();
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
    }
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        myAdStatus = message;
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        HandlePossibilityOfNotWorkingAd();
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        myAdStatus = message;
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        HandlePossibilityOfNotWorkingAd();
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        adStarted = true;
        Debug.Log("Ad Started: " + adUnitId);
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        Debug.Log("Ad Clicked: " + adUnitId);
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        adCompleted = showCompletionState == UnityAdsShowCompletionState.COMPLETED;
        Debug.Log("Ad Completed: " + adUnitId);
        switch(showCompletionState)
        {
            case UnityAdsShowCompletionState.COMPLETED:
                Debug.Log("You watched the whole ad!");
                GameManager.Instance.player.AddGems(100, true);
                UIManager.Instance.ResetSelectionCursor();
                GameManager.Instance.DidNotWatchAd = false;
                break;
            case UnityAdsShowCompletionState.SKIPPED:
                Debug.Log("You skipped the ad, just 50 gems for you!");
                GameManager.Instance.player.AddGems(50, true);
                UIManager.Instance.ResetSelectionCursor();
                GameManager.Instance.DidNotWatchAd = false;
                break;
            case UnityAdsShowCompletionState.UNKNOWN:
                Debug.Log("The video failed, it must not have been ready");
                HandlePossibilityOfNotWorkingAd();
                break;
        }
        UIManager.Instance.getGemsButton.gameObject.SetActive(false);
    }

    public void ShowRewardedAd()
    {
        Debug.Log("Showing Rewarded Ad");
        if (!adStarted)
        {
            Advertisement.Show(myAdUnitId, this);
        }
        else
        {
            HandlePossibilityOfNotWorkingAd();
        }
    }

    public void HandlePossibilityOfNotWorkingAd()
    {
        Debug.Log("There was a problem showing the ad, reward player anyway with 100 gems");
        if (!GameManager.Instance.HandledAdNotWorking && GameManager.Instance.PlayerInShop)
        {
            GameManager.Instance.player.AddGems(100, true);
            UIManager.Instance.ResetSelectionCursor();
            GameManager.Instance.HandledAdNotWorking = true;
            UIManager.Instance.getGemsButton.gameObject.SetActive(false);
        }
    }
}