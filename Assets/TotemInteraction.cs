using UnityEngine;

public class TotemInteraction : MonoBehaviour
{
    public KeyCode interactionKey = KeyCode.E;
    private Totem _currentTotem;

    private void Update()
    {
        if (_currentTotem != null && Input.GetKey(interactionKey))
        {
            _currentTotem.ProgressInteraction(Time.deltaTime);
        }

        if (Input.GetKeyUp(interactionKey) && _currentTotem != null)
        {
            _currentTotem.ResetTimer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Totem>(out var totem))
        {
            _currentTotem = totem;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_currentTotem != null && other.gameObject == _currentTotem.gameObject)
        {
            _currentTotem.ResetTimer();
            _currentTotem = null;
        }
    }
}