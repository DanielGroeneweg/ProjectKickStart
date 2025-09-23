using UnityEngine;
using UnityEngine.Events;
public class Cheat : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private LocationTracker locationTracker;
    public UnityEvent clicked;
    public void CheatSeeds()
    {
        foreach (GrowAPlant.Seed seed in inventory.seeds)
        {
            if (seed.planted)
            {
                seed.distance += 2;
            }
        }
    }

    public void CheatGPS()
    {
        locationTracker.Cheat();
    }

    private void Update()
    {
        Input.simulateMouseWithTouches = true;
        if (Input.GetKeyDown(KeyCode.Space)) clicked?.Invoke();

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).tapCount >= 2) clicked?.Invoke();
        }
        
    }
}