using System.Collections.Generic;

public static class SpinRewardValidator
{
    #region Validation Logic
    // Validates each SpinReward entry, ensuring correct order and proper reward item count.
    public static void ValidateRewards(List<SpinRewardEntry> spinRewardEntries)
    {
        foreach (var entry in spinRewardEntries)
        {
            int order = 0;
            foreach (SpinReward spinReward in entry.spinReward)
            {
                // Ensures each SpinReward has the correct order and SpinType.
                spinReward.order = order;
                spinReward.spinType = entry.spinType;

                // If the reward items array has a size other than 8, resize it.
                if (spinReward.rewardItems.Length != 8)
                    System.Array.Resize(ref spinReward.rewardItems, 8);

                order++;
            }
        }
    }
    #endregion
}