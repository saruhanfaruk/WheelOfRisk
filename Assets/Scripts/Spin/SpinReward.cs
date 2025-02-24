using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SpinReward
{
    #region Fields
    [HideInInspector]
    public int order;
    [HideInInspector]
    public SpinType spinType;
    public BaseReward[] rewardItems = new BaseReward[8];
    #endregion

    #region Editor Methods

#if UNITY_EDITOR
    /// <summary>
    /// Copies the reward list from the previous entry in the spinRewardEntry list.
    /// </summary>
    [Button, ShowIf("@order != 0")]
    public void CopyPreviousRewardList()
    {
        if (SpinRewardConfig.Instance == null) return;

        // Find the correct SpinRewardEntry for the current spinType
        SpinRewardEntry spinRewardEntry = SpinRewardConfig.Instance.spinRewardEntry
            .FirstOrDefault(entry => entry.spinType == spinType);

        if (spinRewardEntry != null)
        {
            if (order > 0 && order - 1 < spinRewardEntry.spinReward.Count)
            {
                // Copy the reward items from the previous SpinReward entry
                for (int i = 0; i < rewardItems.Length; i++)
                    rewardItems[i] = spinRewardEntry.spinReward[order - 1].rewardItems[i];
            }
            else
            {
                Debug.LogError($"SpinRewardConfig: order - 1 out of range for spinType {spinType}");
            }
        }
        else
        {
            Debug.LogError($"SpinRewardConfig: spinType {spinType} not found in spinRewardEntry.");
        }
    }

    /// <summary>
    /// Assigns random rewards to the rewardItems array.
    /// </summary>
    [Button]
    public void AssignRandomRewards()
    {
        // Load the bomb reward and available resources
        BaseReward bombReward = Resources.Load<BaseReward>("Rewards/ScriptableObject_Reward_Bomb");
        List<BaseReward> availableResources = Resources.LoadAll<BaseReward>("Rewards").ToList();
        availableResources.Remove(bombReward); // Remove bomb reward from available resources

        // Assign random rewards to the rewardItems array
        for (int i = 0; i < rewardItems.Length; i++)
        {
            if (i == 0 && spinType == SpinType.Bronze)
                rewardItems[i] = bombReward; // First reward is always the bomb for Bronze type
            else
                rewardItems[i] = availableResources[Random.Range(0, availableResources.Count)];
        }
    }
#endif

    #endregion
}
