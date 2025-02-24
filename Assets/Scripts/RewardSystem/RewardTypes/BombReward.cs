using UnityEngine;

[CreateAssetMenu(fileName = "BombReward", menuName = "Rewards/Bomb")]
public class BombReward : BaseReward
{
    public override void ApplyReward()
    {
        UIManager.Instance.StatusDeathPanel(true);
    }
    private void OnEnable()
    {
        rewardType = RewardType.Bomb;
    }
}
