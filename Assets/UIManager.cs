using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] GameObject messagePanel;
    [SerializeField] TextMeshProUGUI messageText;

    bool hasShownDetectionMessage = false;

    void Awake()
    {
        Instance = this;
        messagePanel.SetActive(true); // Mostrar el mensaje al iniciar
        messageText.text = "Detectando superficie..."; // Mensaje inicial
    }

    /// <summary>
    /// Llama esto solo desde HiddenPlaneNotifier
    /// </summary>
    public void NotifyFirstPlaneDetected()
    {
        if (hasShownDetectionMessage)
            return;

        hasShownDetectionMessage = true;
        StopAllCoroutines();
        StartCoroutine(ShowMessageCoroutine("¡Superficie detectada!", 2f));
    }

    System.Collections.IEnumerator ShowMessageCoroutine(string msg, float duration)
    {
        messageText.text = msg;
        messagePanel.SetActive(true);
        yield return new WaitForSeconds(duration);
        messagePanel.SetActive(false); // Oculta permanentemente
    }
}