using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EarnedRewardPoolManager : MonoBehaviour
{
    #region Singleton
    public static EarnedRewardPoolManager Instance;
    #endregion

    #region Fields
    public Transform poolParent;
    public EarnedRewardPoolSliceController sliceControllerPrefab;
    private Dictionary<string, EarnedRewardPoolSliceController> slicesController = new Dictionary<string, EarnedRewardPoolSliceController>();
    private EarnedRewardPoolSliceController currentSliceController;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    #endregion

    #region Reward Handling Methods
    /// <summary>
    /// Handles the logic for the earned rewards by checking if a slice controller exists or creating a new one.
    /// </summary>
    public void HandleEarnedReward(string rewardAtlasName, Vector3 startingPos, Image rewardImagePrefab, int amount, RewardType rewardType)
    {
        GetOrCreateRewardSliceController(rewardAtlasName, rewardType);
        AnimateRewards(startingPos, rewardImagePrefab, amount);
    }

    /// <summary>
    /// Checks for the existing slice controller for a given reward atlas name or creates a new one.
    /// </summary>
    private void GetOrCreateRewardSliceController(string rewardAtlasName, RewardType rewardType)
    {
        if (slicesController.ContainsKey(rewardAtlasName))
            currentSliceController = slicesController[rewardAtlasName];
        else
        {
            currentSliceController = Instantiate(sliceControllerPrefab, poolParent);
            slicesController.Add(rewardAtlasName, currentSliceController);
            currentSliceController.SetVariables(UIManager.Instance.rewardAtlas.GetSprite(rewardAtlasName), rewardType);
        }
    }
    #endregion

    #region Animation Methods
    /// <summary>
    /// Animates the reward images and moves them to their final positions.
    /// </summary>
    public void AnimateRewards(Vector3 startingPos, Image rewardImagePrefab, int amount)
    {
        GameSettings gameSettings = GameSettings.Instance;
        float totalDuration = gameSettings.rewardPoolingDuration;
        bool isCompleted = false;
        float radius = 80;
        for (int i = 0; i < gameSettings.rewardCloneCount; i++)
        {
            Image newRewardImage = Instantiate(rewardImagePrefab, startingPos, Quaternion.identity, UIManager.Instance.transform);
            newRewardImage.transform.position = startingPos;
            Vector3 randomPos = startingPos + new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), 0);
            newRewardImage.transform.DOMove(randomPos, totalDuration / 2f).OnComplete(() =>
            {
                Vector3 finishPos = currentSliceController.rewardImage.transform.position;
                newRewardImage.transform.DOMove(finishPos, totalDuration / 2f).OnComplete(() =>
                {
                    if (!isCompleted)
                    {
                        isCompleted = true;
                        currentSliceController.IncreaseAmount(amount);
                        CompleteAnimation();
                    }
                    Destroy(newRewardImage.gameObject);
                });
            });
        }
    }
    #endregion

    #region UI and Game State Methods
    /// <summary>
    /// Completes the animation and triggers the next level.
    /// </summary>
    public void CompleteAnimation()
    {
        UIManager.Instance.StatusSpinAndExitButton(true);
        LevelManager.Instance.IncreaseLevel();
        GameManager.Instance.ProceedToNextLevel();
    }

    /// <summary>
    /// Calculates the total amount of gold earned from the pool.
    /// </summary>
    public int CalculateAmountofGoldEarned()
    {
        foreach (var slice in slicesController)
        {
            if (slice.Value.rewardType == RewardType.Gold)
                return slice.Value.RewardAmount;
        }
        return 0;
    }

    /// <summary>
    /// Clears all the earned rewards and resets the pool.
    /// </summary>
    public void ClearEarnedReward()
    {
        foreach (var slice in slicesController)
        {
            Destroy(slice.Value.gameObject);
        }
        slicesController.Clear();
    }
    #endregion
}
