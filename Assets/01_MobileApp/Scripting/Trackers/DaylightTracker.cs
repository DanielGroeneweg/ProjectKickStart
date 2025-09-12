using UnityEngine;
using UnityEngine.Android;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
public class DaylightTracker : MonoBehaviour
{
    #region Variables

    #region Inspector
    [Header("Stats")]
    [Tooltip("The minimum amount of brightness needed to detect daylight")]
    [SerializeField] private float minimumLux = 800;

    [Tooltip("The minimum amount of time needed to have daylight in seconds")]
    [SerializeField] private float minimumTime = 300;

    [Tooltip("The interval in which the user needs to reach the minimum sunlight time")]
    [SerializeField] private float daylightInterval = 1800;
    #endregion

    #region Internal
    private WebCamTexture cameraTexture;
    private float lux = 0;

    private bool isCounting = false;
    private Dictionary<DateTime, float> datedSunlight = new Dictionary<DateTime, float>();
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
        if (!isCounting && lux >= minimumLux) StartCoroutine(LogSunTime());
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

        lux = approximateLux;
    }
    /// <summary>
    /// Keeps track of consecutive daylight and logs it
    /// </summary>
    /// <returns></returns>
    private IEnumerator LogSunTime()
    {
        isCounting = true;
        float counter = 0;
        while (isCounting)
        {
            if (lux >= minimumLux)
            {
                counter += Time.deltaTime;
            }
            else
            {
                datedSunlight.Add(DateTime.Now, counter);
                isCounting = false;
                counter = 0;
            }
            yield return null;
        }
    }
    /// <summary>
    /// Checks whether the daylight intake is equal to the minimum and returns true or false
    /// </summary>
    /// <returns></returns>
    public bool MetDaylightRequirement()
    {
        DateTime intervalMinutesAgo = DateTime.Now.AddMinutes(-daylightInterval / 60f);
        float sunlightTime = 0;

        // Convert keys to a list so they can be indexed
        var keys = datedSunlight.Keys.ToList();

        for (int i = keys.Count - 1; i >= 0; i--)
        {
            DateTime time = keys[i];
            if (time < intervalMinutesAgo) datedSunlight.Remove(time);
            else sunlightTime += datedSunlight[time];
        }

        return (sunlightTime >= minimumTime);
    }
    void OnDisable()
    {
        if (cameraTexture != null) cameraTexture.Stop();
    }
}