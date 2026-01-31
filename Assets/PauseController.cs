using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public enum PauseType
{
    None,
    Pause,
    Inventory,
    Win,
    Lose
}

public class PauseController : MonoBehaviour
{
    public static PauseController Instance { get; private set; }

    private PauseType currentPauseType = PauseType.None;

    [Header("Player Assignment")]
    [SerializeField] private List<Behaviour> playerScriptsToDisable = new();

    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [Header("Navigation")]
    [SerializeField] private GameObject pauseFirstButton;
    [SerializeField] private GameObject inventoryFirstButton;
    [SerializeField] private GameObject winFirstButton;
    [SerializeField] private GameObject loseFirstButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        GameEventManager.Instance.Subscribe("WinGame", OnWinGameEvent);
        GameEventManager.Instance.Subscribe("LoseGame", OnLoseGameEvent);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.Unsubscribe("WinGame", OnWinGameEvent);
        GameEventManager.Instance.Unsubscribe("LoseGame", OnLoseGameEvent);
    }

    private void Start()
    {
        pausePanel?.SetActive(false);
        inventoryPanel?.SetActive(false);
        winPanel?.SetActive(false);
        losePanel?.SetActive(false);
    }

    private void Update()
    {
        if (currentPauseType == PauseType.Win || currentPauseType == PauseType.Lose) return;

        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
        if (Input.GetKeyDown(KeyCode.V)) ToggleInventory();
    }

    private void OnWinGameEvent(GameEvent e) => SetPauseState(PauseType.Win);
    private void OnLoseGameEvent(GameEvent e) => SetPauseState(PauseType.Lose);

    public void TogglePause() => SetPauseState(currentPauseType == PauseType.Pause ? PauseType.None : PauseType.Pause);
    public void ToggleInventory() => SetPauseState(currentPauseType == PauseType.Inventory ? PauseType.None : PauseType.Inventory);

    public void SetPauseState(PauseType type)
    {
        currentPauseType = type;

        pausePanel?.SetActive(type == PauseType.Pause);
        inventoryPanel?.SetActive(type == PauseType.Inventory);
        winPanel?.SetActive(type == PauseType.Win);
        losePanel?.SetActive(type == PauseType.Lose);

        bool isPaused = type != PauseType.None;
        Time.timeScale = isPaused ? 0 : 1;

        foreach (var script in playerScriptsToDisable)
        {
            if (script != null) script.enabled = !isPaused;
        }

        if (type == PauseType.Pause) SetFocus(pauseFirstButton);
        else if (type == PauseType.Inventory) SetFocus(inventoryFirstButton);
        else if (type == PauseType.Win) SetFocus(winFirstButton);
        else if (type == PauseType.Lose) SetFocus(loseFirstButton);
    }

    private void SetFocus(GameObject target)
    {
        if (EventSystem.current == null) return;
        EventSystem.current.SetSelectedGameObject(null);
        if (target != null) EventSystem.current.SetSelectedGameObject(target);
    }

    public void ResumeGame()
    {
        if (currentPauseType == PauseType.Win || currentPauseType == PauseType.Lose) return;
        SetPauseState(PauseType.None);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}