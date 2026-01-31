using UnityEngine;

public class LoseManager : MonoBehaviour
{
    [Header("Configurações de Eventos")]
    [SerializeField] private string playerSawEvent = "PlayerSaw";
    [SerializeField] private string loseEventName = "LoseGame";

    [Header("Parâmetros de Suspeita")]
    [SerializeField] private float maxSuspicion = 100f;
    [SerializeField] private float secondsToLose = 10f;

    [Header("UI Reference")]
    [SerializeField] private UIManager uiManager;

    private float currentSuspicion = 0f;

    private void Start()
    {
        if (uiManager == null)
            uiManager = FindFirstObjectByType<UIManager>();
    }

    private void OnEnable()
    {
        GameEventManager.Instance.Subscribe(playerSawEvent, OnPlayerDetected);
    }

    private void OnDisable()
    {
        if (GameEventManager.Instance != null)
            GameEventManager.Instance.Unsubscribe(playerSawEvent, OnPlayerDetected);
    }

    private void OnPlayerDetected(GameEvent e)
    {
        if (!this.enabled) return;

        float increment = (maxSuspicion / secondsToLose) * Time.deltaTime;
        currentSuspicion += increment;

        if (uiManager != null)
        {
            uiManager.UpdateSuspicionValue(currentSuspicion, maxSuspicion);
        }

        if (currentSuspicion >= maxSuspicion)
        {
            currentSuspicion = maxSuspicion;
            TriggerLose(e);
        }
    }

    private void TriggerLose(GameEvent e)
    {
        // Força a barra visual a completar antes de parar o script
        if (uiManager != null)
        {
            uiManager.ForceFullSuspicion();
        }

        GameEventManager.Instance.TriggerEvent(new GameEvent(loseEventName, e.EventData));
        this.enabled = false;
    }
}