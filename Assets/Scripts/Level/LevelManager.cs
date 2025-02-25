using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Singleton & Fields
    public static LevelManager Instance;
    private int currentLevel = 0;
    public int CurrentLevel => currentLevel;
    int maxLevel = 60;
    public int MaxLevel => maxLevel;
    #endregion

    #region Unity Lifecycle Methods
    private async void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        await new WaitForEndOfFrame();
        maxLevel = GameManager.Instance.gameSettings.maxLevel;
    }

    #endregion

    #region Level Management Methods
    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void IncreaseLevel()
    {
        if (currentLevel < maxLevel)
            currentLevel++;
    }

    public bool IsMaxLevelReached() => CurrentLevel == MaxLevel;

    public void ResetCurrentLevel() => currentLevel = 0;
    #endregion
}
