using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;
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

    [Header("Events")]
    public UnityEvent<Enums.States> PlantStatusChanged;

    [Header("References")]
    [SerializeField] private LocationTracker locationTracker;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Animator animator;
    #endregion

    #region Internal
    #endregion

    #endregion
    private void OnEnable()
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
        bool value = inventory.hasWalked;

        Debug.Log(value);

        if (locationTracker.MetDistanceRequirement())
        {
            plantStatusses.hasMoved = true;
            animator.SetFloat("Happiness", 1);
            animator.SetBool("NeedSun", false);
        }
        else
        {
            plantStatusses.hasMoved = false;
            animator.SetFloat("Happiness", 0);
            animator.SetBool("NeedSun", true);
        }

        if (value != plantStatusses.hasMoved)
        {
            Enums.States state = Enums.States.Happy;
            if (!plantStatusses.hasMoved) state = Enums.States.Sad;
            PlantStatusChanged?.Invoke(state);

            Debug.Log(state);

            inventory.hasWalked = plantStatusses.hasMoved;

            User user = new User
            {
                name = inventory.username,
                flowerID = inventory.equippedSkin.flower.ID,
                stemID = inventory.equippedSkin.stem.ID,
                potID = inventory.equippedSkin.pot.ID,
                hasWalked = plantStatusses.hasMoved
            };

            user.scarfID = string.Empty;
            user.hatID = string.Empty;
            user.glassesID = string.Empty;

            if (inventory.equippedSkin.scarf != null) user.scarfID = inventory.equippedSkin.scarf.ID;
            if (inventory.equippedSkin.hat != null) user.hatID = inventory.equippedSkin.hat.ID;
            if (inventory.equippedSkin.glasses != null) user.glassesID = inventory.equippedSkin.glasses.ID;

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