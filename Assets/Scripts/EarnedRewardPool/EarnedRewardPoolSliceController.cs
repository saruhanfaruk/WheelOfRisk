using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EarnedRewardPoolSliceController : MonoBehaviour
{
    #region Fields
    public RewardType rewardType;
    public Image rewardImage;
    public TextMeshProUGUI rewardAmountText;
    private int rewardAmount = 0;
    public int RewardAmount => rewardAmount;
    #endregion

    #region Public Methods
    /// <summary>
    /// Sets the sprite and reward type for this reward slice.
    /// </summary>
    public void SetVariables(Sprite sprite, RewardType rewardType)
    {
        this.rewardType = rewardType;
        rewardImage.sprite = sprite;
        rewardAmountText.text = "0";
    }

    /// <summary>
    /// Increases the reward amount and updates the UI text.
    /// </summary>
    public void IncreaseAmount(int amount)
    {
        rewardAmount += amount;
        rewardAmountText.text = GlobalUtils.ShortenNumber(rewardAmount).ToString();
    }
    #endregion
}
