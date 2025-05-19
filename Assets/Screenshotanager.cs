using UnityEngine;
using System.Collections;
using System.IO;
using NativeGalleryNamespace; // Asegúrate de importar esto

public class ScreenshotManager : MonoBehaviour
{
    public void TakeScreenshot()
    {
        StartCoroutine(Capture());
    }

    IEnumerator Capture()
    {
        yield return new WaitForEndOfFrame();

        string fileName = $"screenshot_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
        string path = Path.Combine(Application.persistentDataPath, fileName);

        ScreenCapture.CaptureScreenshot(fileName); // Guarda en dataPath
        Debug.Log("Captura guardada temporalmente en: " + path);

        // Espera un poco para que el archivo se escriba
        yield return new WaitForSeconds(0.5f);

        // Copiar a la galería
        NativeGallery.SaveImageToGallery(path, "ARttoo", fileName);

        Debug.Log("📸 Copiada a la galería");
        UIManager.Instance.ShowMessage("¡Captura guardada en galería!");
    }
}