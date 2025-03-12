using UnityEngine;
using TMPro;
using starskyproductions.playground.scoring;

namespace starskyproductions.playground.ballrespawn
{
    /// <summary>
    /// Handles ball respawning and provides visual/auditory feedback.
    /// </summary>
    public class BallRespawner : MonoBehaviour
    {
        #region PUBLIC PROPERTIES
        [Header("Respawn Settings")]
        [Tooltip("TextMeshPro object to display feedback.")]
        [SerializeField] private TextMeshPro feedbackDisplay;

        [Tooltip("Time (in seconds) to display feedback messages.")]
        [SerializeField] private float feedbackDuration = 2.0f;

        [Tooltip("AudioSource to play respawn sound.")]
        [SerializeField] private AudioSource audioSource;

        [Tooltip("Sound to play when the ball is respawned.")]
        [SerializeField] private AudioClip respawnSound;

        [Tooltip("Reference to the GameLogicManager for managing game state.")]
        [SerializeField] private GameLogicManager gameLogicManager;

        [Tooltip("Reference to the BasketballScoringSystem to retrieve the current score.")]
        [SerializeField] private BasketballScoringSystem scoringSystem;
        #endregion

        #region PRIVATE FIELDS
        private Vector3 initialPosition;
        private Rigidbody ballRigidbody;
        #endregion

        #region UNITY METHODS
        private void Start()
        {
            initialPosition = transform.position;

            ballRigidbody = GetComponent<Rigidbody>();
            if (ballRigidbody == null)
            {
                Debug.LogError("Rigidbody is not assigned or found on the ball GameObject.");
            }

            if (audioSource == null)
            {
                Debug.LogError("AudioSource is not assigned or found on the ball GameObject.");
            }

            if (gameLogicManager == null)
            {
                Debug.LogError("GameLogicManager is not assigned in the inspector.");
            }

            if (scoringSystem == null)
            {
                Debug.LogError("BasketballScoringSystem is not assigned in the inspector.");
            }
        }
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Respawns the ball at its initial position.
        /// </summary>
        public void RespawnBall()
        {
            if (ballRigidbody != null)
            {
                ballRigidbody.velocity = Vector3.zero;
                ballRigidbody.angularVelocity = Vector3.zero;
                transform.position = initialPosition;

                PlayRespawnSound();
                DisplayMessage("BALL RESET", Color.green);

                Invoke(nameof(RestoreGameMessage), feedbackDuration);
                Debug.Log("Ball respawned to starting position.");
            }
            else
            {
                Debug.LogWarning("Rigidbody is missing; unable to respawn the ball.");
            }
        }
        #endregion

        #region PRIVATE METHODS
        private void PlayRespawnSound()
        {
            if (audioSource != null && respawnSound != null)
            {
                audioSource.PlayOneShot(respawnSound);
            }
        }

        private void DisplayMessage(string message, Color color)
        {
            if (feedbackDisplay != null)
            {
                feedbackDisplay.text = message;
                feedbackDisplay.color = color;
            }
        }

        private void RestoreGameMessage()
        {
            if (gameLogicManager != null && scoringSystem != null)
            {
                if (gameLogicManager.IsScoreEnabled())
                {
                    // Fetch the score from the scoring system and update the display
                    int currentScore = scoringSystem.GetCurrentScore();
                    DisplayMessage($"SCORE: {currentScore}", Color.white);
                }
                else
                {
                    // Display "TIMED GAME" if scoring is disabled
                    DisplayMessage("TIMED GAME", Color.white);
                }
            }
            else
            {
                Debug.LogError("GameLogicManager or BasketballScoringSystem is not assigned to restore the game message.");
            }
        }
        #endregion
    }
}
