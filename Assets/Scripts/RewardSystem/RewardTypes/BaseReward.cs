using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public abstract class BaseReward : ScriptableObject
{
    #region Fields
    [ReadOnly]
    public RewardType rewardType; // The type of the reward (e.g., Gold, Bomb, etc.)
    public string rewardAtlasName; // Name of the reward image in the atlas
    public float weight = 1; // Weight for determining how often this reward should be chosen
    [ShowIf("@rewardType != RewardType.Bomb")]
    public bool isFixedAmount = true; // Indicates whether the reward amount is fixed
    [ShowIf("@isFixedAmount && rewardType != RewardType.Bomb")]
    public int amount = 1; // The fixed amount of the reward, if isFixedAmount is true
    [HideIf("isFixedAmount")]
    public List<int> amounts = new List<int>(); // List of possible reward amounts if not fixed
    [MinValue(1), ShowIf("@rewardType != RewardType.Bomb")]
    public float levelMultiplier = 1; // Multiplier based on level
    public bool isCustomHeight; // Whether the reward has a custom height
    [ShowIf(nameof(isCustomHeight))]
    public float newCustomHeight; // Custom height if isCustomHeight is true
    #endregion

    #region Abstract Method
    /// <summary>
    /// Applies the reward effect to the player or game system.
    /// </summary>
    public abstract void ApplyReward();
    #endregion
}
