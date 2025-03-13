using System.Collections.Generic;
using UnityEngine;

namespace Amused.XR
{
    /// <summary>
    /// Stores all onboarding dialogue text in a centralized dictionary.
    /// </summary>
    public static class OnboardingDialogueData
    {
        /// <summary>
        /// Dictionary storing all onboarding dialogue lines.
        /// </summary>
        private static readonly Dictionary<string, string> textLines = new Dictionary<string, string>
        {
            {"onboarding_interpreter_1a", "Welcome to the Empathy Machine simulation. I'll be your guide and interpreter during the experience."},
            {"onboarding_interpreter_1b", "This is the text panel. When an NPC speaks, the text will be displayed here above the currently speaking NPC"},

            {"onboarding_interpreter_2a", "Move around using the left stick, and rotate using the right one. Click the left stick to sprint or the right one to crouch, and if you want to stand up, press it again."},
            {"onboarding_interpreter_2b", "Move to the highlighted area to continue."},
            {"onboarding_interpreter_2c", "Instead of moving, teleportation is possible. You can enable it in the locomotion settings panel in front of you."},
            {"onboarding_interpreter_2d", "You don’t need to use controllers for this simulation. If you prefer your hands, just place the controllers down, and training info on how to use hands will be displayed above them."},

            {"onboarding_interpreter_3a", "Some objects in the environment can be grabbed using the trigger button or your hands."},
            {"onboarding_interpreter_3b", "Try picking up the traffic cone in front of you."},
            {"onboarding_interpreter_3c", "Well done! Now, let’s move on."},

            {"onboarding_interpreter_4a", "This experience consists of two scenarios."},
            {"onboarding_interpreter_4b", "First, you will play without an interpreter. Then, you will play with an interpreter to compare the experience."},
            {"onboarding_interpreter_4c", "Your progress is being tracked. You can resume from the menu if you need to take a break."},

            {"onboarding_interpreter_5a", "If you feel ready to start the simulation, press Yes. If you would like to repeat the onboarding, press No."},
            {"onboarding_interpreter_5b_yes", "Please follow me."},
            {"onboarding_interpreter_5c_yes", "Okay, now please enter the waiting room and wait to be called by the doctor."},
            {"onboarding_interpreter_5d_yes", "Just reach your hand to the door to open it."},
            {"onboarding_interpreter_5b_no", "Restarting onboarding tutorial..."}
        };

        /// <summary>
        /// Retrieves a dialogue line based on the provided key.
        /// </summary>
        public static string GetText(string lineKey)
        {
            return textLines.TryGetValue(lineKey, out var text) ? text : "[Missing text for this line.]";
        }
    }
}
