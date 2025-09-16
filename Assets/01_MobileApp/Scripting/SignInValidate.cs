using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Collections;
public class SignInValidate : MonoBehaviour
{
    private string username;
    [SerializeField] private Inventory inventory;
    [SerializeField] private UnityEvent Creationsuccess;
    [SerializeField] private UnityEvent CreationFail;
    [SerializeField] private UnityEvent SignInSuccess;
    [SerializeField] private UnityEvent SignInFail;
    [SerializeField] private UnityEvent InvalidInput;
    [SerializeField] private UnityEvent Error;

    private static string url = "https://api.statusloop.nl/";
    public void SetUsername(string name)
    {
        username = name;
    }
    public void CreateAccount()
    {
        StartCoroutine(Validate(true));
    }
    public void SignIn()
    {
        StartCoroutine(Validate(false));
    }
    private IEnumerator Validate(bool isNewAccount)
    {
        if (string.IsNullOrEmpty(username))
        {
            InvalidInput?.Invoke();
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get($"{url}users/{username}");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Error?.Invoke();
            yield break;
        }

        bool? isInDataBase = IsNameInDataBase(request);

        if (isInDataBase == null)
        {
            Error?.Invoke();
            yield break;
        }

        if (isNewAccount)
        {
            if (isInDataBase == true) CreationFail?.Invoke();
            else
            {
                inventory.username = username;
                Creationsuccess?.Invoke();
            }
        }
        else
        {
            if (isInDataBase == true)
            {
                inventory.username = username;
                SignInSuccess?.Invoke();
            }
            else SignInFail?.Invoke();
        }
    }
    private bool? IsNameInDataBase(UnityWebRequest request)
    {
        string rawJson = request.downloadHandler.text;

        // Manually wrap the array in an object
        string wrappedJson = "{ \"users\": " + rawJson + " }";

        UserListWrapper wrapper = JsonUtility.FromJson<UserListWrapper>(wrappedJson);

        if (wrapper != null && wrapper.users != null)
        {
            Debug.Log("Parsed users: " + wrapper.users.Length);
            if (wrapper.users.Length > 0) return true;
            else return false;
        }

        return null;
    }
}
