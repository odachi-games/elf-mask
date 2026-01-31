using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class Totem : MonoBehaviour
{
    public float interactionTime = 3f;
    public string eventOnStart;
    public string eventToEmit;
    public Sprite completedSprite;

    [Header("UI Reference")]
    public Slider progressSlider;

    private float _currentTimer;
    private bool _isCompleted;
    private bool _startedInteracting;
    private SpriteRenderer _sr;

    private Animator _animator;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        if (progressSlider != null)
        {
            progressSlider.maxValue = interactionTime;
            progressSlider.value = 0;
            progressSlider.gameObject.SetActive(false);
        }
    }

    public void ProgressInteraction(float deltaTime)
    {
        if (_isCompleted) return;

        if (!_startedInteracting)
        {
            _startedInteracting = true;
            if (progressSlider != null) progressSlider.gameObject.SetActive(true);
            GameEventManager.Instance.TriggerEvent(new GameEvent(eventOnStart, this.gameObject));
        }

        _currentTimer += deltaTime;

        if (progressSlider != null)
        {
            progressSlider.value = _currentTimer;
        }

        if (_currentTimer >= interactionTime)
        {
            CompleteTotem();
        }
    }

    private void CompleteTotem()
{
    _isCompleted = true;

    if (_animator != null)
    {
        _animator.SetBool("isCompleted", true);
        _animator.enabled = false; 
    }

    if (progressSlider != null) progressSlider.gameObject.SetActive(false);

    if (completedSprite != null)
    {
        _sr.sprite = completedSprite;
    }

    GameEvent e = new GameEvent(eventToEmit, this.gameObject);
    GameEventManager.Instance.TriggerEvent(e);

    this.enabled = false;
}
}