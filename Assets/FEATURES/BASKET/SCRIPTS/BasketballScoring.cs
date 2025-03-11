using UnityEngine;
using UnityEngine.Events;
using TMPro;
using starskyproductions.playground.stat;

namespace starskyproductions.playground.scoring
{
    public class BasketballScoringSystem : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _scoreText; // Score display TMP object
        [SerializeField] private GameLogicManager gameLogicManager; // Reference to GameLogicManager
        [SerializeField] private LeaderboardManager leaderboardManager;
        [SerializeField] private StatisticsManager statisticsManager;

        public UnityEvent<int> OnScoreUpdated;

        private int _currentScore;
        private int _lastZoneEntered = 3; // Default to Zone 3 (largest zone)
        private bool _scoreExited; // Tracks if ball exited "score_collider"

        private void Awake()
        {
            // Ensure LeaderboardManager is assigned
            if (leaderboardManager == null)
            {
                leaderboardManager = FindObjectOfType<LeaderboardManager>();
                if (leaderboardManager == null)
                {
                    Debug.LogError("[BasketballScoringSystem] LeaderboardManager not found in the scene!");
                }
            }

            if (statisticsManager == null)
            {
                statisticsManager = FindObjectOfType<StatisticsManager>();
                if (statisticsManager == null)
                {
                    Debug.LogError("[BasketballScoringSystem] StatisticsManager not found in the scene!");
                }
            }

            // Subscribe to relevant events
            ZoneDetector.OnPlayerEnterZone += HandlePlayerEnterZone;
            HoopColliderDetector.OnBallHoopEvent += HandleBallHoopEvent;

            // Initialise score update event
            OnScoreUpdated ??= new UnityEvent<int>();
            OnScoreUpdated.AddListener(UpdateScoreDisplay);
        }

        private void OnDestroy()
        {
            ZoneDetector.OnPlayerEnterZone -= HandlePlayerEnterZone;
            HoopColliderDetector.OnBallHoopEvent -= HandleBallHoopEvent;
        }

        private void HandlePlayerEnterZone(int zoneScore)
        {
            _lastZoneEntered = zoneScore;
            Debug.Log($"[BasketballScoringSystem] Last zone entered updated to: Zone {_lastZoneEntered}");
        }

        private void HandleBallHoopEvent(string colliderName, bool isExiting)
        {
            if (colliderName == "score_collider" && !isExiting)
            {
                Debug.Log("[BasketballScoringSystem] Ball entered score_collider.");
                _scoreExited = false;
            }
            else if (colliderName == "score_collider" && isExiting)
            {
                Debug.Log("[BasketballScoringSystem] Ball exited score_collider.");
                _scoreExited = true;
            }
            else if (colliderName == "cheat_collider" && isExiting && _scoreExited)
            {
                Debug.Log("[BasketballScoringSystem] Valid scoring sequence. Awarding points.");
                AwardPoints();
                _scoreExited = false;
            }
        }

        private void AwardPoints()
        {
            if (gameLogicManager != null && !gameLogicManager.IsScoreEnabled())
            {
                Debug.Log("[BasketballScoringSystem] Scoring is currently disabled.");
                return;
            }

            Debug.Log($"[BasketballScoringSystem] Awarding points based on last entered zone: {_lastZoneEntered}");
            AddScore(_lastZoneEntered);

            // Notify StatisticsManager about the shot
            if (statisticsManager != null)
            {
                statisticsManager.IncrementShots();
            }
        }

        public void AddScore(int points)
        {
            _currentScore += points;
            Debug.Log($"[BasketballScoringSystem] Points added: {points}, Total score: {_currentScore}");
            OnScoreUpdated.Invoke(_currentScore);

            // Notify the GameLogicManager to update the score display
            if (gameLogicManager != null)
            {
                gameLogicManager.UpdateScoreDisplay(_currentScore);
                Debug.Log("[BasketballScoringSystem] GameLogicManager score display updated.");
            }
        }

        public void SubmitScoreToLeaderboard()
        {
            if (leaderboardManager != null)
            {
                leaderboardManager.AddScore(_currentScore);
                Debug.Log($"[BasketballScoringSystem] Final score submitted to LeaderboardManager: {_currentScore}");
            }
        }

        private void UpdateScoreDisplay(int newScore)
        {
            if (gameLogicManager != null)
            {
                gameLogicManager.UpdateScoreDisplay(newScore); // Update score on the message screen (gameMessageTMP)
                Debug.Log($"[BasketballScoringSystem] Score updated on GameLogicManager: {newScore}");
            }
        }

        public void ResetScore()
        {
            _currentScore = 0;
            OnScoreUpdated.Invoke(_currentScore);
            Debug.Log("[BasketballScoringSystem] Score has been reset.");

            if (_scoreText != null)
            {
                _scoreText.text = "0";
            }
        }

        public int GetCurrentScore()
        {
            return _currentScore;
        }

        public void IncrementShots()
        {
            if (statisticsManager != null)
            {
                statisticsManager.IncrementShots(); // Notify StatisticsManager
                Debug.Log("[BasketballScoringSystem] IncrementShots called in StatisticsManager.");
            }
            else
            {
                Debug.LogError("[BasketballScoringSystem] StatisticsManager is not assigned!");
            }
        }

        public void ResetGame()
        {
            if (statisticsManager != null)
            {
                statisticsManager.ResetStatistics(); // Reset statistics on game reset
                Debug.Log("[BasketballScoringSystem] ResetStatistics called in StatisticsManager.");
            }
            else
            {
                Debug.LogError("[BasketballScoringSystem] StatisticsManager is not assigned!");
            }
        }

    }
}
