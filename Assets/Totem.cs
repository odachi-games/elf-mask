using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Totem : MonoBehaviour
{
    public float interactionTime = 3f;
    public string eventOnStart;
    public string eventToEmit;
    public Sprite completedSprite;

    private float _currentTimer;
    private bool _isCompleted;
    private bool _startedInteracting;
    private SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public void ProgressInteraction(float deltaTime)
    {
        if (_isCompleted) return;

        if (!_startedInteracting)
        {
            _startedInteracting = true;
            GameEventManager.Instance.TriggerEvent(new GameEvent(eventOnStart, this.gameObject));
        }

        _currentTimer += deltaTime;

        if (_currentTimer >= interactionTime)
        {
            CompleteTotem();
        }
    }

    public void ResetTimer()
    {
        // Vazio para manter o tempo acumulativo conforme solicitado
    }

    private void CompleteTotem()
    {
        _isCompleted = true;

        if (completedSprite != null)
        {
            _sr.sprite = completedSprite;
        }

        GameEvent e = new GameEvent(eventToEmit, this.gameObject);
        GameEventManager.Instance.TriggerEvent(e);

        this.enabled = false;
    }
}