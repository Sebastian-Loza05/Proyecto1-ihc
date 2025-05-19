using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TattooPlacementController : MonoBehaviour
{
    [Header("Asignar en Inspector")]
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private GameObject tattooPrefab;

    // estado interno
    private GameObject spawnedTattoo;
    private readonly List<ARRaycastHit> hits = new();

    void Update()
    {
        if (tattooPrefab == null || raycastManager == null) return;
        if (Input.touchCount == 0) return;

        // Un solo toque = colocar o arrastrar
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // Raycast “sin filtro” (planos + puntos)
            if (raycastManager.Raycast(touch.position, hits))
            {
                Pose hitPose = hits[0].pose;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        // Sólo instanciamos si aún NO existe ningún tatuaje
                        if (spawnedTattoo == null)
                        {
                            spawnedTattoo = Instantiate(tattooPrefab, hitPose.position, hitPose.rotation);
                        }
                        break;

                    case TouchPhase.Moved:
                        // Mover el tatuaje existente
                        if (spawnedTattoo != null)
                            spawnedTattoo.transform.position = hitPose.position;
                        break;

                    // Quitamos el TouchPhase.Ended para no liberar spawnedTattoo
                }
            }
        }
        // Dos dedos = escalar + rotar (sólo si ya existe el tatuaje)
        else if (Input.touchCount == 2 && spawnedTattoo != null)
        {
            var t0 = Input.GetTouch(0);
            var t1 = Input.GetTouch(1);

            // --- ESCALAR con pinch ---
            float prevDist = (t0.position - t0.deltaPosition - (t1.position - t1.deltaPosition)).magnitude;
            float currDist = (t0.position - t1.position).magnitude;
            if (prevDist > 0f)
            {
                float scaleFactor = currDist / prevDist;
                spawnedTattoo.transform.localScale *= scaleFactor;
            }

            // --- ROTAR con twist ---
            Vector2 prevDir = (t1.position - t1.deltaPosition) - (t0.position - t0.deltaPosition);
            Vector2 currDir = t1.position - t0.position;
            float angle = Vector2.SignedAngle(prevDir, currDir);
            spawnedTattoo.transform.Rotate(0f, 0f, angle, Space.World);
        }
    }

    public void SelectTattooPrefab(GameObject newPrefab) {
    tattooPrefab = newPrefab;
    if (spawnedTattoo != null)
    {
        Destroy(spawnedTattoo);
        spawnedTattoo = null;
    }
}
}
