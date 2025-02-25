using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
public class GameSettings : SerializedScriptableObject
{
    [TabGroup("EarnedReward")]
    public float rewardPoolingDuration = .7f;
    [TabGroup("EarnedReward")]
    public int rewardCloneCount = 7;

    [TabGroup("Spin")]
    public float spinDuration = 2f;
    [TabGroup("Spin")]
    public int spinRotationCount = 3;
    [TabGroup("Spin")]
    public AnimationCurve spinCurve;
    [TabGroup("Spin")]
    public float spinSliceAnimationStartTime = .35f;
    [TabGroup("Spin")]
    public Dictionary<SpinType, SpinNameDetail> spinNameDetails = new Dictionary<SpinType, SpinNameDetail>();



    [TabGroup("GameManager")]
    public List<int> reviveCosts = new List<int>();
    [TabGroup("GameManager")]
    public int maxLevel = 60;
    [TabGroup("GameManager")]
    public int silverSpinLevelInterval = 5;
    [TabGroup("GameManager")]
    public int goldSpinLevelInterval = 30;

    [TabGroup("LevelScroll")]
    public float levelScrollDuration = 1f;
    [TabGroup("LevelScroll")]
    public Dictionary<ZoneType, ZoneStyle> zoneStyles = new Dictionary<ZoneType, ZoneStyle>();

    private static GameSettings _instance;

    public static GameSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<GameSettings>("GameSettings");
                if (_instance == null)
                {
                    Debug.LogError("GameSettings asset is missing! Please create one in Resources folder.");
                }
            }
            return _instance;
        }
    }


}
