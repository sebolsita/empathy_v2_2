using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

namespace Amused.XR
{
    /// <summary>
    /// Manages the overall game state, handling scene transitions, dialogue, UI updates, and user interactions.
    /// Implements a state machine for cleaner logic flow.
    /// </summary>
    //public class GameManager : MonoBehaviour
    //{
    //    #region PUBLIC PROPERTIES

    //    public static GameManager Instance { get; private set; } // Singleton instance
    //    public GameState CurrentState { get; private set; } = GameState.MainMenu;

    //    #endregion

    //    #region SERIALIZED PRIVATE FIELDS

    //    [Header("Yarn Dialogue")]
    //    [SerializeField] private DialogueRunner _dialogueRunner; // Handles in-game dialogue

    //    [Header("Scenario Management")]
    //    [SerializeField] private float _sceneTransitionDelay = 1.5f; // Delay before transitioning scenes
    //    [SerializeField] private int _currentScenarioIndex = 0; // Tracks which scenario is active

    //    [Header("UI & Gameplay Elements")]
    //    [SerializeField] private GameObject _mainMenuCanvas;
    //    [SerializeField] private GameObject _gameplayCanvas;
    //    [SerializeField] private GameObject _pauseMenuCanvas;
    //    [SerializeField] private GameObject _scenarioCompleteCanvas;
    //    [SerializeField] private GameObject _questionnaireCanvas;
    //    [SerializeField] private GameObject _creditsCanvas;

    //    #endregion

    //    #region PRIVATE FIELDS

    //    private bool _isGamePaused = false;
    //    private Coroutine _cachedTransitionCoroutine;

    //    #endregion

    //    #region UNITY METHODS

    //    private void Awake()
    //    {
    //        if (Instance == null)
    //        {
    //            Instance = this;
    //            DontDestroyOnLoad(gameObject);
    //        }
    //        else
    //        {
    //            Destroy(gameObject);
    //            return;
    //        }
    //    }

    //    private void Start()
    //    {
    //        LoadProgress();
    //        SetState(GameState.MainMenu);
    //    }

    //    #endregion

    //    #region PUBLIC METHODS

    //    /// <summary>
    //    /// Changes the current game state and updates relevant game elements accordingly.
    //    /// </summary>
    //    public void SetState(GameState newState)
    //    {
    //        Debug.Log($"[GameManager] State changed from {CurrentState} to {newState}");
    //        CurrentState = newState;

    //        switch (newState)
    //        {
    //            case GameState.MainMenu:
    //                ShowMainMenu();
    //                break;

    //            case GameState.Playing:
    //                StartGameplay();
    //                break;

    //            case GameState.Dialogue:
    //                StartDialogue();
    //                break;

    //            case GameState.Transitioning:
    //                StartCoroutine(SceneTransition());
    //                break;

    //            case GameState.Paused:
    //                PauseGame();
    //                break;

    //            case GameState.Questionnaire:
    //                TriggerQuestionnaire();
    //                break;

    //            case GameState.Credits:
    //                ShowCredits();
    //                break;

    //            case GameState.GameOver:
    //                DisplayScenarioResults();
    //                break;
    //        }
    //    }

    //    /// <summary>
    //    /// Loads the next scenario, saves progress, and starts the transition.
    //    /// </summary>
    //    public void LoadNextScenario()
    //    {
    //        _currentScenarioIndex++;
    //        SaveProgress();
    //        SetState(GameState.Transitioning);
    //    }

    //    /// <summary>
    //    /// Saves the player's progress to PlayerPrefs.
    //    /// </summary>
    //    public void SaveProgress()
    //    {
    //        PlayerPrefs.SetInt("ScenarioIndex", _currentScenarioIndex);
    //        PlayerPrefs.Save();
    //        Debug.Log($"[GameManager] Progress saved: Scenario {_currentScenarioIndex}");
    //    }

    //    /// <summary>
    //    /// Loads the player's last saved progress.
    //    /// </summary>
    //    public void LoadProgress()
    //    {
    //        _currentScenarioIndex = PlayerPrefs.GetInt("ScenarioIndex", 0);
    //        Debug.Log($"[GameManager] Loaded progress: Scenario {_currentScenarioIndex}");
    //    }

