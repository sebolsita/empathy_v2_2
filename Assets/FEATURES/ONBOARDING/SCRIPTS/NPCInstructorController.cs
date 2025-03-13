using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Amused.XR
{
    /// <summary>
    /// Controls NPC Instructor, manages dialogue, animations, and ensures onboarding sequences run properly.
    /// Handles text display and panel positioning.
    /// </summary>
    public class NPCInstructorController : MonoBehaviour
    {
        #region SERIALIZED PRIVATE FIELDS

        [Header("Audio Settings")]
        [SerializeField] private AudioSource voiceAudioSource;      // Plays instructor voice
        [SerializeField] private AudioSource lipSyncAudioSource;    // Separate source for lip sync

        [Header("Dialogue Settings")]
        [SerializeField] private TextMeshPro dialogueText;          // Text displayed to the user
        [SerializeField] private Animator instructorAnimator;       // Controls NPC animations

        [Header("NPC Head Reference")]
        [SerializeField] private Transform npcHead;                 // The head position for the text panel

        #endregion

        #region PRIVATE FIELDS

        private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

        #endregion

        #region UNITY METHODS

        private void Start()
        {
            LoadAudioClips();
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Plays a line of dialogue, updating text, triggering animation, and ensuring the panel follows the NPC.
        /// </summary>
        /// <summary>
        /// Plays a line of dialogue, updating text, triggering animation, lip-sync, and panel.
        /// </summary>
        public void PlayDialogue(string lineKey)
        {
            Debug.Log($"[NPCInstructorController] Attempting to play line: {lineKey}");

            // Set active speaker and text in TextPanelController
            if (TextPanelController.Instance != null)
            {
                TextPanelController.Instance.SetActiveSpeaker(npcHead);
                TextPanelController.Instance.SetText(GetTextForLine(lineKey));
            }
            else
            {
                Debug.LogWarning("[NPCInstructorController] TextPanelController is missing!");
            }

            // Play animation
            if (instructorAnimator != null)
            {
                instructorAnimator.SetTrigger("Talk");
            }
            else
            {
                Debug.LogWarning("[NPCInstructorController] No Animator assigned.");
            }

            // Handle voice audio
            if (audioClips.ContainsKey(lineKey))
            {
                AudioClip clipToPlay = audioClips[lineKey];

                if (voiceAudioSource != null)
                {
                    voiceAudioSource.clip = clipToPlay;
                    voiceAudioSource.Play();
                }
                else
                {
                    Debug.LogWarning("[NPCInstructorController] Missing voice audio source.");
                }

                if (lipSyncAudioSource != null)
                {
                    lipSyncAudioSource.clip = clipToPlay;
                    lipSyncAudioSource.Play();
                    Debug.Log($"[NPCInstructorController] Lip sync playing for {lineKey}");
                }
                else
                {
                    Debug.LogWarning("[NPCInstructorController] Missing LipSync AudioSource.");
                }
            }
            else
            {
                Debug.LogWarning($"[NPCInstructorController] Missing audio clip for line: {lineKey}");
            }

            // Trigger animation
            if (instructorAnimator != null)
            {
                instructorAnimator.SetTrigger("Talk");
            }
        }


        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// Loads all NPC audio files from the Resources folder into a dictionary.
        /// </summary>
        private void LoadAudioClips()
        {
            AudioClip[] clips = Resources.LoadAll<AudioClip>("FEATURES/ONBOARDING/AUDIO");
            foreach (var clip in clips)
            {
                audioClips[clip.name] = clip;
            }
            Debug.Log($"[NPCInstructorController] Loaded {audioClips.Count} audio files from Resources.");
        }

        /// <summary>
        /// Retrieves dialogue text associated with the specified line key.
        /// </summary>
        private string GetTextForLine(string lineKey)
        {
            Dictionary<string, string> textLines = new Dictionary<string, string>
            {
                {"Instructor_1", "Welcome to the onboarding session."},
                {"Instructor_2", "Here, we will teach you how to interact with the VR environment."},
                {"Instructor_3", "Press the trigger to proceed."}
            };

            return textLines.TryGetValue(lineKey, out var text) ? text : "[Missing text for this line.]";
        }

        #endregion
    }
}
