using UnityEngine;
using System.Collections;

public class WinTotemDisabled : MonoBehaviour
{
    [Header("Configuração de Vitória")]
    public string totemDisabledEvent = "TotemAtivado";
    public int totalTotemsRequired = 3;
    public float winDelaySeconds = 2f;

    [Header("Estado Atual")]
    [SerializeField] private int _currentDisabledCount = 0;
    private bool _isWinTriggered = false;

    private void OnEnable()
    {
        GameEventManager.Instance.Subscribe(totemDisabledEvent, OnTotemDisabled);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.Unsubscribe(totemDisabledEvent, OnTotemDisabled);
    }

    private void OnTotemDisabled(GameEvent e)
    {
        if (_isWinTriggered) return;

        _currentDisabledCount++;

        if (_currentDisabledCount >= totalTotemsRequired)
        {
            _isWinTriggered = true;
            StartCoroutine(WaitAndWin());
        }
    }

    private IEnumerator WaitAndWin()
    {
        yield return new WaitForSecondsRealtime(winDelaySeconds);

        GameEvent winEvent = new GameEvent("WinGame", this.gameObject);
        GameEventManager.Instance.TriggerEvent(winEvent);

        this.enabled = false;
    }
}