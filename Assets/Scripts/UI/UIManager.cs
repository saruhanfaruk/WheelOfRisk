using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UIManager : SerializedMonoBehaviour
{
    #region Singleton
    public static UIManager Instance;
    #endregion

    #region UI Elements
    private Dictionary<SpinType, SpinNameDetail> spinNameDetails = new Dictionary<SpinType, SpinNameDetail>();
    public TextMeshProUGUI spinNameText;
    public SpriteAtlas rewardAtlas; // Sprite Atlas for rewards
    public Transform wheelParent; // Parent object for the spinning wheel
    public GameObject wheelSlicePrefab; // Prefab for each slice of the wheel
    public Button spinButton;
    public Button exitButton;
    public TextMeshProUGUI goldText;
    private Color defGoldTextColor;
    private Sequence goldColorSequence;

    // Death panel UI elements
    public GameObject deathPanel;
    public Button deathPanelGiveUpButton;
    public Button deathPanelReviveButton;
    public TextMeshProUGUI deathPanelReviveCostText;
    #endregion

    #region Private Fields
    private List<SpinSliceController> slices = new List<SpinSliceController>();
    private const int SliceCount = 8; // The wheel consists of 8 slices
    private bool isSpinSlicesCreated;
    public bool IsSpinSlicesCreated => isSpinSlicesCreated;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        isSpinSlicesCreated = false;
        Instance = this;

        // Add button listeners
        spinButton.onClick.AddListener(SpinClick);
        exitButton.onClick.AddListener(ExitClick);
        deathPanelGiveUpButton.onClick.AddListener(GiveUpButtonClick);
        deathPanelReviveButton.onClick.AddListener(ReviveButtonClick);

        // Store the default gold text color
        defGoldTextColor = goldText.color;
    }

    private void Start()
    {
        // Initialize UI elements
        spinNameDetails = GameSettings.Instance.spinNameDetails;
        SetGoldText(PlayerPrefs.GetInt(GlobalUtils.goldPrefKey));
        GenerateSpinSlices();
        SetReviveCostText();
    }
    #endregion

    #region Button Click
    private void SpinClick()
    {
        // Disable spin and exit buttons and start the spin
        StatusSpinAndExitButton(false);
        SpinManager.Instance.SpinWheel();
    }

    private void ExitClick()
    {
        // Trigger game exit logic
        GameManager.Instance.WinToGame();
    }

    private void GiveUpButtonClick()
    {
        // Restart the game and hide the death panel
        GameManager.Instance.RestartGame();
        StatusDeathPanel(false);
    }

    private void ReviveButtonClick()
    {
        // Handle the revive logic if the player has enough gold
        int currentGold = PlayerPrefs.GetInt(GlobalUtils.goldPrefKey);
        int reviveCost = GameManager.Instance.GetReviveCost();

        if (currentGold > reviveCost)
        {
            currentGold -= reviveCost;
            PlayerPrefs.SetInt(GlobalUtils.goldPrefKey, currentGold);
            SetGoldText(currentGold);
            StatusDeathPanel(false);
            GameManager.Instance.IncreaseReviveCostIndex();
            SetReviveCostText();
            GameManager.Instance.LevelProgressionController();
            StatusSpinAndExitButton(true);
        }
        else
        {
            ShowInsufficientFundsFeedback();
        }
    }
    #endregion

    #region Text Setting
    public void UpdateSpinNameText(SpinType spinType)
    {
        // Update the spin name and color based on the selected spin type
        spinNameText.text = spinNameDetails[spinType].spinName;
        spinNameText.color = spinNameDetails[spinType].spinTextColor;
    }

    public void SetReviveCostText()
    {
        // Update the revive cost text
        deathPanelReviveCostText.text = GlobalUtils.ShortenNumber(GameManager.Instance.GetReviveCost());
    }

    public void SetGoldText(int amount)
    {
        // Update the gold amount display
        goldText.text = GlobalUtils.ShortenNumber(amount).ToString();
    }
    #endregion

    #region Spin Wheel Generation
    private void GenerateSpinSlices()
    {
        // Generate the slices of the spin wheel
        float angleStep = 360f / SliceCount;
        for (int i = 0; i < SliceCount; i++)
        {
            GameObject slice = Instantiate(wheelSlicePrefab, wheelParent);
            slice.transform.localRotation = Quaternion.Euler(0, 0, -i * angleStep);
            SpinSliceController spinSliceController = slice.GetComponent<SpinSliceController>();
            slices.Add(spinSliceController);
        }
        isSpinSlicesCreated = true;
    }

    public void AssignRewardsToSlices()
    {
        // Assign rewards to each slice of the spin wheel
        BaseReward[] rewardItems = SpinManager.Instance.GetRewards();
        int i = 0;
        foreach (SpinSliceController slice in slices)
        {
            SetRewardToSlice(slice, rewardItems[i]);
            i++;
        }
    }

    private void SetRewardToSlice(SpinSliceController slice, BaseReward reward)
    {
        // Set the reward for the given slice
        Sprite rewardSprite = rewardAtlas.GetSprite(reward.rewardAtlasName);
        int amount;

        if (reward.rewardType == RewardType.Bomb)
        {
            amount = -1;
        }
        else
        {
            amount = reward.isFixedAmount ? reward.amount : reward.amounts[Random.Range(0, reward.amounts.Count)];
            amount *= (int)(reward.levelMultiplier * (LevelManager.Instance.GetCurrentLevel() + 1));
        }

        slice.SetVariables(rewardSprite, amount, reward);
    }
    #endregion

    #region Reward Processing
    public void ProcessEarnedReward(int targetRewardIndex, string rewardSpriteName)
    {
        // Handle the logic when a player earns a reward
        Image rewardImagePrefab = slices[targetRewardIndex].rewardImage;
        Vector3 startingPos = rewardImagePrefab.transform.position;

        EarnedRewardPoolManager.Instance.HandleEarnedReward(
            rewardSpriteName,
            startingPos,
            rewardImagePrefab,
            slices[targetRewardIndex].RewardAmount,
            slices[targetRewardIndex].RewardType
        );
    }
    #endregion

    #region UI Status Updates
    public void StatusSpinAndExitButton(bool status)
    {
        // Enable or disable spin and exit buttons
        exitButton.gameObject.SetActive(status);
        spinButton.gameObject.SetActive(status);
    }

    public void StatusDeathPanel(bool value)
    {
        // Show or hide the death panel
        deathPanel.gameObject.SetActive(value);
    }

    public void ShowInsufficientFundsFeedback()
    {
        // Flash the gold text in red if the player has insufficient funds
        float time = 0.15f;
        if (goldColorSequence != null)
        {
            goldColorSequence.Kill();
            goldText.color = defGoldTextColor;
        }

        goldColorSequence = DOTween.Sequence();
        goldColorSequence.Append(goldText.DOColor(Color.red, time))
                         .Append(goldText.DOColor(defGoldTextColor, time))
                         .SetLoops(2);
    }
    #endregion
}
