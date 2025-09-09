using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;
public class StepAndLocation : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool useStepCounter = true;
    [SerializeField] private bool useGPS = true;
    [SerializeField] private bool useLightSensor = true;
    private WebCamTexture cameraTexture;

    [Header("References")]
    [SerializeField] private TMP_Text stepCounter;
    [SerializeField] private TMP_Text locationDisplay;
    [SerializeField] private TMP_Text lightDisplay;

    Vector3 prevAccel;
    float stepThreshold = 1.2f;
    int steps = 0;
    IEnumerator Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Debug.Log("Asking Permission for Camera");
            Permission.RequestUserPermission(Permission.Camera);
        }

        // Check if permission is already granted
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Debug.Log("Asking Permission for Location");
            // Request permission
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        // Start the first available camera
        if (WebCamTexture.devices.Length > 0)
        {
            cameraTexture = new WebCamTexture();
            cameraTexture.Play();
        }
        else
        {
            Debug.LogWarning("No camera found on this device.");
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
    private void Update()
    {
        // Location for steps
        if (useGPS) Position();

        // Acceleration for steps
        if (useStepCounter) Acceleration();

        // Light for steps
        if (useLightSensor) Light();
    }
    private void Position()
    {
        Debug.Log(Input.location.status);
        if (Input.location.status == LocationServiceStatus.Running)
        {
            LocationInfo info = Input.location.lastData;
            Vector3 pos = new Vector3(
                (float)info.latitude,   // X = latitude
                (float)info.longitude,  // Y = longitude
                (float)info.altitude    // Z = altitude
            );

            Debug.Log($"Position: {pos}");
            locationDisplay.text = $"{pos.x},{pos.y},{pos.z}";
        }
    }
    private void Acceleration()
    {
        Debug.Log($"accel: {Input.acceleration}");
        Vector3 accel = Input.acceleration;
        float delta = (accel - prevAccel).magnitude;

        if (delta > stepThreshold)
        {
            steps++;
            stepCounter.text = $"{steps}";
        }

        prevAccel = accel;
    }
    private void Light()
    {
        if (cameraTexture == null || !cameraTexture.isPlaying)
        {
            Debug.Log("No Camera");
            return;
        }

        // Get camera pixels (small downsample for performance)
        Color32[] pixels = cameraTexture.GetPixels32();
        if (pixels.Length == 0) return;

        // Compute average brightness (grayscale)
        float total = 0f;
        for (int i = 0; i < pixels.Length; i += 10) // sample every 10th pixel to improve performance
        {
            Color32 c = pixels[i];
            float brightness = (c.r + c.g + c.b) / 3f / 255f; // normalize 0-1
            total += brightness;
        }

        float avgBrightness = total / (pixels.Length / 10);
        float approximateLux = avgBrightness * 1000f; // scale to approximate lux

        lightDisplay.text = $"{approximateLux:F1} lux";
    }

    void OnDisable()
    {
        if (cameraTexture != null) cameraTexture.Stop();
    }
}