using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class SpinManager : SerializedMonoBehaviour
{
    #region Fields
    public Dictionary<SpinType, SpinSpriteAtlasData> spinSpriteAtlasData = new Dictionary<SpinType, SpinSpriteAtlasData>();
    public static SpinManager Instance;
    public SpinController spinController;
    public SpinRewardConfig rewardConfig;
    private int targetRewardIndex;
    private BaseReward[] rewardItems = new BaseReward[8];

    private SpinType currentSpinType;
    public SpinType CurrentSpinType => currentSpinType;
    private int silverSpinLevelInterval = 5;
    private int goldSpinLevelInterval = 30;
    public int GoldSpinLevelInterval => goldSpinLevelInterval;
    public int SilverSpinLevelInterval => silverSpinLevelInterval;
    #endregion

    #region Initialization
    private void Awake()
    {
        // Singleton pattern to ensure only one instance of SpinManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        GameSettings gameSettings = GameSettings.Instance;
        silverSpinLevelInterval = gameSettings.silverSpinLevelInterval;
        goldSpinLevelInterval = gameSettings.goldSpinLevelInterval;
    }
    #endregion

    #region Reward Management
    /// <summary>
    /// Gets the array of reward items.
    /// </summary>
    public BaseReward[] GetRewards()
    {
        return rewardItems;
    }

    /// <summary>
    /// Sets new rewards based on the active spin type.
    /// </summary>
    public void SetNewReward()
    {
        foreach (var item in rewardConfig.spinRewardEntry)
        {
            if (item.spinType == currentSpinType)
            {
                rewardItems = item.spinReward[Random.Range(0, item.spinReward.Count)].rewardItems
                    .OrderBy(x => Random.value) // Shuffle rewards randomly
                    .ToArray();
            }
        }
    }
    #endregion

    #region Spin Operations
    public void SetSpinType()
    {
        int currentLevel = LevelManager.Instance.CurrentLevel;
        if ((currentLevel + 1) % goldSpinLevelInterval == 0)
            currentSpinType = SpinType.Gold;
        else if ((currentLevel + 1) % silverSpinLevelInterval == 0)
            currentSpinType = SpinType.Silver;
        else
            currentSpinType = SpinType.Bronze;
    }
    /// <summary>
    /// Starts the wheel spin by selecting a random reward.
    /// </summary>
    public void SpinWheel()
    {
        targetRewardIndex = GetWeightedRandomIndex(); // Get the index of the selected reward
        spinController.SpinWheel(targetRewardIndex); // Spin the wheel based on the selected reward
    }

    /// <summary>
    /// Completes the spin and applies the reward to the player.
    /// </summary>
    public void CompletedSpin()
    {
        rewardItems[targetRewardIndex].ApplyReward(); // Apply the selected reward
        if (rewardItems[targetRewardIndex].rewardType != RewardType.Bomb)
        {
            UIManager.Instance.ProcessEarnedReward(targetRewardIndex, rewardItems[targetRewardIndex].rewardAtlasName); // Process earned reward
        }
    }

    /// <summary>
    /// Returns a weighted random index based on reward item weights.
    /// </summary>
    private int GetWeightedRandomIndex()
    {
        List<float> weights = new List<float>();
        foreach (var item in rewardItems)
            weights.Add(item.weight); // Add the weights of the reward items

        float totalWeight = weights.Sum(); // Calculate the total weight
        float randomValue = Random.Range(0, totalWeight); // Generate a random number based on total weight
        float sum = 0;

        for (int i = 0; i < weights.Count; i++)
        {
            sum += weights[i];
            if (randomValue <= sum)
                return i; // Return the index where the random value is within the weighted range
        }
        return weights.Count - 1; // If all else fails, return the last index
    }
    #endregion
}