    //    /// <summary>
    //    /// Fully resets the game progress and returns to the main menu.
    //    /// </summary>
    //    public void ResetGame()
    //    {
    //        _currentScenarioIndex = 0;
    //        SaveProgress();
    //        SceneManager.LoadScene(0);
    //        SetState(GameState.MainMenu);
    //        Debug.Log("[GameManager] Game Reset");
    //    }

    //    /// <summary>
    //    /// Starts Yarn dialogue for the current scenario.
    //    /// </summary>
    //    public void StartDialogue()
    //    {
    //        if (_dialogueRunner != null)
    //        {
    //            _dialogueRunner.StartDialogue("Start");
    //        }
    //    }

    //    /// <summary>
    //    /// Ends dialogue and resumes gameplay.
    //    /// </summary>
    //    public void EndDialogue()
    //    {
    //        SetState(GameState.Playing);
    //    }

    //    /// <summary>
    //    /// Pauses the game and displays the pause menu.
    //    /// </summary>
    //    public void PauseGame()
    //    {
    //        _isGamePaused = true;
    //        Time.timeScale = 0f;
    //        _pauseMenuCanvas.SetActive(true);
    //        Debug.Log("[GameManager] Game Paused");
    //    }

    //    /// <summary>
    //    /// Resumes the game after being paused.
    //    /// </summary>
    //    public void ResumeGame()
    //    {
    //        _isGamePaused = false;
    //        Time.timeScale = 1f;
    //        _pauseMenuCanvas.SetActive(false);
    //        Debug.Log("[GameManager] Game Resumed");
    //    }

    //    /// <summary>
    //    /// Displays the post-scenario questionnaire.
    //    /// </summary>
    //    public void TriggerQuestionnaire()
    //    {
    //        _questionnaireCanvas.SetActive(true);
    //        Debug.Log("[GameManager] Displaying Questionnaire.");
    //    }

    //    /// <summary>
    //    /// Displays the credits screen.
    //    /// </summary>
    //    public void ShowCredits()
    //    {
    //        _creditsCanvas.SetActive(true);
    //        Debug.Log("[GameManager] Showing Credits.");
    //    }

    //    /// <summary>
    //    /// Displays results or feedback at the end of a scenario.
    //    /// </summary>
    //    public void DisplayScenarioResults()
    //    {
    //        _scenarioCompleteCanvas.SetActive(true);
    //        Debug.Log("[GameManager] Showing scenario results.");
    //    }

    //    /// <summary>
    //    /// Submits player data to Airtable for research tracking.
    //    /// </summary>
    //    public void SubmitPlayerData()
    //    {
    //        Debug.Log("[GameManager] Submitting player data to Airtable.");
    //        // TODO: Implement Airtable data submission logic
    //    }

    //    /// <summary>
    //    /// Quits the application.
    //    /// </summary>
    //    public void QuitGame()
    //    {
    //        Debug.Log("[GameManager] Quitting Game.");
    //        Application.Quit();
    //    }

    //    #endregion

    //    #region PRIVATE METHODS

    //    /// <summary>
    //    /// Shows the main menu UI.
    //    /// </summary>
    //    private void ShowMainMenu()
    //    {
    //        _mainMenuCanvas.SetActive(true);
    //        _gameplayCanvas.SetActive(false);
    //    }

    //    /// <summary>
    //    /// Starts gameplay and updates UI.
    //    /// </summary>
    //    private void StartGameplay()
    //    {
    //        _mainMenuCanvas.SetActive(false);
    //        _gameplayCanvas.SetActive(true);
    //    }

    //    #endregion

    //    #region COROUTINES

    //    /// <summary>
    //    /// Handles smooth scene transitions.
    //    /// </summary>
    //    private IEnumerator SceneTransition()
    //    {
    //        yield return new WaitForSeconds(_sceneTransitionDelay);
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //        SetState(GameState.Playing);
    //    }

    //    #endregion
    //}

    /// <summary>
    /// Enum representing different game states.
    /// </summary>
    public enum GameState
    {
        MainMenu,
        Playing,
        Dialogue,
        Transitioning,
        Paused,
        Questionnaire,
        Credits,
        GameOver
    }
}
