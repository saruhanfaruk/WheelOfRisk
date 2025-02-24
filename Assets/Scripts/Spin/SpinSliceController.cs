using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpinSliceController : MonoBehaviour
{
    #region Fields
    // UI components for reward image and amount text
    public Image rewardImage;
    public TextMeshProUGUI rewardAmountText;

    // RectTransforms and scale values for animations
    private RectTransform defRewardImageRectTransform;
    private RectTransform defrewardAmountRectTransform;
    private Vector3 defRewardImageScale;
    private Vector3 defRewardAmountScale;

    // Animation duration
    public float animationDuration = .35f;

    // Reward details
    private int rewardAmount;
    public RewardType rewardType;
    public int RewardAmount => rewardAmount;

    // Default height for reward image
    public float defRewardImageHeight;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        // Initialize RectTransform and default values
        defRewardImageRectTransform = rewardImage.GetComponent<RectTransform>();
        defrewardAmountRectTransform = rewardAmountText.GetComponent<RectTransform>();
        defRewardImageScale = defRewardImageRectTransform.localScale;
        defRewardAmountScale = defrewardAmountRectTransform.localScale;
        defRewardImageHeight = defRewardImageRectTransform.sizeDelta.y;
    }

    private void Start()
    {
        // Get animation duration from game settings
        animationDuration = GameManager.Instance.gameSettings.spinSliceAnimationStartTime;
    }
    #endregion

    #region Animation Methods
    private void AnimateRewardScale()
    {
        // Start the animation by scaling the reward image and amount from zero
        defRewardImageRectTransform.localScale = Vector3.zero;
        defrewardAmountRectTransform.localScale = Vector3.zero;
        defRewardImageRectTransform.DOScale(defRewardImageScale, animationDuration);
        defrewardAmountRectTransform.DOScale(defRewardAmountScale, animationDuration);
    }
    #endregion

    #region Public Methods
    // Sets the reward variables and starts the animation
    public void SetVariables(Sprite sprite, int amount, BaseReward reward)
    {
        // Set the reward amount and display it
        rewardAmount = amount;
        rewardImage.sprite = sprite;
        rewardType = reward.rewardType;
        rewardAmountText.text = GlobalUtils.ShortenNumber(amount).ToString();

        // Toggle visibility based on the reward amount
        rewardImage.gameObject.SetActive(true);
        rewardAmountText.gameObject.SetActive(rewardAmount != -1);

        // Adjust the size of the reward image if custom height is set
        Vector2 newSize = defRewardImageRectTransform.sizeDelta;
        newSize.y = reward.isCustomHeight ? reward.newCustomHeight : defRewardImageHeight;
        defRewardImageRectTransform.sizeDelta = newSize;

        // Start the animation for scaling the reward
        AnimateRewardScale();
    }
    #endregion
}
