using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlantEmotions : MonoBehaviour
{
    #region Variables

    #region Inpsector
    [Header("Stats")]
    [Tooltip("How often the plant's emotion is updated")]
    [SerializeField] private float updateTime = 15;

    [Header("References")]
    [SerializeField] private LocationTracker locationTracker;
    [SerializeField] private DaylightTracker daylightTracker;
    [SerializeField] private RawImage plantImage;
    #endregion

    #region Internal
    private enum emotions { Happy, Dry, NotMoved, NoLight }
    private emotions plantEmotion = emotions.Happy;
    #endregion

    #endregion
    private void Start()
    {
        InvokeRepeating(nameof(UpdateEmotions), 0, updateTime);
    }
    private void UpdateEmotions()
    {
        if (locationTracker.MetDistanceRequirement())
        {
            plantEmotion = emotions.Happy;
            plantImage.color = Color.green;
        }
        else
        {
            plantEmotion = emotions.NotMoved;
            plantImage.color = Color.red;
        }
    }
}