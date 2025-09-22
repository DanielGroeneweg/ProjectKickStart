using UnityEngine;
using System.IO;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.Networking;
public class DekstopStartUp : MonoBehaviour
{
    private static string url = "https://api.statusloop.nl/";
    private static string savePath;
    [SerializeField] private UnityEvent NoFile;
    [SerializeField] private Inventory inventory;
    [SerializeField] private AvailableSkins availableSkins;
    private void Awake()
    {
        savePath = Application.persistentDataPath + "/user.json";
    }
    private void Start()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            inventory.username = data.username;
            StartCoroutine(ReadUser());
        }
        else
        {
            NoFile?.Invoke();
        }
    }
    private IEnumerator ReadUser()
    {
        UnityWebRequest request = UnityWebRequest.Get($"{url}/users/{inventory.username}");
        yield return request.SendWebRequest();

        string rawJson = request.downloadHandler.text;

        // Manually wrap the array in an object
        string wrappedJson = "{ \"users\": " + rawJson + " }";

        UserListWrapper wrapper = JsonUtility.FromJson<UserListWrapper>(wrappedJson);

        if (wrapper != null && wrapper.users != null)
        {
            if (wrapper.users.Length > 0)
            {
                inventory.equippedSkin.flower = availableSkins.skins.Find(s => s.ID == wrapper.users[0].flowerID);
                inventory.equippedSkin.stem = availableSkins.skins.Find(s => s.ID == wrapper.users[0].stemID);
                inventory.equippedSkin.pot = availableSkins.skins.Find(s => s.ID == wrapper.users[0].potID);

                inventory.equippedSkin.hat = null;
                inventory.equippedSkin.glasses = null;
                inventory.equippedSkin.scarf = null;

                if (!string.IsNullOrEmpty(wrapper.users[0].hatID)) inventory.equippedSkin.hat = availableSkins.skins.Find(s => s.ID == wrapper.users[0].hatID);
                if (!string.IsNullOrEmpty(wrapper.users[0].scarfID)) inventory.equippedSkin.scarf = availableSkins.skins.Find(s => s.ID == wrapper.users[0].scarfID);
                if (!string.IsNullOrEmpty(wrapper.users[0].glassesID)) inventory.equippedSkin.glasses = availableSkins.skins.Find(s => s.ID == wrapper.users[0].glassesID);
            }
        }
    }
    private void OnApplicationQuit()
    {
        SaveData data = new SaveData
        {
            username = inventory.username
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }
}