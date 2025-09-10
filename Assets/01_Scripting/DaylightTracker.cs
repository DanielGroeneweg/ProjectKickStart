using UnityEngine;
using TMPro;
using UnityEngine.Android;
public class DaylightTracker : MonoBehaviour
{
    #region Variables

    #region Inspector
    [Header("Stats")]
    [Tooltip("The minimum amount of brightness needed to detect daylight")]
    [SerializeField] private float minimumLux = 800;

    [Tooltip("The minimum amount of time needed to have daylight in seconds")]
    [SerializeField] private float minimumTime = 300;

    [Header("References")]
    [SerializeField] private TMP_Text lightDisplay;
    #endregion

    #region Internal
    private WebCamTexture cameraTexture;
    private float consecutiveLightTime = 0;
    private float lux = 0;
    #endregion

    #endregion
    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Debug.Log("Asking Permission for Camera");
            Permission.RequestUserPermission(Permission.Camera);
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
    }
    private void Update()
    {
        TrackDaylight();
    }
    /// <summary>
    /// Stores the current amount of light into a variable based on the camera
    /// </summary>
    private void TrackDaylight()
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
        lux = approximateLux;
    }
    void OnDisable()
    {
        if (cameraTexture != null) cameraTexture.Stop();
    }
}