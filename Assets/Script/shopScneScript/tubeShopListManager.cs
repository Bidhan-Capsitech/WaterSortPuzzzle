using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tubeShopListManager : MonoBehaviour
{
    public static tubeShopListManager instance;
    public Button tubeButtonPrefab;

    public GameObject shop_panel, notEnoughCoins_Panel;

    // parents objects
    public Transform tubesParent;

    private Button[] bottles_list_Buttons;

    List<Button> TubesButtonObjects;

    public GeneralGameSettings generalGameSettings;

    [SerializeField] private GameObject bottleShopePanel;
    [SerializeField] private GameObject bgShopePanel;


    // sprites status for the image
    public Sprite lockedShopItem_sprite, opensShopItem_sprite, selectedShopItem_sprite;

    public Button blueBgBtn, orangeBgBtn, blueTubeBtn, orangeTubeBtn;

    private void Awake()
    {
        if(instance == null)
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
        // set the old selected sprite for the background

        HandleShopOption();
        generateBackButtons();
        notEnoughCoins_Panel.SetActive(false);
    }


    // back butotns
    void generateBackButtons()
    {
        TubesButtonObjects = new List<Button>();

        TubeBack[] TubeShopList = generalGameSettings.shopTubes;
        for (int i = 0; i < TubeShopList.Length; i++)
        {
            int copy = i + 1;
            // instantiate buttons 
            Button currentButton = Instantiate(tubeButtonPrefab, tubeButtonPrefab.transform.position, Quaternion.identity);
            currentButton.transform.SetParent(tubesParent.transform, false);

            // give those new buttons listener ... when the buton clicked 
            currentButton.onClick.AddListener(() => selectBack(copy));
            TubesButtonObjects.Add(currentButton);

        }


        for (int i = 0; i < TubeShopList.Length; i++)
        {

            // give the shop item a priview image of the background
            TubesButtonObjects[i].transform.GetChild(0).GetComponentInChildren<Image>().sprite = TubeShopList[i].prefabSprite; // button sprite
                                                                                                                               // Debug.Log("the number is" + i);



            // is the background locked or unlocked
            if (TubeShopList[i].isOpened == false)      // that's means it's locked
            {
                // its locked assign the locked panel
                TubesButtonObjects[i].transform.GetComponentInChildren<Image>().sprite = lockedShopItem_sprite;


                // active the locked panel
                TubesButtonObjects[i].transform.GetChild(2).GetComponentInChildren<Image>().gameObject.SetActive(true);

                // because it's locked update the price to open text
                TubesButtonObjects[i].transform.GetChild(1).GetComponentInChildren<Text>().text = TubeShopList[i].coinsNumber_To_Unlock.ToString();  // button level to open

            }
            else
            {

                // its locked assign the locked panel
                TubesButtonObjects[i].transform.GetComponentInChildren<Image>().sprite = opensShopItem_sprite;


                TubesButtonObjects[i].transform.GetChild(2).gameObject.SetActive(false);

                TubesButtonObjects[i].transform.GetChild(1).GetComponentInChildren<Text>().text = "";  // button level to open

            }

            int selected_item = PlayerPrefs.GetInt("Selected_Tube", 0);
            if (i == selected_item)
            {
                TubesButtonObjects[i].transform.GetComponentInChildren<Image>().sprite = selectedShopItem_sprite;
            }
        }

    }


    void selectBack(int index)
    {

        TubeBack selectedtube = generalGameSettings.shopTubes[index - 1];

        if (selectedtube.isOpened == false)
        {
            // then try to open it 

            int playerGameCoins = PlayerPrefs.GetInt("game_coins_number", 0);
            if (playerGameCoins >= selectedtube.coinsNumber_To_Unlock)
            {

                playerGameCoins -= selectedtube.coinsNumber_To_Unlock;
                selectedtube.isOpened = true;
                PlayerPrefs.SetInt("game_coins_number", playerGameCoins);

                ShopSystem_Manager.instance.UpdateCoinsSHopPanel();
                if(MainLevelListManager.instance != null)
                {
                    MainLevelListManager.instance.UpdateCoinsLevelPanel();
                }


                // unlock the ui   
                // its locked assign the locked panel
                TubesButtonObjects[index - 1].transform.GetComponentInChildren<Image>().sprite = opensShopItem_sprite;   

                TubesButtonObjects[index - 1].transform.GetChild(2).gameObject.SetActive(false);

                TubesButtonObjects[index - 1].transform.GetChild(1).GetComponentInChildren<Text>().text = "";  // button level to open


            }
            else
            {
                Debug.Log("not enough coins you have, please recharge your coins");
                notEnoughCoins_Panel.SetActive(true);
            }

        }
        else
        {

            // the item is opened so select that item
            int oldselected = PlayerPrefs.GetInt("Selected_Tube", 0);
            TubesButtonObjects[oldselected].transform.GetComponentInChildren<Image>().sprite = opensShopItem_sprite;


            // give the new selected item data needed
            PlayerPrefs.SetInt("Selected_Tube", index - 1);

            TubesButtonObjects[index - 1].transform.GetComponentInChildren<Image>().sprite = selectedShopItem_sprite;


        }


    }

    public void HandleShopOption()
    {
        //if (GameManager.instance.isBottleOpen)
        //{
        //    GameManager.instance.isBottleOpen = false;
        //    bottleShopePanel.SetActive(true);
        //    bgShopePanel.SetActive(false);
        //}
        if (GameManager.instance.isBGOpen)
        {
            GameManager.instance.isBGOpen = false;
            bottleShopePanel.SetActive(false);
            bgShopePanel.SetActive(true);
            orangeBgBtn.gameObject.SetActive(true);
            blueTubeBtn.gameObject.SetActive(true);
        }
    }

    public void OpenTubePanel()
    {
        blueTubeBtn.gameObject.SetActive(false);
        orangeTubeBtn.gameObject.SetActive(true);
        blueBgBtn.gameObject.SetActive(true);
        orangeBgBtn.gameObject.SetActive(false);
        bottleShopePanel.SetActive(true);
        bgShopePanel.SetActive(false);
    }
    public void OpenBgPanel()
    {
        blueBgBtn.gameObject.SetActive(false);
        orangeBgBtn.gameObject.SetActive(true);
        blueTubeBtn.gameObject.SetActive(true);
        orangeTubeBtn.gameObject.SetActive(false);
        bgShopePanel.SetActive(true);
        bottleShopePanel.SetActive(false);
    }
}
