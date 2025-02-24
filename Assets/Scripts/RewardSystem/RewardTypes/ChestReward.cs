using UnityEngine;

[CreateAssetMenu(fileName = "ChestReward", menuName = "Rewards/Chest")]
public class ChestReward : BaseReward
{
    public override void ApplyReward()
    {
    }
    private void OnEnable()
    {
        rewardType = RewardType.Chest;
    }
}
