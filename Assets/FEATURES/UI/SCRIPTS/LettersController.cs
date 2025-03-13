using System.Collections;
using TMPro;
using UnityEngine;

namespace Amused.XR
{
    /// <summary>
    /// Controls letter-by-letter text display, syncing with audio duration when available.
    /// </summary>
    public class LettersController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshPro textComponent; // The TMP component displaying text

        [Header("Speed Settings")]
        [SerializeField, Tooltip("Multiplier for normal audio-driven text speed")]
        private float audioSpeedMultiplier = 1.0f;

        [SerializeField, Tooltip("Multiplier for estimated text speed (when no audio)")]
        private float estimatedSpeedMultiplier = 1.2f;

        private Coroutine typingCoroutine;
        private System.Action onTextComplete;

        /// <summary>
        /// Starts displaying text letter by letter, syncing speed to audio duration when available.
        /// </summary>
        public void StartDisplayingText(string text, float audioDuration, bool isEstimated, System.Action onComplete = null)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            ClearText();
            onTextComplete = onComplete;

            // Apply the correct speed multiplier based on whether the duration is estimated
            float letterDelay = GetLetterDelay(text, audioDuration, isEstimated);

            Debug.Log($"[LettersController] Displaying text at {letterDelay:F3} sec per letter (Multiplier: {(isEstimated ? estimatedSpeedMultiplier : audioSpeedMultiplier)}).");

            typingCoroutine = StartCoroutine(TypeText(text, letterDelay));
        }

        /// <summary>
        /// Clears the text display before typing starts.
        /// </summary>
        private void ClearText()
        {
            if (textComponent != null)
            {
                textComponent.text = "";
            }
        }

        /// <summary>
        /// Calculates the delay per letter based on the audio duration or fallback to default speed.
        /// </summary>
        private float GetLetterDelay(string text, float audioDuration, bool isEstimated)
        {
            float baseDelay = (audioDuration > 0) ? audioDuration / text.Length : 1f / 12f; // Default: ~12 characters per second

            return isEstimated ? baseDelay * estimatedSpeedMultiplier : baseDelay * audioSpeedMultiplier;
        }

        /// <summary>
        /// Displays text letter by letter.
        /// </summary>
        private IEnumerator TypeText(string text, float delay)
        {
            foreach (char letter in text)
            {
                textComponent.text += letter;
                yield return new WaitForSeconds(delay);
            }

            OnTextCompleted();
        }

        /// <summary>
        /// Called when text display is finished.
        /// </summary>
        private void OnTextCompleted()
        {
            Debug.Log("[LettersController] Text display completed. Notifying NPCInstructorController.");
            onTextComplete?.Invoke(); // Ensure the callback is fired
        }

    }
}
