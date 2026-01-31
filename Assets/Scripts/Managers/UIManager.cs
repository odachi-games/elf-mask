using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Stats Reference")]
    [SerializeField] private PlayerStats stats;

    [Header("Bars")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image suspicionBar;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI healthTMP;
    [SerializeField] private TextMeshProUGUI suspicionTMP;

    private float targetSuspicionFill;
    private float displaySuspicionValue;

    private void Start()
    {
        if (stats != null && healthBar != null)
            healthBar.fillAmount = stats.Health / stats.MaxHealth;

        if (suspicionBar != null)
            suspicionBar.fillAmount = 0;
    }

    private void Update()
    {
        UpdatePlayerUI();
    }

    public void UpdateSuspicionValue(float current, float max)
    {
        displaySuspicionValue = current;
        targetSuspicionFill = current / max;
    }

    public void ForceFullSuspicion()
    {
        targetSuspicionFill = 1f;
        displaySuspicionValue = 100f;
        if (suspicionBar != null) suspicionBar.fillAmount = 1f;
        if (suspicionTMP != null) suspicionTMP.text = "100%";
    }

    private void UpdatePlayerUI()
    {
        if (stats != null && healthBar != null)
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, stats.Health / stats.MaxHealth, 10f * Time.deltaTime);

            if (healthTMP != null)
            {
                healthTMP.text = $"{Mathf.CeilToInt(stats.Health)} / {stats.MaxHealth}";
            }
        }

        if (suspicionBar != null)
        {
            suspicionBar.fillAmount = Mathf.Lerp(suspicionBar.fillAmount, targetSuspicionFill, 10f * Time.deltaTime);

            if (suspicionTMP != null)
            {
                suspicionTMP.text = $"{Mathf.CeilToInt(displaySuspicionValue)}%";
            }
        }
    }
}