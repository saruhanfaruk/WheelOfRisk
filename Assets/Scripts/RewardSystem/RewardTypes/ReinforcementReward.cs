using UnityEngine;

[CreateAssetMenu(fileName = "ReinforcementReward", menuName = "Rewards/Reinforcement")]

public class ReinforcementReward : BaseReward
{
    public override void ApplyReward()
    {
    }
    private void OnEnable()
    {
        rewardType = RewardType.Reinforcement;
    }
}
