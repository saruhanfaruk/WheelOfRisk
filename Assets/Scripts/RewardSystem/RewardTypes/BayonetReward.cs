using UnityEngine;

[CreateAssetMenu(fileName = "BayonetReward", menuName = "Rewards/Bayonet")]
public class BayonetReward : BaseReward
{
    public override void ApplyReward()
    {
    }
    private void OnEnable()
    {
        rewardType = RewardType.Bayonet;
    }
}
