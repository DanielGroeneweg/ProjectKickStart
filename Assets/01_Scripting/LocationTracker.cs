using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Android;
using System.Collections.Generic;
using System.Linq;
using System;

public class LocationTracker : MonoBehaviour
{
    #region floatiables

    #region Inspector
    [Header("Stats")]
    [Tooltip("The minimum amount of distance needed to walk in meters")]
    [SerializeField] private float minimumWalkDistance = 1000;

    [Tooltip("The minimum amount of time needed to walk in seconds")]
    [SerializeField] private float minimumTime = 600;

    [Tooltip("The minimum distance needed to be away from home location to count as walking")]
    [SerializeField] private float minDistFromHome = 20;

    [Tooltip("The maximum distance needed to be away from home location to count as walking")]
    [SerializeField] private float maxDistFromHome = 2000;

    [Tooltip("The time interval in which a user needs to walk in seconds")]
    [SerializeField] private float walkingInterval = 1800;
    #endregion

    #region Internal
    private Vector3 location;
    private Vector3 lastLocation;
    private Vector3 homeLocation;

    private Dictionary<DateTime, float> timedDistances = new Dictionary<DateTime, float>();
    #endregion

    #endregion
    private IEnumerator Start()
    {
        // Check if permission is already granted
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Debug.Log("Asking Permission for Location");
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

        if (Input.location.status == LocationServiceStatus.Running)
        {
            LocationInfo info = Input.location.lastData;
            Vector3 pos = new Vector3(
                (float)info.latitude,   // X = latitude
                (float)info.longitude,  // Y = longitude
                (float)info.altitude    // Z = altitude
            );

            location = pos;
            lastLocation = pos;
            homeLocation = pos;
        }
    }
    private void Update()
    {
        /*
        TrackPosition();

        DistanceTracker();
        */

        if (Input.GetKeyDown(KeyCode.Space)) timedDistances.Add(DateTime.Now, 1);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) timedDistances.Add(DateTime.Now, 1);
        }
    }
    /// <summary>
    /// Checks and logs distances travelled in the dictionary
    /// </summary>
    private void DistanceTracker()
    {
        if (location != lastLocation)
        {
            float distance = DistanceCalculator(location.x, location.y, lastLocation.x, lastLocation.y);

            // Check for reasonable walking distance
            float distanceToHome = DistanceCalculator(location.x, location.y, homeLocation.x, homeLocation.y);
            if (distanceToHome > minDistFromHome && distanceToHome < maxDistFromHome)
                timedDistances.Add(DateTime.Now, distance);
        }
    }
    /// <summary>
    /// Loops through the dictionary, removing old data, adding up the distance walked and returning if that is bigger than the minimum
    /// </summary>
    /// <returns></returns>
    public bool MetDistanceRequirement()
    {
        DateTime intervalMinutesAgo = DateTime.Now.AddMinutes(-walkingInterval / 60f);
        float distanceWalked = 0;

        // Convert keys to a list so they can be indexed
        var keys = timedDistances.Keys.ToList();

        for (int i = keys.Count - 1; i >= 0; i--)
        {
            DateTime time = keys[i];
            if (time < intervalMinutesAgo) timedDistances.Remove(time);
            else distanceWalked += timedDistances[time];
        }

        return (distanceWalked >= minimumWalkDistance);
    }
    /// <summary>
    /// Stores the location of the user
    /// </summary>
    private void TrackPosition()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            LocationInfo info = Input.location.lastData;
            Vector3 pos = new Vector3(
                (float)info.latitude,   // X = latitude
                (float)info.longitude,  // Y = longitude
                (float)info.altitude    // Z = altitude
            );
            location = pos;
        }
    }
    /// <summary>
    /// Returns the distance between two GPS coordinates
    /// </summary>
    /// <param name="lat1"></param>
    /// <param name="lon1"></param>
    /// <param name="lat2"></param>
    /// <param name="lon2"></param>
    /// <returns></returns>
    private float DistanceCalculator(float lat1, float lon1, float lat2, float lon2)
    {
        float R = 6378.137f; // Radius of earth in KM
        float dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
        float dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;
        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
        Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
        Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        float d = R * c;
        return (float)(d * 1000); // meters
    }
}