using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSManager : MonoBehaviour
{
    public static double Latitude { get; private set; }
    public static double Longitude { get; private set; }

    private void Start()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location services are not enabled");
            return;
        }

        // Start location updates
        Input.location.Start(1f);

        // Wait until location data is available
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            maxWait--;
            Debug.Log("Waiting for location data...");
            System.Threading.Thread.Sleep(1000);
        }

        // Check if location data was obtained
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            return;
        }

        // Get the device's current location
        Latitude = Input.location.lastData.latitude;
        Longitude = Input.location.lastData.longitude;
    }

    private void OnDestroy()
    {
        // Stop location updates when the object is destroyed
        Input.location.Stop();
    }
}
