using UnityEngine;
using TMPro;

public class DebugEvent : MonoBehaviour
{
    [Header("Configuração")]
    public string eventToWatch;
    public string logPrefix = "Recebido: ";

    [Header("UI")]
    public TextMeshProUGUI debugText;

    private void OnEnable()
    {
        GameEventManager.Instance.Subscribe(eventToWatch, HandleDebugEvent);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.Unsubscribe(eventToWatch, HandleDebugEvent);
    }

    private void HandleDebugEvent(GameEvent gameEvent)
    {
        string timestamp = System.DateTime.Now.ToString("HH:mm:ss");
        string message = $"[{timestamp}] {logPrefix} {gameEvent.EventName}";

        if (gameEvent.EventData != null)
        {
            message += $" | Data: {gameEvent.EventData}";
        }

        if (debugText != null)
        {
            debugText.text = message;
        }

        //Debug.Log("<color=yellow>[DEBUG SCRIPT]</color> " + message);
    }
}