using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource bgmSource;

    public AudioClip bgmClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            bgmSource = GetComponent<AudioSource>();
            if (bgmSource != null && bgmClip != null)
            {
                bgmSource.clip = bgmClip;
                bgmSource.loop = true;
                bgmSource.Play();
            }
           // SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CheckVolumeStatus();
    }
    private void OnDestroy()
    {
       // SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    //private void OnSceneLoaded(Scene scene , LoadSceneMode mode)
    //{
    //    if(scene.name == "Game")
    //    {
    //        StopMusic();
    //    }
    //    else
    //    {
    //        PlayMusic();
    //    }
    //}

    public void CheckVolumeStatus()
    {
        int status = PlayerPrefs.GetInt("volume_status", 1);
        if(status == 0)
        {
            MuteVolume();
        }
        else
        {
            UnMuteVolume();
        }
    }
    //public void SetVolume(float volume)
    //{
    //    if (bgmSource != null)
    //        bgmSource.volume = volume;
    //}

    public void MuteVolume()
    {
        PlayerPrefs.SetInt("volume_status", 0);
        if (bgmSource != null)
            bgmSource.mute = true;
    }

    public void UnMuteVolume()
    {
        PlayerPrefs.SetInt("volume_status", 1);
        if (bgmSource != null)
            bgmSource.mute = false;
    }
    public void StopMusic()
    {
        if (bgmSource != null && bgmSource.isPlaying)
            bgmSource.Stop();
    }
    public void PlayMusic()
    {
        if (bgmSource != null && !bgmSource.isPlaying)
            bgmSource.Play();
    }
}
