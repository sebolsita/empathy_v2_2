using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Amused.XR
{
    /// <summary>
    /// Controls NPC Instructor, manages dialogue and animations, and ensures onboarding sequences run properly.
    /// </summary>
    public class NPCInstructorController : MonoBehaviour
    {
        [Header("Audio Settings")]
        [SerializeField] private AudioSource voiceAudioSource;  // Plays instructor voice
        [SerializeField] private AudioSource lipSyncAudioSource;  // Separate source for lip sync

        [Header("Dialogue Settings")]
        [SerializeField] private TextMeshPro dialogueText;  // Text displayed to the user
        [SerializeField] private Animator instructorAnimator;  // Controls NPC animations

        private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

        private void Start()
        {
            LoadAudioClips();
        }

        /// <summary>
        /// Loads all NPC audio files from the Resources folder and stores them in a dictionary for quick access.
        /// </summary>
        private void LoadAudioClips()
        {
            AudioClip[] clips = Resources.LoadAll<AudioClip>("FEATURES/ONBOARDING/AUDIO");
            foreach (var clip in clips)
            {
                audioClips[clip.name] = clip;
            }
            Debug.Log($"[NPCInstructorController] Loaded {audioClips.Count} audio files.");
        }

        /// <summary>
        /// Plays a line of dialogue, updating text, triggering animation, and handling voice audio.
        /// </summary>
        public void PlayDialogue(string lineKey)
        {
            Debug.Log($"[NPCInstructorController] Attempting to play line: {lineKey}");

            if (!audioClips.ContainsKey(lineKey))
            {
                Debug.LogWarning($"[NPCInstructorController] Missing audio file for line: {lineKey}");
            }

            if (dialogueText != null)
            {
                dialogueText.text = GetTextForLine(lineKey);
            }
            else
            {
                Debug.LogWarning("[NPCInstructorController] No TextMeshPro reference assigned.");
            }

            if (instructorAnimator != null)
            {
                instructorAnimator.SetTrigger("Talk");
            }
            else
            {
                Debug.LogWarning("[NPCInstructorController] No Animator assigned.");
            }

            if (audioClips.ContainsKey(lineKey) && voiceAudioSource != null)
            {
                voiceAudioSource.clip = audioClips[lineKey];
                voiceAudioSource.Play();
            }
        }

        /// <summary>
        /// Retrieves the correct text for a given dialogue line.
        /// </summary>
        private string GetTextForLine(string lineKey)
        {
            Dictionary<string, string> textLines = new Dictionary<string, string>
            {
                {"Instructor_1", "Welcome to the onboarding session."},
                {"Instructor_2", "Here, we will teach you how to interact with the VR environment."},
                {"Instructor_3", "Press the trigger to proceed."}
            };

            return textLines.ContainsKey(lineKey) ? textLines[lineKey] : "Missing text for this line.";
        }
    }
}
