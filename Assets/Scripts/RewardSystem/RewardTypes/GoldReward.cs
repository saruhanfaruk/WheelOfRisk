using UnityEngine;

[CreateAssetMenu(fileName = "GoldReward", menuName = "Rewards/Gold")]
public class GoldReward : BaseReward
{
    public override void ApplyReward()
    {
    }
    private void OnEnable()
    {
        rewardType = RewardType.Gold;
    }
}
