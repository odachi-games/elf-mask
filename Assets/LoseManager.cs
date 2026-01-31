using UnityEngine;

public class LoseManager : MonoBehaviour
{
    [Header("Configuração de Derrota")]
    public string triggerEventName = "PlayerDied";

    private void OnEnable()
    {
        GameEventManager.Instance.Subscribe(triggerEventName, TriggerLose);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.Unsubscribe(triggerEventName, TriggerLose);
    }

    private void TriggerLose(GameEvent e)
    {
        GameEventManager.Instance.TriggerEvent(new GameEvent("LoseGame", e.EventData));
        this.enabled = false;
    }
}