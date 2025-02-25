using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.U2D;

public class SpinController : MonoBehaviour
{
    #region Fields
    private Dictionary<SpinType, SpinSpriteAtlasData> spinSpriteAtlasData;
    public SpriteAtlas spinSpriteAtlas; // Sprite atlas containing spin images
    public Image spinImage; // Image component for the spin wheel
    public Image spinIndicatorImage; // Image component for the spin indicator
    public RectTransform wheel; // Wheel object to rotate
    private float spinDuration = 3f; // Duration of the spin animation
    private float rotationCount; // Number of complete rotations for the wheel
    private readonly int[] targetAngles = { 0, 45, 90, 135, 180, 225, 270, 315 }; // Possible target angles for the wheel
    private AnimationCurve curve; // Animation curve for spin easing
    #endregion

    #region Initialization
    private void Start()
    {
        // Get settings from the game manager
        GameSettings gameSettings = GameSettings.Instance;
        spinSpriteAtlasData = SpinManager.Instance.spinSpriteAtlasData; // Get spin sprite data
        spinDuration = gameSettings.spinDuration; // Set spin duration
        rotationCount = gameSettings.spinRotationCount; // Set the number of rotations
        curve = gameSettings.spinCurve; // Set the spin easing curve
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Starts the wheel spin towards a target index.
    /// </summary>
    /// <param name="targetIndex">The index of the target angle to spin towards.</param>
    public void SpinWheel(int targetIndex)
    {
        float targetAngle = targetAngles[targetIndex] + 360 * rotationCount;
        wheel.DORotate(new Vector3(0, 0, targetAngle - wheel.eulerAngles.z), spinDuration, RotateMode.WorldAxisAdd)
            .SetEase(curve).OnComplete(() => SpinManager.Instance.CompletedSpin());
    }

    /// <summary>
    /// Changes the spin image based on the selected spin type.
    /// </summary>
    /// <param name="spinType">The spin type that determines which images to use.</param>
    public void ChangeSpinImage(SpinType spinType)
    {
        // Update the spin and indicator images based on the spin type
        spinImage.sprite = spinSpriteAtlas.GetSprite(spinSpriteAtlasData[spinType].spinAtlasName);
        spinIndicatorImage.sprite = spinSpriteAtlas.GetSprite(spinSpriteAtlasData[spinType].spinIndicatorAtlasName);
    }
    #endregion
}
