using UnityEngine;
using System;
using UnityEngine.UI;
public class PlantEmotions : MonoBehaviour
{
    #region Variables
    [Serializable] private class PlantStatusses
    {
        public bool hasMoved = true;
        public bool hasDaylight = true;
        public bool hasWater = true;
        public bool hasFood = true;
    }
    #region Inpsector
    [Header("Stats")]
    [Tooltip("DO NOT TOUCH, DEBUGGING PURPOSES ONLY")]
    [SerializeField] private PlantStatusses plantStatusses;
    [Tooltip("How often the plant's emotion is updated")]
    [SerializeField] private float updateTime = 15;

    [Header("References")]
    [SerializeField] private LocationTracker locationTracker;
    [SerializeField] private DaylightTracker daylightTracker;
    [SerializeField] private RawImage plantImage;
    #endregion

    #region Internal
    #endregion

    #endregion
    private void Start()
    {
        InvokeRepeating(nameof(UpdateEmotions), 0, updateTime);
    }
    private void UpdateEmotions()
    {
        CheckWalking();

        CheckSunlight();
    }
    private void CheckWalking()
    {
        if (locationTracker.MetDistanceRequirement())
        {
            plantStatusses.hasMoved = true;
            plantImage.color = Color.green;
        }
        else
        {
            plantStatusses.hasMoved = false;
            plantImage.color = Color.red;
        }
    }
    private void CheckSunlight()
    {
        if (daylightTracker.MetDaylightRequirement())
        {
            plantStatusses.hasDaylight = true;
            plantImage.color = Color.green;
        }
        else
        {
            plantStatusses.hasDaylight = false;
            plantImage.color = Color.red;
        }
    }
}