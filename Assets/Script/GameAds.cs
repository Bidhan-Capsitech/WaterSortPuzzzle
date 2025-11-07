using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameAds : MonoBehaviour
{
    public GeneralGameSettings m_gamesettings;
    public static GameAds instance;

    private string game_id, interstitial_id, banner_id, rewarded_id;
    private bool testMode;


    private BannerView banner_view;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        else
        {
            instance = this;
        }
    }

    public void initializeAdmobSdk()
    {

        game_id = m_gamesettings.gameId;
        interstitial_id = m_gamesettings.InterstitialAdId;
        banner_id = m_gamesettings.BannerAd_ID;
        rewarded_id = m_gamesettings.rewardAdId;

        int remove_ads_status = PlayerPrefs.GetInt("isPlayer_BuyNoAds", 0);

        MobileAds.Initialize(initStatus => { });

        MobileAds.SetApplicationMuted(true);
        MobileAds.SetApplicationVolume(0);

        requestToload_RewardedAd();

        if (check_RemoveAds_Status_toShow() == true)  // if its true then you are able to show ads, the player doesnt buy the item yet
        {
            RequestBanner();
            RequestInterstitial();
        }

    }



    private void Start()
    {
        initializeAdmobSdk();
    }


    bool check_RemoveAds_Status_toShow()
    {
        int remove_ads_status = PlayerPrefs.GetInt("isPlayer_BuyNoAds", 0);

        if (remove_ads_status == 0)
        {
            //show ads 
            return true;
        }
        else
        {
            return false;
        }

    }


    AdRequest AdRequestBuild()
    {
        int gdpr_status = PlayerPrefs.GetInt("gdpr_status", 0);

        if (gdpr_status == -1)
        {
            gdpr_status = 0;
        }

        AdRequest request = new AdRequest
        {
            Extras = new Dictionary<string, string> { { "npa", gdpr_status.ToString() } }
        };
        return request;

    }


    //*************************************************************** baner ad
    #region  banner ad

    bool is_banner_showed = false;
    public void showbannerAD()
    {

        if (check_RemoveAds_Status_toShow() == true)
        {
            banner_view.Show();
            is_banner_showed = true;
        }
    }

    public bool isBannerShowing()
    {
        return is_banner_showed;
    }

    private void RequestBanner()
    {
        // Create an empty ad request.
        AdRequest request = AdRequestBuild();
        // Create a 320x50 banner at the top of the screen.
        banner_view = new BannerView(banner_id, AdSize.Banner, AdPosition.Bottom);
        // Load the banner with the request.
        banner_view.LoadAd(request);
    }

    #endregion


    //*************************************************************** interstitial ad
    #region  interstitial Ad


    public void ShowInterstitialAd()
    {
        if (check_RemoveAds_Status_toShow() && interstitial != null && interstitial.CanShowAd())
        {
            interstitial.Show();
        }
        else
        {
            Debug.Log("Interstitial ad not ready — reloading...");
            RequestInterstitial();
        }
    }

    private void RequestInterstitial()
    {
       AdRequest request = AdRequestBuild();

    InterstitialAd.Load(interstitial_id, request, (InterstitialAd ad, LoadAdError error) =>
    {
        if (error != null || ad == null)
        {
            Debug.LogError("Failed to load interstitial ad: " + error);
            return;
        }

        interstitial = ad;

        // Optional: subscribe to full screen content events
        interstitial.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad closed.");
            RequestInterstitial(); // reload after close
        };

        interstitial.OnAdFullScreenContentFailed += (AdError err) =>
        {
            Debug.Log("Interstitial ad failed to show: " + err);
        };

        Debug.Log("Interstitial ad loaded successfully!");
    });
    }
    #endregion



    #region   reward_ads

    void requestToload_RewardedAd()
    {
        AdRequest request = AdRequestBuild();

        RewardedAd.Load(rewarded_id, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.Log("Rewarded ad failed to load: " + error);
                StartCoroutine(reload_reward());
                return;
            }

            rewardedAd = ad;

            // Subscribe to events
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded ad closed.");
                requestToload_RewardedAd(); // Reload next ad
            };

            rewardedAd.OnAdFullScreenContentFailed += (AdError err) =>
            {
                Debug.Log("Rewarded ad failed to show: " + err);
            };

            Debug.Log("Rewarded ad loaded successfully!");
        });
    }

    void AddBottleReward()
    {
        if (GameScManger.instance != null)
        {
            Debug.Log("Reward confirmed — adding bottle!");
            GameScManger.instance.reward_player_addBottle();
        }
        else
        {
            Debug.LogError("GameScManger.instance is null! Cannot add bottle.");
        }
    }

    public void showreward_Ad()
    {
        //if (this.rewardedAd.IsLoaded())
        //{
        //    this.rewardedAd.Show();
        //}

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("User earned reward!");
                AddBottleReward();
                requestToload_RewardedAd(); // reload next ad
            });
        }
        else
        {
            Debug.Log("Rewarded ad not ready, reloading...");
            requestToload_RewardedAd();
        }
    }


    /*void subscribe_reward_Events()
    {
        this.rewardedAd = new RewardedAd(rewarded_id);

        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
    }
    */

    /*public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        // the ads field to load 

        StartCoroutine(reload_reward());

        //load after 2 seconds
    }*/


    IEnumerator reload_reward()
    {
        yield return new WaitForSeconds(2.0f);
        requestToload_RewardedAd();
    }


  /*  public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("reward add field to show");
    }*/



   /* public void HandleUserEarnedReward(object sender, Reward args)
    {
        //reward the user
        getuserRewarded();

    }*/


    void getuserRewarded()
    {
        //int status_reward = PlayerPrefs.GetInt("reward_stats", -1);

        //if (status_reward == 1)
        //{

        //    GameScManger.instance.reward_player_addBottle();

        //}

        //requestToload_RewardedAd();

        int status_reward = PlayerPrefs.GetInt("reward_stats", -1);
        if (status_reward == 1)
        {
            Debug.Log("Reward confirmed — adding bottle!");
            GameScManger.instance.reward_player_addBottle();
            PlayerPrefs.SetInt("reward_stats", -1); // reset
        }

        requestToload_RewardedAd();
    }

    #endregion


}
