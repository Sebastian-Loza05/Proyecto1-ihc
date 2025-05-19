using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class HiddenPlaneNotifier : MonoBehaviour
{
    ARPlaneManager _planeManager;
    bool hasDetectedPlane = false;

    void Awake()
    {
        _planeManager = GetComponent<ARPlaneManager>();
    }

    void OnEnable()
    {
        _planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        _planeManager.planesChanged -= OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (hasDetectedPlane) return;

        if (args.added.Count > 0)
        {
            hasDetectedPlane = true;
            UIManager.Instance.NotifyFirstPlaneDetected();

            foreach (var plane in args.added)
                plane.gameObject.SetActive(false); // Ocultar si aún hay prefab
        }
    }
}