using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class LevelItemController : SerializedMonoBehaviour
{
    #region Fields
    private ZoneType activeZoneType;
    public ZoneType ActiveZoneType { get => activeZoneType; set { activeZoneType = value; } }
    Dictionary<ZoneType, ZoneStyle> zoneStyles;
    public TextMeshProUGUI levelText;
    public Image levelBackgroundImage;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        zoneStyles = GameSettings.Instance.zoneStyles;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Activates the level UI, making it visible and applying the selected styles.
    /// </summary>
    public void ActiveLevel()
    {
        levelBackgroundImage.enabled = true;
        levelText.color = zoneStyles[activeZoneType].selectedLevelColor;
        levelBackgroundImage.sprite = zoneStyles[activeZoneType].selectedLevelBackground;
    }

    /// <summary>
    /// Deactivates the level UI, making it hidden and applying the unselected styles.
    /// </summary>
    public void UnActiveLevel()
    {
        levelText.color = zoneStyles[activeZoneType].unselectedLevelColor;
        levelBackgroundImage.enabled = false;
    }

    /// <summary>
    /// Sets the level text to display the current level.
    /// </summary>
    public void SetLevelText(int level)
    {
        levelText.text = level.ToString();
    }

    /// <summary>
    /// Hides the level UI completely, both background image and level text.
    /// </summary>
    public void HideLevel()
    {
        levelBackgroundImage.enabled = false;
        levelText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows the level text, leaving the background image disabled.
    /// </summary>
    public void ShowLevel()
    {
        levelText.gameObject.SetActive(true);
    }
    #endregion
}
