using UnityEngine;
using UnityEngine.UI;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager instance;

    public GeneralGameSettings general_gamesettings;

    private AudioClip click_Sound;
    private AudioSource main_Ui_source;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
            main_Ui_source = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        click_Sound = general_gamesettings.click_sound;
        checkSoundStatus();
    }

    public void playClikAudio()
    {
        main_Ui_source.clip = click_Sound;
        main_Ui_source.Play();
    }

    public void mute_audio()
    {
        PlayerPrefs.SetInt("sound_status", 0);

        main_Ui_source.mute = true;

    }

    public void inmute_audio()
    {
        PlayerPrefs.SetInt("sound_status", 1); 
        main_Ui_source.mute = false;
    }

    public void checkSoundStatus()
    {
        if (PlayerPrefs.GetInt("sound_status", 1) == 0)
        {
            mute_audio();
        }
        else if (PlayerPrefs.GetInt("sound_status", 0) == 1)
        {
            inmute_audio();
        }

    }
}
