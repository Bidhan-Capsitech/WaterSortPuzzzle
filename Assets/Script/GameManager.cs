using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isBottleOpen;
    public bool isBGOpen;
    public static GameManager instance;

    public Sprite normal;
    public Sprite ipad;
    public Sprite Tall;
    public Image targetImage;
    private void Awake()
    {
        if (Camera.main.aspect < 0.55f)
        {
            // 9:21
            targetImage.sprite = ipad;

        }
        else if (Camera.main.aspect < 0.65f)
        {
            // 9:16
            targetImage.sprite = normal;
        }
        else
        {
            // 3:4
            targetImage.sprite = Tall;

        }
    }
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        AdMobManager.Instance.LoadBannerAd();
        AdMobManager.Instance.LoadInterstitialAd();
        //AdMobManager.Instance.LoadRewardedAd();
    }

    public void BottleShopBtn()
    {
        isBottleOpen = true;
        clickOnPlay();
    }
    //public void BackgroundShopBtn()
    //{
    //    isBGOpen = true;
    //    clickOnPlay();
    //}

    public void clickOnPlay()
    {
        StartCoroutine(LoadSceneAfterDelay(0.1f));
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(2);
    }
    public void OpenShopPanel()
    {
        isBGOpen = true;
        clickOnPlay();
    }
}
