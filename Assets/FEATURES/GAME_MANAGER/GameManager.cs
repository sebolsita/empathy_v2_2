using UnityEngine;
using UnityEngine.SceneManagement;

namespace Amused.XR
{
    /// <summary>
    /// Controls game flow, manages states, UI panels, onboarding, and scenario progression.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region PUBLIC PROPERTIES

        public static GameManager Instance { get; private set; }
        public GameState CurrentState { get; private set; } = GameState.MainMenu;

        #endregion

        #region SERIALIZED PRIVATE FIELDS

        [Header("Game Panels")]
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject answerPanel;
        [SerializeField] private GameObject questionnairePanel; // placeholder
        [SerializeField] private GameObject pausePanel; // placeholder
        [SerializeField] private GameObject creditsPanel; // placeholder

        [Header("Gameplay References")]
        [SerializeField] private OnboardingController onboardingController;
        [SerializeField] private float sceneTransitionDelay = 1.5f;

        #endregion

        #region PRIVATE FIELDS

        private int _currentScenarioIndex = 0;
        private bool _isPaused = false;

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
        /// Updates game state and handles corresponding actions.
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
                case GameState.Onboarding:
                    StartOnboarding();
                    break;
                case GameState.Scenario1:
                    StartScenario(1);
                    break;
                case GameState.Scenario2:
                    StartScenario(2);
                    break;
                case GameState.Survey:
                    ShowSurvey();
                    break;
                case GameState.Credits:
                    ShowCredits();
                    break;
                case GameState.Paused:
                    PauseGame();
                    break;
                case GameState.Playing:
                    ResumeGame();
                    break;
            }
        }

        public void StartOnboarding()
        {
            menuPanel.SetActive(false);
            answerPanel.SetActive(true);
            onboardingController.StartOnboarding();
            Debug.Log("[GameManager] Onboarding started.");
        }

        public void StartScenario(int scenarioNumber)
        {
            Debug.Log($"[GameManager] Scenario {scenarioNumber} started.");
            // TODO: Add actual scenario loading logic here.
        }

        public void LoadNextScenario()
        {
            _currentScenarioIndex++;
            SaveProgress();
            Debug.Log($"[GameManager] Loading scenario {_currentScenarioIndex}");
            // TODO: Implement scenario loading logic.
        }

        #endregion

        #region UI HANDLING METHODS

        private void ShowMainMenu()
        {
            menuPanel.SetActive(true);
            answerPanel.SetActive(false);
            questionnairePanel.SetActive(false);
            creditsPanel.SetActive(false);
            pausePanel.SetActive(false);
            Debug.Log("[GameManager] Main menu displayed.");
        }

        private void ShowSurvey()
        {
            questionnairePanel.SetActive(true);
            Debug.Log("[GameManager] Displaying questionnaire panel.");
        }

        private void ShowCredits()
        {
            creditsPanel.SetActive(true);
            Debug.Log("[GameManager] Credits panel displayed.");
        }

        #endregion

        #region SAVING AND LOADING

        public void SaveProgress()
        {
            PlayerPrefs.SetInt("ScenarioIndex", _currentScenarioIndex);
            PlayerPrefs.Save();
            Debug.Log($"[GameManager] Progress saved: Scenario {_currentScenarioIndex}");
        }

        public void LoadProgress()
        {
            _currentScenarioIndex = PlayerPrefs.GetInt("ScenarioIndex", 0);
            Debug.Log($"[GameManager] Progress loaded: Scenario {_currentScenarioIndex}");
        }

        public void ResetProgress()
        {
            _currentScenarioIndex = 0;
            SaveProgress();
            Debug.Log("[GameManager] Progress reset.");
        }

        #endregion

        #region PAUSE HANDLING

        public void PauseGame()
        {
            _isPaused = true;
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            Debug.Log("[GameManager] Game paused.");
        }

        public void ResumeGame()
        {
            _isPaused = false;
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
            Debug.Log("[GameManager] Game resumed.");
        }

        public void TogglePause()
        {
            if (_isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        #endregion
    }

    public enum GameState
    {
        MainMenu,
        Onboarding,
        Scenario1,
        Scenario2,
        Survey,
        Credits,
        Paused,
        Playing
    }
}
