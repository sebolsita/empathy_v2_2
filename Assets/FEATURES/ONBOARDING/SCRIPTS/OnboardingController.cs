using System.Collections;
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

        [Header("Onboarding State")]
        private int currentStep = 0;
        private bool onboardingCompleted = false;

        private void Start()
        {
            LoadOnboardingProgress();
        }

        /// <summary>
        /// Starts the onboarding sequence from step 0.
        /// </summary>
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

        /// <summary>
        /// Proceeds to the next step in the onboarding sequence.
        /// </summary>
        public void ProceedToNextStep()
        {
            Debug.Log($"[OnboardingController] Advancing to step {currentStep}");

            switch (currentStep)
            {
                case 0:
                    instructorNPC.PlayDialogue("Instructor_1"); // "Welcome to the onboarding session."
                    break;
                case 1:
                    instructorNPC.PlayDialogue("Instructor_2"); // "Here, we will teach you how to interact with the VR environment."
                    break;
                case 2:
                    instructorNPC.PlayDialogue("Instructor_3"); // "Press the trigger to proceed."
                    break;
                case 3:
                    CompleteOnboarding();
                    return;
                default:
                    Debug.LogWarning("[OnboardingController] Invalid step.");
                    return;
            }

            SaveOnboardingProgress();
            currentStep++;
        }

        /// <summary>
        /// Marks the onboarding as complete and saves progress.
        /// </summary>
        private void CompleteOnboarding()
        {
            onboardingCompleted = true;
            SaveOnboardingProgress();
            Debug.Log("[OnboardingController] Onboarding completed. Transitioning to next phase.");
        }

        /// <summary>
        /// Saves onboarding progress.
        /// </summary>
        private void SaveOnboardingProgress()
        {
            PlayerPrefs.SetInt("OnboardingStep", currentStep);
            PlayerPrefs.SetInt("OnboardingCompleted", onboardingCompleted ? 1 : 0);
            PlayerPrefs.Save();
            Debug.Log($"[OnboardingController] Saved progress: Step {currentStep}, Completed: {onboardingCompleted}");
        }

        /// <summary>
        /// Loads saved onboarding progress.
        /// </summary>
        private void LoadOnboardingProgress()
        {
            currentStep = PlayerPrefs.GetInt("OnboardingStep", 0);
            onboardingCompleted = PlayerPrefs.GetInt("OnboardingCompleted", 0) == 1;

            Debug.Log($"[OnboardingController] Loaded progress: Step {currentStep}, Completed: {onboardingCompleted}");
        }

        /// <summary>
        /// Resets onboarding progress.
        /// </summary>
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
