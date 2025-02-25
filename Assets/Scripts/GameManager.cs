using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Fields
    public GameSettings gameSettings;
    private int lastReviveIndex;
    public static GameManager Instance;
    public LevelScrollController scrollController;
    bool isFirstGame;
    #endregion
    #region Unity Methods
    private void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        isFirstGame = true;

    }

    private void Start()
    {
        scrollController.GenerateLevelPrefabs();
        LevelProgressionController();
    }
    #endregion

    #region Public Methods
    // Manages the progression of the level, including setting the spin and rewards
    public async void LevelProgressionController()
    {
        // Wait until the level items are created before proceeding
        await new WaitUntil(() => scrollController.IsLevelItemCreated);
        SpinManager.Instance.SetSpinType();
        SpinType currentSpinType = SpinManager.Instance.CurrentSpinType;
        UIManager.Instance.UpdateSpinNameText(currentSpinType);
        SpinManager.Instance.SetNewReward();

        // Wait until the spin slices are created before proceeding
        await new WaitUntil(() => UIManager.Instance.IsSpinSlicesCreated);

        // Scroll to the correct level
        scrollController.ScrollToLevel(LevelManager.Instance.CurrentLevel, isFirstGame);

        // Set the first game flag to false after the first game
        if (isFirstGame)
            isFirstGame = false;

        // Assign rewards to the spin slices
        UIManager.Instance.AssignRewardsToSlices();

        // Update the spin image based on the current spin type
        SpinManager.Instance.spinController.ChangeSpinImage(currentSpinType);
    }

    // Proceeds to the next level or ends the game if max level is reached
    public void ProceedToNextLevel()
    {
        if (LevelManager.Instance.IsMaxLevelReached())
            WinToGame();
        else
            LevelProgressionController();
    }

    // Handles the logic for when the game is won
    public void WinToGame()
    {
        int earnedGold = EarnedRewardPoolManager.Instance.CalculateAmountofGoldEarned();

        // If gold was earned, update the player's gold
        if (earnedGold > 0)
        {
            int newGold = PlayerPrefs.GetInt(GlobalUtils.goldPrefKey) + earnedGold;
            UIManager.Instance.SetGoldText(newGold);
            PlayerPrefs.SetInt(GlobalUtils.goldPrefKey, newGold);
        }

        // Restart the game after winning
        RestartGame();
    }

    // Resets the game state for a fresh start
    public void RestartGame()
    {
        lastReviveIndex = 0;
        UIManager.Instance.SetReviveCostText();
        UIManager.Instance.StatusSpinAndExitButton(true);
        scrollController.ResetSystem();
        LevelManager.Instance.ResetCurrentLevel();
        LevelProgressionController();
        EarnedRewardPoolManager.Instance.ClearEarnedReward();
    }

    // Gets the current revive cost based on the index
    public int GetReviveCost()
    {
        return gameSettings.reviveCosts[lastReviveIndex];
    }

    // Increases the revive cost index for the next revive
    public void IncreaseReviveCostIndex()
    {
        if (lastReviveIndex < gameSettings.reviveCosts.Count)
            lastReviveIndex++;
    }
    #endregion
}
