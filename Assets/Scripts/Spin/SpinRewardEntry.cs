using System.Collections.Generic;

[System.Serializable]
public class SpinRewardEntry
{
    #region Fields
    public SpinType spinType;
    public List<SpinReward> spinReward = new List<SpinReward>();
    #endregion
}
