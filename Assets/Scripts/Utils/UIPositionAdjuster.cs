using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIPositionAdjuster : MonoBehaviour
{
    #region Variables
    private RectTransform uiElement;

    private float referenceAspect = 4f / 3f; // Reference aspect ratio (X = minX)
    private float maxAspect = 20f / 9f; // Max aspect ratio (X = maxX)
    public float minX = 0f;
    public float maxX = -400f;

    private int lastScreenWidth;
    private int lastScreenHeight;
    #endregion

    #region Unity Methods
    void Start()
    {
        uiElement = GetComponent<RectTransform>();
        AdjustPosition();
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        StartCoroutine(WatchScreenResolutionChange());
    }
    #endregion

    #region Coroutine
    private IEnumerator WatchScreenResolutionChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);

            if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
            {
                AdjustPosition();
                lastScreenWidth = Screen.width;
                lastScreenHeight = Screen.height;
            }
        }
    }
    #endregion

    #region Position Adjustment
    [Button]
    void AdjustPosition()
    {
        float currentAspect = (float)Screen.width / Screen.height;

        if (currentAspect <= referenceAspect)
        {
            SetXPosition(minX);
            return;
        }

        if (currentAspect >= maxAspect)
        {
            SetXPosition(maxX);
            return;
        }

        float t = (currentAspect - referenceAspect) / (maxAspect - referenceAspect);
        float offsetX = Mathf.Lerp(minX, maxX, t);

        SetXPosition(offsetX);
    }

    void SetXPosition(float x)
    {
        if (uiElement != null)
        {
            Vector2 newPosition = uiElement.anchoredPosition;
            newPosition.x = x;
            uiElement.anchoredPosition = newPosition;
        }
    }
    #endregion
}
