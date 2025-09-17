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
            Debug.Log("Parsed users: " + wrapper.users.Length);
            if (wrapper.users.Length > 0)
            {
                inventory.equippedSkin = availableSkins.skins.Find(s => s.ID == wrapper.users[0].skinID);
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