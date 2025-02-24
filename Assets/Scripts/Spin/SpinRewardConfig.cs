using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SpinRewards", menuName = "SpinRewardConfig")]
public class SpinRewardConfig : SerializedScriptableObject
{
    private static SpinRewardConfig _instance;

    #region Fields
    // List to store SpinRewardEntry objects that hold spin reward information.
    [ShowInInspector]
    [GUIColor(nameof(GetSpinRewardEntryColor))]
    [InfoBox("Duplicate SpinTypes detected!", InfoMessageType.Warning, nameof(HasDuplicateSpinTypes))]
    public List<SpinRewardEntry> spinRewardEntry = new List<SpinRewardEntry>();
    #endregion

    #region Properties
    // Singleton instance for accessing SpinRewardConfig across the project.
    public static SpinRewardConfig Instance
    {
        get
        {
            // If the instance is not available, load it from the Resources folder.
            if (_instance == null)
            {
                _instance = Resources.Load<SpinRewardConfig>("SpinRewards");
                if (_instance == null)
                {
                    Debug.LogError("SpinRewardConfig singleton instance not found!");
                }
            }
            return _instance;
        }
    }
    #endregion

    #region Helper Methods
    // Returns the color for the SpinRewardEntry list. Red if duplicate SpinTypes are detected.
    private Color GetSpinRewardEntryColor()
    {
        return HasDuplicateSpinTypes() ? new Color(1f, 0.5f, 0.5f) : Color.white;
    }

    // Checks if there are any duplicate SpinTypes in the SpinRewardEntry list.
    private bool HasDuplicateSpinTypes()
    {
        return spinRewardEntry.GroupBy(e => e.spinType).Any(g => g.Count() > 1);
    }
    #endregion

    #region Validation
    // Validates the SpinRewardEntry list to ensure the data is correctly set up.
    private void OnValidate()
    {
        // Validate each SpinRewardEntry in the list for correct order and rewards.
        SpinRewardValidator.ValidateRewards(spinRewardEntry);
    }
    #endregion
}
