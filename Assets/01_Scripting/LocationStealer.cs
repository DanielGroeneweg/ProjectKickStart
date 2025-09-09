using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using UnityEngine.Android;
public class LocationStealer : MonoBehaviour
{
    [SerializeField] private TMP_Text stepCounter;
    [SerializeField] private TMP_Text locationDisplay;
    IEnumerator Start()
    {
        // Check if permission is already granted
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Debug.Log("Asking Permission");
            // Request permission
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        // First, check if user has location enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogWarning("Location not enabled by user.");
            yield break;
        }

        // Start location service (desiredAccuracyInMeters, updateDistanceInMeters)
        Input.location.Start(5f, 5f);

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service failed to initialize
        if (maxWait < 1 || Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogWarning("Unable to initialize location services.");
            yield break;
        }

        // Now Input System can access the Location device
        Debug.Log("Location tracking started.");
    }
    public void OnStep(InputValue input)
    {
        int value = input.Get<int>();
        stepCounter.text = value.ToString();
    }
    public void OnLocation(InputValue input)
    {
        Vector3 value = input.Get<Vector3>();
        locationDisplay.text = $"{value.x},{value.y},{value.z}";
    }
    public void OnButton(InputValue input)
    {
        Debug.Log(input);
        //Test
    }
    public InputAction locationAction;
    void OnEnable()
    {
        locationAction.Enable();
        locationAction.performed += ctx =>
        {
            Vector3 pos = ctx.ReadValue<Vector3>();
            Debug.Log("Tracked position: " + pos);
        };
    }
}