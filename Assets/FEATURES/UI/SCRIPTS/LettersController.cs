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

        [Header("Settings")]
        [SerializeField] private float defaultLetterDelay = 0.08f; // Default delay per letter if no audio is provided

        private Coroutine typingCoroutine; // Holds reference to the typing coroutine
        private System.Action onTextComplete; // Event callback when text is fully displayed

        /// <summary>
        /// Starts displaying text letter by letter.
        /// </summary>
        public void StartDisplayingText(string text, float audioDuration = -1f, System.Action onComplete = null)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            ClearText();
            onTextComplete = onComplete;
            float letterDelay = GetLetterDelay(text, audioDuration);
            typingCoroutine = StartCoroutine(TypeText(text, letterDelay));
        }

        /// <summary>
        /// Clears the text display.
        /// </summary>
        private void ClearText()
        {
            if (textComponent != null)
            {
                textComponent.text = "";
            }
        }

        /// <summary>
        /// Calculates the delay per letter based on the audio duration.
        /// </summary>
        private float GetLetterDelay(string text, float audioDuration)
        {
            if (audioDuration > 0)
            {
                return Mathf.Max(audioDuration / text.Length, 0.02f); // Ensure minimum delay of 0.02s
            }
            return defaultLetterDelay; // Fallback delay if no audio
        }

        /// <summary>
        /// Displays the text letter by letter.
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
            onTextComplete?.Invoke(); // Notify other scripts that text is fully displayed
        }
    }
}
