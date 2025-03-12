using UnityEngine;
using UnityEngine.SceneManagement;

namespace Amused.XR
{
    /// <summary>
    /// Manages overall game states, controls transitions between scenarios, and initiates onboarding.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region PUBLIC PROPERTIES

        public static GameManager Instance { get; private set; }
        public GameState CurrentState { get; private set; } = GameState.MainMenu;

        #endregion

        #region SERIALIZED PRIVATE FIELDS

        [Header("Controllers")]
        [SerializeField] private OnboardingController onboardingController;

        [Header("UI Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject onboardingPanel;
        [SerializeField] private GameObject gameplayPanel;
        [SerializeField] private GameObject pauseMenuPanel;

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
            SetState(GameState.MainMenu);
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Sets the game state and handles transitions accordingly.
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

                case GameState.Playing:
                    StartGameplay();
                    break;

                case GameState.Paused:
                    PauseGame();
                    break;

                case GameState.Credits:
                    ShowCredits();
                    break;
            }
        }

        /// <summary>
        /// Called from the onboarding button in Main Menu UI.
        /// </summary>
        public void OnOnboardingButtonPressed()
        {
            SetState(GameState.Onboarding);
        }

        #endregion

        #region PRIVATE METHODS

        private void ShowMainMenu()
        {
            mainMenuPanel.SetActive(true);
            onboardingPanel.SetActive(false);
            gameplayPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            Debug.Log("[GameManager] Main menu displayed.");
        }

        private void StartOnboarding()
        {
            mainMenuPanel.SetActive(false);
            onboardingPanel.SetActive(true);
            gameplayPanel.SetActive(false);

            if (onboardingController != null)
            {
                onboardingController.StartOnboarding();
                Debug.Log("[GameManager] Onboarding initialized.");
            }
            else
            {
                Debug.LogError("[GameManager] OnboardingController reference missing!");
            }
        }

        private void StartGameplay()
        {
            mainMenuPanel.SetActive(false);
            onboardingPanel.SetActive(false);
            gameplayPanel.SetActive(true);
            Debug.Log("[GameManager] Gameplay started.");
        }

        private void PauseGame()
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
            Debug.Log("[GameManager] Game paused.");
        }

        private void ShowCredits()
        {
            Debug.Log("[GameManager] Credits displayed.");
            // Implement credits panel logic here
        }

        #endregion
    }

    /// <summary>
    /// Enum representing different game states.
    /// </summary>
    public enum GameState
    {
        MainMenu,
        Onboarding,
        Playing,
        Paused,
        Credits
    }
}
