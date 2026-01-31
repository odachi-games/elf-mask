using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Trigger2Event : MonoBehaviour
{
    public string eventName;
    public bool sendColliderAsData = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("Encontrou o player");

        object data = sendColliderAsData ? other.gameObject : null;
        GameEvent gameEvent = new GameEvent(eventName, data);

        GameEventManager.Instance.TriggerEvent(gameEvent);
    }

}