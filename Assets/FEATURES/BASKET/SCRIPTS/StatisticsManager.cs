using UnityEngine;
using TMPro;

namespace starskyproductions.playground.stat
{
    /// <summary>
    /// Tracks and displays game statistics, including streaks, total shots, and average time between shots.
    /// </summary>
    public class StatisticsManager : MonoBehaviour
    {
        #region PUBLIC PROPERTIES
        [Header("Statistics Settings")]
        [Tooltip("TMP object for displaying current streak.")]
        [SerializeField] private TextMeshProUGUI streakTMP;

        [Tooltip("TMP object for displaying highest streak.")]
        [SerializeField] private TextMeshProUGUI highestStreakTMP;

        [Tooltip("TMP object for displaying total shots.")]
        [SerializeField] private TextMeshProUGUI totalShotsTMP;

        [Tooltip("TMP object for displaying average time between shots.")]
        [SerializeField] private TextMeshProUGUI avgTimeTMP;
        #endregion

        #region PRIVATE FIELDS
        private int currentStreak = 0;
        private int highestStreak = 0;
        private int totalShots = 0;
        private float totalShotTime = 0f; // Cumulative time for all shots
        private float lastShotTime = 0f; // Time of the previous shot
        #endregion

        #region UNITY METHODS
        private void Start()
        {
            Debug.Log("[StatisticsManager] Checking TMP assignments...");
            if (streakTMP == null) Debug.LogError("[StatisticsManager] streakTMP is not assigned!");
            if (highestStreakTMP == null) Debug.LogError("[StatisticsManager] highestStreakTMP is not assigned!");
            if (totalShotsTMP == null) Debug.LogError("[StatisticsManager] totalShotsTMP is not assigned!");
            if (avgTimeTMP == null) Debug.LogError("[StatisticsManager] avgTimeTMP is not assigned!");

            ResetStatistics(); // Reset stats and update displays
        }

        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Resets all statistics for a new game.
        /// </summary>
        public void ResetStatistics()
        {
            currentStreak = 0;
            highestStreak = 0;
            totalShots = 0;
            totalShotTime = 0f;
            lastShotTime = 0f;

            Debug.Log("[StatisticsManager] Statistics reset.");
            UpdateDisplays();
        }


        /// <summary>
        /// Updates the streak and checks for a new high streak.
        /// </summary>
        /// <param name="streak">The new streak value.</param>
        public void UpdateStreak(int streak)
        {
            currentStreak = streak;
            if (currentStreak > highestStreak)
            {
                highestStreak = currentStreak;
            }
            UpdateDisplays();
        }

        /// <summary>
        /// Increments the total shots count and calculates the average time between shots.
        /// </summary>
        public void IncrementShots()
        {
            totalShots++;

            float currentTime = Time.time;
            if (lastShotTime > 0)
            {
                float timeSinceLastShot = currentTime - lastShotTime;
                totalShotTime += timeSinceLastShot;
                Debug.Log($"[StatisticsManager] Time since last shot: {timeSinceLastShot:F2}s");
            }
            lastShotTime = currentTime;

            Debug.Log($"[StatisticsManager] Total shots incremented: {totalShots}");
            UpdateDisplays();
        }
        #endregion

        #region PRIVATE METHODS
        /// <summary>
        /// Updates all TMP displays with the latest statistics.
        /// </summary>
        private void UpdateDisplays()
        {
            Debug.Log("[StatisticsManager] Updating TMP displays...");

            if (streakTMP != null)
            {
                Debug.Log($"[StatisticsManager] Streak TMP updated to: {currentStreak}");
                streakTMP.text = $"{currentStreak}";
            }

            if (highestStreakTMP != null)
            {
                Debug.Log($"[StatisticsManager] Highest Streak TMP updated to: {highestStreak}");
                highestStreakTMP.text = $"{highestStreak}";
            }

            if (totalShotsTMP != null)
            {
                Debug.Log($"[StatisticsManager] Total Shots TMP updated to: {totalShots}");
                totalShotsTMP.text = $"{totalShots}";
            }

            if (avgTimeTMP != null)
            {
                float avgTime = totalShots > 1 ? totalShotTime / (totalShots - 1) : 0f;
                Debug.Log($"[StatisticsManager] Average Time TMP updated to: {avgTime:F2}s");
                avgTimeTMP.text = $"{avgTime:F2}s";
            }
        }


        #endregion
    }
}
