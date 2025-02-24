using UnityEngine;

[CreateAssetMenu(fileName = "CashReward", menuName = "Rewards/Cash")]
public class CashReward : BaseReward
{
    public override void ApplyReward()
    {
    }
    private void OnEnable()
    {
        rewardType = RewardType.Cash;
    }
}
