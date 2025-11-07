using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uitween : MonoBehaviour
{
    [SerializeField]
    GameObject star1, star2, star3;
    public static uitween instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
    }

    public void StarsAnim()
    {
        if (star1 == null || star2 == null || star3 == null)
        {
            Debug.LogError("One or more stars not assigned in UITween!");
            return;
        }
        LeanTween.scale(star1, new Vector3(1, 1, 1f), 2f).setDelay(0f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star2, new Vector3(1, 1, 1f), 2f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star3, new Vector3(1, 1, 1f), 2f).setDelay(1f).setEase(LeanTweenType.easeOutElastic);

    }


}