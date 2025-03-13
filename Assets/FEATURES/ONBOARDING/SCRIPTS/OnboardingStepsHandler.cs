using System.Collections.Generic;
using UnityEngine;

namespace Amused.XR
{
    /// <summary>
    /// Handles execution of each step in the onboarding process.
    /// This keeps OnboardingController clean and modular.
    /// </summary>
    public class OnboardingStepsHandler : MonoBehaviour
    {
        private NPCInstructorController instructorNPC;
        private OnboardingController onboardingController;

        /// <summary>
        /// Defines which steps should auto-progress after dialogue finishes.
        /// </summary>
        private readonly Dictionary<int, bool> autoProceedSteps = new Dictionary<int, bool>
        {
            { 0, true },  // Intro - Auto
            { 1, true },  // Text panel explanation - Auto
            { 2, true },  // Movement tutorial intro - Auto
            { 3, false }, // Requires player to move
            { 4, true },  // Teleportation info - Auto
            { 5, true },  // Hand interaction explanation - Auto
            { 6, true },  // Object interaction intro - Auto
            { 7, false }, // Requires player to grab an object
            { 8, true },  // Interaction success message - Auto
            { 9, true },  // Scenario explanation - Auto
            { 10, true }, // Interpreter explanation - Auto
            { 11, true }, // Tracking info - Auto
            { 12, false }, // Requires user to press Yes/No
            { 13, true },  // Follow me message - Auto
            { 14, false }, // Requires player to enter the waiting room
            { 15, false }, // Requires player to open the door
            { 16, false }, // Waits for scenario transition
            { 17, true }   // Restart onboarding - Auto
        };

        public void Initialize(NPCInstructorController npcController, OnboardingController controller)
        {
            instructorNPC = npcController;
            onboardingController = controller;
        }

        /// <summary>
        /// Executes the correct action based on the current onboarding step.
        /// </summary>
        public void ExecuteStep(int step)
        {
            Debug.Log($"[OnboardingStepsHandler] Executing step {step}");

            bool shouldAutoProceed = autoProceedSteps.ContainsKey(step) && autoProceedSteps[step];

            switch (step)
            {
                case 0:
                    instructorNPC.PlayDialogue("onboarding_interpreter_1a", shouldAutoProceed);
                    SaveProgress();
                    break;
                case 1:
                    instructorNPC.PlayDialogue("onboarding_interpreter_1b", shouldAutoProceed);
                    break;
                case 2:
                    instructorNPC.PlayDialogue("onboarding_interpreter_2a", shouldAutoProceed);
                    SaveProgress();
                    break;
                case 3:
                    instructorNPC.PlayDialogue("onboarding_interpreter_2b", shouldAutoProceed);
                    onboardingController.WaitForColliderTrigger(); // Activate the trigger
                    break;
                case 4:
                    instructorNPC.PlayDialogue("onboarding_interpreter_2c", shouldAutoProceed);
                    break;
                case 5:
                    instructorNPC.PlayDialogue("onboarding_interpreter_2d", shouldAutoProceed);
                    break;
                case 6:
                    instructorNPC.PlayDialogue("onboarding_interpreter_3a", shouldAutoProceed);
                    SaveProgress();
                    break;
                case 7:
                    instructorNPC.PlayDialogue("onboarding_interpreter_3b", shouldAutoProceed);
                    break;
                case 8:
                    instructorNPC.PlayDialogue("onboarding_interpreter_3c", shouldAutoProceed);
                    break;
                case 9:
                    instructorNPC.PlayDialogue("onboarding_interpreter_4a", shouldAutoProceed);
                    SaveProgress();
                    break;
                case 10:
                    instructorNPC.PlayDialogue("onboarding_interpreter_4b", shouldAutoProceed);
                    break;
                case 11:
                    instructorNPC.PlayDialogue("onboarding_interpreter_4c", shouldAutoProceed);
                    break;
                case 12:
                    instructorNPC.PlayDialogue("onboarding_interpreter_5a", shouldAutoProceed);
                    SaveProgress();
                    break;
                case 13:
                    instructorNPC.PlayDialogue("onboarding_interpreter_5b_yes", shouldAutoProceed);
                    break;
                case 14:
                    instructorNPC.PlayDialogue("onboarding_interpreter_5c_yes", shouldAutoProceed);
                    break;
                case 15:
                    instructorNPC.PlayDialogue("onboarding_interpreter_5d_yes", shouldAutoProceed);
                    break;
                case 16:
                    SaveProgress();
                    break;
                case 17:
                    instructorNPC.PlayDialogue("onboarding_interpreter_5b_no", shouldAutoProceed);
                    onboardingController.ResetOnboarding();
                    SaveProgress();
                    break;
                default:
                    Debug.LogWarning("[OnboardingStepsHandler] Invalid step number.");
                    break;
            }
        }

        /// <summary>
        /// Saves onboarding progress.
        /// </summary>
        private void SaveProgress()
        {
            Debug.LogWarning("[OnboardingStepsHandler] This pretends that the progress was saved.");
        }
    }
}
