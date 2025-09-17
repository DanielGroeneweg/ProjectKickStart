using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
public class PlantEmotions : MonoBehaviour
{
    #region Variables
    [Serializable] private class PlantStatusses
    {
        public bool hasMoved = true;
    }
    #region Inspector
    [Header("Stats")]
    [Tooltip("DO NOT TOUCH, DEBUGGING PURPOSES ONLY")]
    [SerializeField] private PlantStatusses plantStatusses;
    [Tooltip("How often the plant's emotion is updated")]
    [SerializeField] private float updateTime = 15;
    [SerializeField] private float distanceForMoney = 0;
    [SerializeField] private int moneyCashOut = 0;

    [Header("References")]
    [SerializeField] private LocationTracker locationTracker;
    [SerializeField] private RawImage plantImage;
    [SerializeField] private Inventory inventory;
    #endregion

    #region Internal
    #endregion

    #endregion
    private void Start()
    {
        InvokeRepeating(nameof(UpdateMoney), 0, updateTime);
        InvokeRepeating(nameof(UpdateEmotions), 0, updateTime);
    }
    private void UpdateMoney()
    {
        inventory.distanceWalked += locationTracker.distanceWalked;

        foreach (GrowAPlant.Seed seed in inventory.seeds)
        {
            if (seed != null && seed.planted) seed.distance += locationTracker.distanceWalked;
        }

        locationTracker.distanceWalked = 0;
        while (inventory.distanceWalked >= distanceForMoney)
        {
            inventory.distanceWalked -= distanceForMoney;

            MoneyHandler.instance.AddCoins(moneyCashOut);
        }
    }
    private void UpdateEmotions()
    {
        CheckWalking();
    }
    private void CheckWalking()
    {
        bool value = plantStatusses.hasMoved;

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

        if (value != plantStatusses.hasMoved)
        {
            User user = new User
            {
                name = inventory.username,
                skinID = inventory.equippedSkin.ID,
                hasWalked = plantStatusses.hasMoved
            };

            string json = JsonUtility.ToJson(user);

            UnityWebRequest request = new UnityWebRequest($"https://api.statusloop.nl/users/", "PUT");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            request.SendWebRequest();
        }
    }
}