using UnityEngine;

[CreateAssetMenu(fileName = "WeaponReward", menuName = "Rewards/Weapon")]
public class WeaponReward : BaseReward
{
    public override void ApplyReward()
    {
    }
    private void OnEnable()
    {
        rewardType = RewardType.Weapon;
    }
}
