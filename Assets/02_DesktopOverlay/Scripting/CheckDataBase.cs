using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class CheckDataBase : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private AvailableSkins availableSkins;
    [SerializeField] private float checkTime;

    private static string url = "https://api.statusloop.nl/";
    void Start()
    {
        InvokeRepeating(nameof(Check), 0, checkTime);
    }
    private IEnumerator Check()
    {
        if (string.IsNullOrEmpty(inventory.name)) yield break;

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
                inventory.hasWalked = wrapper.users[0].hasWalked;
            }
        }
    }
}
