using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Amused.XR
{
    /// <summary>
    /// Manages the overall game state, handling scene transitions, UI updates, and user interactions.
    /// Implements a state machine for cleaner logic flow.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region PUBLIC PROPERTIES

        public static GameManager Instance { get; private set; }
        public GameState CurrentState { get; private set; } = GameState.MainMenu;

        #endregion

        #region SERIALIZED PRIVATE FIELDS

        [Header("Scenario Management")]
        [SerializeField] private float _sceneTransitionDelay = 1.5f;
        [SerializeField] private int _currentScenarioIndex = 0;

        [Header("UI & Gameplay Elements")]
        [SerializeField] private GameObject _mainMenuCanvas;
        [SerializeField] private GameObject _gameplayCanvas;
        [SerializeField] private GameObject _pauseMenuCanvas;
        [SerializeField] private GameObject _scenarioCompleteCanvas;
        [SerializeField] private GameObject _questionnaireCanvas;
        [SerializeField] private GameObject _creditsCanvas;

        #endregion

        #region PRIVATE FIELDS

        private bool _isGamePaused = false;
        private Coroutine _cachedTransitionCoroutine;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            LoadProgress();
            SetState(GameState.MainMenu);
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Changes the current game state and updates relevant game elements accordingly.
        /// </summary>
        public void SetState(GameState newState)
        {
            Debug.Log($"[GameManager] State changed from {CurrentState} to {newState}");
            CurrentState = newState;

            switch (newState)
            {
                case GameState.MainMenu:
                    ShowMainMenu();
                    break;

                case GameState.Playing:
                    StartGameplay();
                    break;

                case GameState.Transitioning:
                    StartCoroutine(SceneTransition());
                    break;

                case GameState.Paused:
                    PauseGame();
                    break;

                case GameState.Questionnaire:
                    TriggerQuestionnaire();
                    break;

                case GameState.Credits:
                    ShowCredits();
                    break;

                case GameState.GameOver:
                    DisplayScenarioResults();
                    break;
            }
        }

        public void LoadNextScenario()
        {
            _currentScenarioIndex++;
            SaveProgress();
            SetState(GameState.Transitioning);
        }

        public void SaveProgress()
        {
            PlayerPrefs.SetInt("ScenarioIndex", _currentScenarioIndex);
            PlayerPrefs.Save();
            Debug.Log($"[GameManager] Progress saved: Scenario {_currentScenarioIndex}");
        }

        public void LoadProgress()
        {
            _currentScenarioIndex = PlayerPrefs.GetInt("ScenarioIndex", 0);
            Debug.Log($"[GameManager] Loaded progress: Scenario {_currentScenarioIndex}");
        }

        public void ResetGame()
        {
            _currentScenarioIndex = 0;
            SaveProgress();
            SceneManager.LoadScene(0);
            SetState(GameState.MainMenu);
            Debug.Log("[GameManager] Game Reset");
        }

        public void PauseGame()
        {
            _isGamePaused = true;
            Time.timeScale = 0f;
            _pauseMenuCanvas.SetActive(true);
            Debug.Log("[GameManager] Game Paused");
        }

        public void ResumeGame()
        {
            _isGamePaused = false;
            Time.timeScale = 1f;
            _pauseMenuCanvas.SetActive(false);
            Debug.Log("[GameManager] Game Resumed");
        }

        public void TriggerQuestionnaire()
        {
            _questionnaireCanvas.SetActive(true);
            Debug.Log("[GameManager] Displaying Questionnaire.");
        }

        public void ShowCredits()
        {
            _creditsCanvas.SetActive(true);
            Debug.Log("[GameManager] Showing Credits.");
        }

        public void DisplayScenarioResults()
        {
            _scenarioCompleteCanvas.SetActive(true);
            Debug.Log("[GameManager] Showing scenario results.");
        }

        public void QuitGame()
        {
            Debug.Log("[GameManager] Quitting Game.");
            Application.Quit();
        }

        #endregion

        #region PRIVATE METHODS

        private void ShowMainMenu()
        {
            _mainMenuCanvas.SetActive(true);
            _gameplayCanvas.SetActive(false);
        }

        private void StartGameplay()
        {
            _mainMenuCanvas.SetActive(false);
            _gameplayCanvas.SetActive(true);
        }

        #endregion

        #region COROUTINES

        private IEnumerator SceneTransition()
        {
            yield return new WaitForSeconds(_sceneTransitionDelay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            SetState(GameState.Playing);
        }

        #endregion
    }

    public enum GameState
    {
        MainMenu,
        Playing,
        Transitioning,
        Paused,
        Questionnaire,
        Credits,
        GameOver
    }
}
