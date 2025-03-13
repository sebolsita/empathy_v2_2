using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Amused.XR
{
    /// <summary>
    /// Controls NPC Instructor, manages dialogue, animations, and ensures onboarding sequences run properly.
    /// Handles text display, animations, and lip-syncing.
    /// </summary>
    public class NPCInstructorController : MonoBehaviour
    {
        #region SERIALIZED PRIVATE FIELDS

        [Header("Audio Settings")]
        [SerializeField] private AudioSource voiceAudioSource;   // Plays instructor voice
        [SerializeField] private AudioSource lipSyncAudioSource; // Separate source for lip sync

        [Header("Dialogue Settings")]
        [SerializeField] private Animator instructorAnimator;    // Controls NPC animations

        [Header("NPC Head Reference")]
        [SerializeField] private Transform npcHead;              // The head position for the text panel

        #endregion

        #region PRIVATE FIELDS

        private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
        private const float DefaultSpeechSpeed = 12f; // Characters per second (approximate human speech)

        #endregion

        #region UNITY METHODS

        private void Start()
        {
            LoadAudioClips();
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Plays a line of dialogue, updating text, triggering animation, lip-sync, and UI panel.
        /// </summary>
        public void PlayDialogue(string lineKey)
        {
            Debug.Log($"[NPCInstructorController] Attempting to play line: {lineKey}");

            // Retrieve the dialogue text
            string text = OnboardingDialogueData.GetText(lineKey);
            bool isEstimated = false;
            float audioDuration = -1f;

            // Check if an audio file exists
            if (audioClips.ContainsKey(lineKey))
            {
                AudioClip clipToPlay = audioClips[lineKey];
                audioDuration = clipToPlay.length; // Use actual audio duration

                // Play voice audio
                if (voiceAudioSource != null)
                {
                    voiceAudioSource.clip = clipToPlay;
                    voiceAudioSource.Play();
                }
                else
                {
                    Debug.LogWarning("[NPCInstructorController] Missing voice audio source.");
                }

                // Play lip-sync audio
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
                // No audio found, estimate duration
                isEstimated = true;
                audioDuration = text.Length / DefaultSpeechSpeed;
                Debug.LogWarning($"[NPCInstructorController] No audio found for {lineKey}, using estimated duration: {audioDuration:F2} sec.");
            }

            // Set active speaker and display text with correct pacing
            if (TextPanelController.Instance != null)
            {
                TextPanelController.Instance.SetActiveSpeaker(npcHead);
                TextPanelController.Instance.DisplayTextWithTyping(text, audioDuration, isEstimated);
            }
            else
            {
                Debug.LogWarning("[NPCInstructorController] TextPanelController is missing!");
            }

            // Trigger NPC talking animation
            if (instructorAnimator != null)
            {
                instructorAnimator.SetTrigger("Talk");
            }
            else
            {
                Debug.LogWarning("[NPCInstructorController] No Animator assigned.");
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

        #endregion
    }
}
