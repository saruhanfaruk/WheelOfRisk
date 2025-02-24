using UnityEngine;

[CreateAssetMenu(fileName = "ItemReward", menuName = "Rewards/Item")]
public class ItemReward : BaseReward
{
    public override void ApplyReward()
    {
    }
    private void OnEnable()
    {
        rewardType = RewardType.Item;
    }
}
