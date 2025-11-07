using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
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
}
