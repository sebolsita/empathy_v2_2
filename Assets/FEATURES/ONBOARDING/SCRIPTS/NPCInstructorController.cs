using System.Collections;
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

        [Header("Auto Progression Settings")]
        [SerializeField] private float proceedDelay = 1f; // Delay before proceeding (adjustable in Inspector)
        [SerializeField] private OnboardingController onboardingController; // Assign in Inspector or find dynamically
        #endregion

        #region PRIVATE FIELDS

        private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
        private const float DefaultSpeechSpeed = 12f; // Characters per second (approximate human speech)
        private bool audioFinished = false;
        private bool textFinished = false;
        private bool shouldAutoProceed = false;



        #endregion

        #region UNITY METHODS

        private void Start()
        {
            LoadAudioClips();

            if (onboardingController == null)
            {
                onboardingController = FindObjectOfType<OnboardingController>();
                if (onboardingController == null)
                {
                    Debug.LogError("[NPCInstructorController] OnboardingController reference is missing and could not be found.");
                }
            }
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Plays a line of dialogue, updating text, triggering animation, lip-sync, and UI panel.
        /// </summary>
        public void PlayDialogue(string lineKey, bool autoProceed)
        {
            Debug.Log($"[NPCInstructorController] Attempting to play line: {lineKey}");

            // Reset progression tracking
            audioFinished = false;
            textFinished = false;
            shouldAutoProceed = autoProceed;

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
                    StartCoroutine(WaitForAudio(clipToPlay.length));
                }
                else
                {
                    Debug.LogWarning("[NPCInstructorController] Missing voice audio source.");
                    audioFinished = true; // No audio source, consider it done
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
                audioFinished = true; // No audio to wait for
            }

            // Set active speaker and display text with correct pacing
            if (TextPanelController.Instance != null)
            {
                TextPanelController.Instance.SetActiveSpeaker(npcHead);
                TextPanelController.Instance.DisplayTextWithTyping(text, audioDuration, isEstimated, OnTextFinished);
            }
            else
            {
                Debug.LogWarning("[NPCInstructorController] TextPanelController is missing!");
                textFinished = true; // No text panel, consider text done
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

            // Check for immediate transition (in case both are already done)
            CheckProceedToNextStep();
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
        /// Called when the letter-by-letter text display is finished.
        /// </summary>
        private void OnTextFinished()
        {
            textFinished = true;
            Debug.Log("[NPCInstructorController] Text display finished.");
            CheckProceedToNextStep();
        }

        /// <summary>
        /// Waits for audio to complete before allowing progression.
        /// </summary>
        private IEnumerator WaitForAudio(float duration)
        {
            yield return new WaitForSeconds(duration);
            audioFinished = true;
            Debug.Log("[NPCInstructorController] Audio playback finished.");
            CheckProceedToNextStep();
        }

        /// <summary>
        /// Checks if both text and audio are finished, then proceeds after delay.
        /// </summary>
        private void CheckProceedToNextStep()
        {
            if (!audioFinished || !textFinished) return;

            if (shouldAutoProceed)
            {
                Debug.Log($"[NPCInstructorController] Both text and audio finished. Proceeding in {proceedDelay} seconds.");
                StartCoroutine(ProceedAfterDelay());
            }
            else
            {
                Debug.Log("[NPCInstructorController] Waiting for player action to proceed.");
            }
        }

        /// <summary>
        /// Delays before calling ProceedToNextStep().
        /// </summary>
        private IEnumerator ProceedAfterDelay()
        {
            yield return new WaitForSeconds(proceedDelay);
            if (onboardingController != null)
            {
                onboardingController.ProceedToNextStep();
            }
            else
            {
                Debug.LogWarning("[NPCInstructorController] OnboardingController reference is missing.");
            }
        }

        #endregion
    }
}
