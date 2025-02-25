using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LevelScrollController : MonoBehaviour
{
    #region Fields
    public RectTransform targetPoint;
    private float targetPosition;
    public ScrollRect scrollRect;
    public RectTransform content;
    public RectTransform levelPrefab; // Prefab for each level
    private float scrollDuration = 1f;
    List<LevelItemController> levelItems = new List<LevelItemController>();

    private bool isLevelItemCreated;
    public bool IsLevelItemCreated => isLevelItemCreated;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        targetPosition = targetPoint.localPosition.x;
    }

    private void Start()
    {
        scrollDuration = GameSettings.Instance.levelScrollDuration;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Generates the level prefabs and sets their properties.
    /// </summary>
    public void GenerateLevelPrefabs()
    {
        SpinManager spinManager = SpinManager.Instance;

        // Clear existing level items
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // Instantiate prefabs for each level up to MaxLevel
        for (int i = 0; i < LevelManager.Instance.MaxLevel; i++)
        {
            RectTransform levelRectTransform = Instantiate(levelPrefab, content);
            levelItems.Add(levelRectTransform.GetComponent<LevelItemController>());
            levelRectTransform.localPosition = new Vector3(i * levelRectTransform.rect.width, 0, 0);

            // Set the zone type based on level intervals
            if ((i + 1) % spinManager.GoldSpinLevelInterval == 0)
                levelItems[i].ActiveZoneType = ZoneType.SuperZone;
            else if ((i + 1) % spinManager.SilverSpinLevelInterval == 0)
                levelItems[i].ActiveZoneType = ZoneType.SafeZone;
            else
                levelItems[i].ActiveZoneType = ZoneType.Normal;

            levelItems[i].UnActiveLevel();
            levelItems[i].SetLevelText(i + 1);
            levelItems[i].HideLevel();
        }
        isLevelItemCreated = true;
    }

    /// <summary>
    /// Resets all the level items to their inactive state.
    /// </summary>
    public void ResetSystem()
    {
        foreach (var item in levelItems)
        {
            item.UnActiveLevel();
        }
    }

    /// <summary>
    /// Scrolls to the specified level with an optional duration based on whether it's the first game.
    /// </summary>
    public async void ScrollToLevel(int level, bool firstGame)
    {
        float duration = firstGame ? 0 : scrollDuration;
        if (level > LevelManager.Instance.MaxLevel - 1)
            return;

        RectTransform targetChild = content.GetChild(level).GetComponent<RectTransform>();
        float targetChildPosition = targetChild.localPosition.x;

        content.DOKill();
        content.DOLocalMoveX(targetPosition - targetChildPosition, duration).SetEase(Ease.InOutSine);

        // Deactivate the previous level's highlight
        if (level > 0)
            levelItems[level - 1].UnActiveLevel();

        if (firstGame)
        {
            await new WaitForEndOfFrame();
            foreach (var item in levelItems)
                item.ShowLevel();
        }

        // Activate the current level's highlight
        levelItems[level].ActiveLevel();
    }
    #endregion
}
