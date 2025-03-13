using UnityEngine;
using UnityEngine.SceneManagement;

namespace Amused.XR
{
    /// <summary>
    /// Controls the onboarding sequence, guiding the player through the introduction and interactions.
    /// </summary>
    public class OnboardingController : MonoBehaviour
    {
        [Header("NPC Instructor Reference")]
        [SerializeField] private NPCInstructorController instructorNPC;
        [SerializeField] private OnboardingStepsHandler stepsHandler;

        private int currentStep = 0;
        private bool onboardingCompleted = false;

        private void Start()
        {
            LoadOnboardingProgress();
            stepsHandler.Initialize(instructorNPC, this); // Pass both references
        }

        public void StartOnboarding()
        {
            if (onboardingCompleted)
            {
                Debug.Log("[OnboardingController] Onboarding already completed. Skipping...");
                return;
            }

            Debug.Log("[OnboardingController] Starting onboarding process.");
            currentStep = 0;
            ProceedToNextStep();
        }

        public void ProceedToNextStep()
        {
            Debug.Log($"[OnboardingController] Advancing to step {currentStep}");

            stepsHandler.ExecuteStep(currentStep); // Call new handler script to execute the step.

            SaveOnboardingProgress();
            currentStep++;
        }

        private void CompleteOnboarding()
        {
            onboardingCompleted = true;
            SaveOnboardingProgress();
            Debug.Log("[OnboardingController] Onboarding completed. Transitioning to next phase.");
        }

        private void SaveOnboardingProgress()
        {
            PlayerPrefs.SetInt("OnboardingStep", currentStep);
            PlayerPrefs.SetInt("OnboardingCompleted", onboardingCompleted ? 1 : 0);
            PlayerPrefs.Save();
            Debug.Log($"[OnboardingController] Saved progress: Step {currentStep}, Completed: {onboardingCompleted}");
        }

        private void LoadOnboardingProgress()
        {
            currentStep = PlayerPrefs.GetInt("OnboardingStep", 0);
            onboardingCompleted = PlayerPrefs.GetInt("OnboardingCompleted", 0) == 1;

            Debug.Log($"[OnboardingController] Loaded progress: Step {currentStep}, Completed: {onboardingCompleted}");
        }

        public void ResetOnboarding()
        {
            PlayerPrefs.DeleteKey("OnboardingStep");
            PlayerPrefs.DeleteKey("OnboardingCompleted");
            PlayerPrefs.Save();

            currentStep = 0;
            onboardingCompleted = false;

            Debug.Log("[OnboardingController] Onboarding reset.");
        }
    }
}

