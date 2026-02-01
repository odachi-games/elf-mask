using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    [Header("Configurações de Dano")]
    [SerializeField] private PlayerStats stats;
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private string targetTag = "Player";

    [Header("Configurações de Repetição")]
    [SerializeField] private bool continuousDamage = false;
    [SerializeField] private float damageInterval = 1f;

    private float timer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            ApplyDamage();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (continuousDamage && collision.gameObject.CompareTag(targetTag))
        {
            timer += Time.deltaTime;
            if (timer >= damageInterval)
            {
                timer = 0f;
                ApplyDamage();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            timer = 0f;
        }
    }

    private void ApplyDamage()
    {
        if (stats != null)
        {
            stats.Health -= damageAmount;

            if (stats.Health < 0)
            {
                stats.Health = 0;
            }
        }
    }
}