using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeReward", menuName = "Rewards/Grenade")]
public class GrenadeReward : BaseReward
{
    public override void ApplyReward()
    {
    }
    private void OnEnable()
    {
        rewardType = RewardType.Grenade;
    }
}
