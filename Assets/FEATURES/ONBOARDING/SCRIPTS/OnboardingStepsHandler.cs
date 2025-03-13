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

            switch (step)
            {
                // **Step 1: Introduction**
                case 0:
                    instructorNPC.PlayDialogue("onboarding_interpreter_1a"); // "Welcome to the Empathy Machine simulation..."
                    SaveProgress();
                    break;
                case 1:
                    instructorNPC.PlayDialogue("onboarding_interpreter_1b"); // "This is your main text panel..."
                    break;

                // **Step 2: Movement Tutorial**
                case 2:
                    instructorNPC.PlayDialogue("onboarding_interpreter_2a"); // "Move around using the left stick..."
                    SaveProgress();
                    break;
                case 3:
                    instructorNPC.PlayDialogue("onboarding_interpreter_2b"); // "Move to the highlighted area to continue."
                    //onboardingController.WaitForColliderTrigger();
                    break;
                case 4:
                    instructorNPC.PlayDialogue("onboarding_interpreter_2c"); // "Teleportation is triggered by pressing the right stick."
                    break;
                case 5:
                    instructorNPC.PlayDialogue("onboarding_interpreter_2d"); // "You don’t need to use controllers for this simulation..."
                    break;

                // **Step 3: Interaction Tutorial**
                case 6:
                    instructorNPC.PlayDialogue("onboarding_interpreter_3a"); // "Some objects in the environment can be grabbed..."
                    SaveProgress();
                    break;
                case 7:
                    instructorNPC.PlayDialogue("onboarding_interpreter_3b"); // "Try picking up the traffic cone in front of you."
                    //onboardingController.WaitForObjectGrab();
                    break;
                case 8:
                    instructorNPC.PlayDialogue("onboarding_interpreter_3c"); // "Well done! Now, let’s move on."
                    break;

                // **Step 4: Understanding the Experience**
                case 9:
                    instructorNPC.PlayDialogue("onboarding_interpreter_4a"); // "This experience consists of two scenarios..."
                    SaveProgress();
                    break;
                case 10:
                    instructorNPC.PlayDialogue("onboarding_interpreter_4b"); // "First, you will play without an interpreter."
                    break;
                case 11:
                    instructorNPC.PlayDialogue("onboarding_interpreter_4c"); // "Your progress is being tracked..."
                    break;

                // **Step 5: Ready Check**
                case 12:
                    instructorNPC.PlayDialogue("onboarding_interpreter_5a"); // "If you feel ready, press Yes. If you want to repeat, press No."
                    SaveProgress();
                    //onboardingController.WaitForPlayerInput();
                    break;
                case 13:
                    instructorNPC.PlayDialogue("onboarding_interpreter_5b_yes"); // "Please follow me."
                    //onboardingController.MoveInstructorToDoor();
                    break;
                case 14:
                    instructorNPC.PlayDialogue("onboarding_interpreter_5c_yes"); // "Okay, now please enter the waiting room..."
                    //onboardingController.EnableDoorInteraction();
                    break;
                case 15:
                    instructorNPC.PlayDialogue("onboarding_interpreter_5d_yes"); // "Just reach your hand to the door to open it."
                    //onboardingController.WaitForPlayerEnterRoom();
                    break;
                case 16:
                    // Player has entered the room
                    //onboardingController.TransitionToScenario1();
                    SaveProgress();
                    break;

                // **Restarting Onboarding**
                case 17:
                    instructorNPC.PlayDialogue("onboarding_interpreter_5b_no"); // "Restarting onboarding tutorial..."
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
            //onboardingController.SaveOnboardingProgress();
        }
    }
}
