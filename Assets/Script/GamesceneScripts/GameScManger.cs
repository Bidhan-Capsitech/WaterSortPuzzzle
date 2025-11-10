using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class GameScManger : MonoBehaviour
{

    public static GameScManger instance;

    public GeneralGameSettings generalGameSettings;

    //ui pannels
    public GameObject winPanel, notEnoughGames, addCoinsAfterBottleFilled;
    public AudioClip winSound, Click_sound;
    public AudioSource game_AudioSOurce;
    public AudioListener mainLisitner_audio;
    public ParticleSystem winParticle;
    public ParticleSystem winSkyshot;
    public GameObject bgCanvasPanel;
    public GameObject canvas;
    public GameObject gameUIBtn;
    public GameObject levelnCoin;
    // ui buttons
    public Button sound_BTN, add_bottle_btn;
    public Sprite mutedImage, non_muted_img;



    // ui prices to skip and add more bottles 
    public Text skipLevelPrice_txt, undoPriceTxt, addBottlePirce_txt, CoinsUIText, coinsTextWinPanel;


    // singilton design pattern
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {

            Destroy(gameObject);

        }
    }


    // Start is called before the first frame update
    void Start()
    {
        updateMain_CoinsVlue();
        // check the sound status
        checkSound_status();
        setupUiButtonsPrices();


        Click_sound = generalGameSettings.click_sound;

        Screen.orientation = ScreenOrientation.Portrait;
        bgCanvasPanel.SetActive(false);
        winPanel.SetActive(false);
        notEnoughGames.SetActive(false);
        addCoinsAfterBottleFilled.SetActive(false);
        winParticle.gameObject.SetActive(false);
        winSkyshot.gameObject.SetActive(false);
        canvas.SetActive(false);
        gameUIBtn.gameObject.SetActive(true);
        levelnCoin.SetActive(true);
        // ads 
        // GameAds.instance.loadInterstitialAd();
        //GameAds.instance.showbannerAD();










    }



    public void givePlayerBottleCoins()
    {
        int coins = PlayerPrefs.GetInt("game_coins_number");
        coins += generalGameSettings.coinsNumberRewarded_bottleFillsUp;

        addCoinsAfterBottleFilled.SetActive(true);


        // show the  animation 
        PlayerPrefs.SetInt("game_coins_number", coins);

        updateMain_CoinsVlue();

    }

    public void undoRewardPlayer()
    {
        int coins = PlayerPrefs.GetInt("game_coins_number");
        coins -= generalGameSettings.coinsNumberRewarded_bottleFillsUp;

        // show the  animation 
        PlayerPrefs.SetInt("game_coins_number", coins);

        updateMain_CoinsVlue();
    }

    void updateMain_CoinsVlue()
    {
        // update the coins ui 
        CoinsUIText.text = PlayerPrefs.GetInt("game_coins_number").ToString();

    }




    void setupUiButtonsPrices()
    {
        skipLevelPrice_txt.text = generalGameSettings.coinsNumber_to_skip.ToString();
        undoPriceTxt.text = generalGameSettings.coinsNumber_to_UndoMoves.ToString();
        //addBottlePirce_txt.text = generalGameSettings.coinsNumber_to_AddnewBottle.ToString();
    }




    private void checkSound_status()
    {

        // check sound status 
        int i = PlayerPrefs.GetInt("sound_status", 1);
        if (i == 0)
        {
            mute_audio();
        }
        else
        {
            inmute_audio();
        }

    }

    public void mute_audio()
    {
        PlayerPrefs.SetInt("sound_status", 0); //no sound (muted) 
       // sound_BTN.GetComponent<Image>().sprite = mutedImage;
        game_AudioSOurce.mute = true;
        AudioListener.pause = true;


    }
    public void inmute_audio()
    {
        PlayerPrefs.SetInt("sound_status", 1);  // sound not muted   
        //sound_BTN.GetComponent<Image>().sprite = non_muted_img;
        game_AudioSOurce.mute = false;
        AudioListener.pause = false;

    }








    #region  ui manager in game


    public void openLevelList()
    {
        // open levle ist
       // SceneManager.LoadScene(1);
        clickOnLoad(1);
    }


    public void openMainMenu()
    {
        // load main scene
        //SceneManager.LoadScene(0);
        clickOnLoad(0);
    }

    // principal menu butotns 
    public void restartBtnCLikced()
    {
        //SceneManager.LoadScene(3);
        clickOnLoad(3);
    }

    public void clickOnLoad(int sceneIndex)
    {
        StartCoroutine(LoadSceneDelay(sceneIndex));
    }

    private IEnumerator LoadSceneDelay(int index)
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(index);
    }

    public void addOneTubeBtnClicked()
    {


        PlayerPrefs.SetInt("reward_stats", 1);
        GameAds.instance.showreward_Ad();    ///showreward_Ad
       // reward_player_addBottle();




        /*int coins = getcurrentCoinsNumber();
        if (coins >= generalGameSettings.coinsNumber_to_AddnewBottle)
        {
            coins -= generalGameSettings.coinsNumber_to_AddnewBottle;
            PlayerPrefs.SetInt("game_coins_number", coins);

            // add a new bottle to the game
            LevleGeneartor.instance.addAnotherBottle();

            updateMain_CoinsVlue();

        }

        else
        {
            notEnoughGames.SetActive(true);
        }*/


        /* GameUnityAds.instance.status_reward = 0;
         GameUnityAds.instance.ShowRewardedVideo();*/
    }


    int v_btl = 0;
    public void reward_player_addBottle()
    {



        if (v_btl >= 2)
        {
            return;
        }

        v_btl++;
        if (v_btl >= 2)
        {
            add_bottle_btn.enabled = false;
            add_bottle_btn.transform.GetChild(0).gameObject.SetActive(true);
        }
        LevleGeneartor.instance.addAnotherBottle();
    }


    public void UndoMoveCLicked()
    {




        int coins = getcurrentCoinsNumber();
        if (coins >= generalGameSettings.coinsNumber_to_UndoMoves)
        {
            if (GameController.instance.doesUndoAvailibal() == true)
            {
                coins -= generalGameSettings.coinsNumber_to_UndoMoves;
                PlayerPrefs.SetInt("game_coins_number", coins);

                // add a new bottle to the game
                updateMain_CoinsVlue();



                GameController.instance.makeUndoMovement();

            }

        }
        else
        {
            notEnoughGames.SetActive(true);
        }


        // do undo moves with the needed coins number
    }

    public void clickOnSkipbutoon()
    {
        int coins = getcurrentCoinsNumber();
        if (coins >= generalGameSettings.coinsNumber_to_skip)
        {
            coins -= generalGameSettings.coinsNumber_to_skip;
            PlayerPrefs.SetInt("game_coins_number", coins);

            // skip the game 
            updateMain_CoinsVlue();

            StartCoroutine(SkipDelay());
            //  LevleGeneartor.instance.openTheWinsLevel();
            //LevleGeneartor.instance.Skip_Level();
            // LevleGeneartor.instance.loadNextLevel();



        }
        else
        {
            notEnoughGames.SetActive(true);
        }
    }

    IEnumerator SkipDelay()
    {
        yield return new WaitForSeconds(0.1f);
        LevleGeneartor.instance.Skip_Level();
    }

    int getcurrentCoinsNumber()
    {
        return PlayerPrefs.GetInt("game_coins_number", 0);
    }

    #endregion



    #region  win panel butotns
    public void win_level()
    {
        int n_win = PlayerPrefs.GetInt("n_win_value", 0);
        n_win++;
        if (n_win >= generalGameSettings.showInterstitialAfter_n_win)
        {
            //show interstitial ad
            //  GameAds.instance.ShowInterstitialAd(); //// ShowInterstitialAd 
            
            AdMobManager.Instance.ShowInterstitialAd();

            //AdMobManager.Instance.LoadInterstitialAd();

            // give the n_win 0



            n_win = 0;
        }
        // update the n_win value on playerprefs
        PlayerPrefs.SetInt("n_win_value", n_win);



        // change the coins number on win panel
        coinsTextWinPanel.text = PlayerPrefs.GetInt("game_coins_number").ToString();


        //winPanel.SetActive(true);
        StartCoroutine(PopupComplete());
        LevleGeneartor.instance.openTheWinsLevel();

        game_AudioSOurce.clip = winSound;
        game_AudioSOurce.Play();

    }

    IEnumerator PopupComplete()
    {
        yield return new WaitForSeconds(0.7f);
        bgCanvasPanel.SetActive(true);
        winPanel.SetActive(true);
        uitween.instance.StarsAnim();
        winParticle.gameObject.SetActive(true);
        winSkyshot.gameObject.SetActive(true);
        winSkyshot.Play();
        canvas.SetActive(true);
        gameUIBtn.gameObject.SetActive(false);
        levelnCoin.SetActive(false);
    }



    public void bottleFullAddCoins()
    {

    }



    public void palyClickSound()
    {
        game_AudioSOurce.clip = Click_sound;
        game_AudioSOurce.Play();
    }
    #endregion

    #region sound manager 

    public void CLickOnSOundBtn()
    {

        int a = PlayerPrefs.GetInt("sound_status", 1);
        if (a == 0)
        {
            inmute_audio();
        }
        else
        {
            mute_audio();
        }




    }


    public void openHomeScene()
    {
        clickOnPlay();
    }

    public void clickOnPlay()
    {
        StartCoroutine(LoadSceneAfterDelay(0.1f));
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(0);
    }



    #endregion


    #region undo event






    #endregion

}
