using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections;
public class HTTPTest : MonoBehaviour
{
    Dictionary<string, string> dic = new Dictionary<string, string>();
    private void HelpMe()
    {
        UnityWebRequest.Post("gaming.statusloop.nl", dic);
    }

    private string url = "https://gaming.statusloop.nl/"; // <-- must be HTTPS if on Android

    private IEnumerator Start()
    {
        Debug.Log("Requesting");
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error fetching JSON: " + request.error);
        }
        else
        {
            string json = request.downloadHandler.text;
            Debug.Log("Got JSON: " + json);
        }

        Debug.Log(request.result);
    }
}
