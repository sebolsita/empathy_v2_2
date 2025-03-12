using UnityEngine;
using Meta.XR.MRUtilityKit;

namespace starskyproductions.playground
{
    public class ARBasketballSpawner : MonoBehaviour
    {
        [Header("Basketball Game Prefab")]
        [SerializeField, Tooltip("The basketball game prefab that includes all components (hoop, court, UI, etc.).")]
        private GameObject basketballGamePrefab;

        [Header("Placement Offsets")]
        [SerializeField, Tooltip("Height offset for placing the basketball game.")]
        private float heightOffset = 0.5f;

        [SerializeField, Tooltip("Depth offset for placing the basketball game.")]
        private float depthOffset = 0.1f;

        private GameObject _spawnedBasketballGame;

        private void Start()
        {
            Debug.Log("[ARBasketballSpawner] Starting prefab placement process...");

            // Validate prefab assignment
            if (basketballGamePrefab == null)
            {
                Debug.LogError("[ARBasketballSpawner] Basketball game prefab is not assigned. Please assign it in the Inspector.");
                return;
            }

            // Delay placement to ensure MRUK has initialised
            Invoke(nameof(SpawnBasketballGameAtKeyWall), 2f);
        }

        private void SpawnBasketballGameAtKeyWall()
        {
            Debug.Log("[ARBasketballSpawner] Attempting to detect a key wall for placement...");

            // Check MRUK instance
            if (MRUK.Instance == null)
            {
                Debug.LogError("[ARBasketballSpawner] MRUK.Instance is null. Ensure the MRUKManager is present and enabled in the scene.");
                return;
            }

            // Check current room
            var currentRoom = MRUK.Instance.GetCurrentRoom();
            if (currentRoom == null)
            {
                Debug.LogError("[ARBasketballSpawner] No room detected. Ensure spatial data has been captured and is accessible.");
                return;
            }

            Debug.Log("[ARBasketballSpawner] Room detected successfully. Proceeding to detect key wall...");

            // Detect key wall
            MRUKAnchor keyWall = currentRoom.GetKeyWall(out Vector2 wallScale);
            if (keyWall == null)
            {
                Debug.LogWarning("[ARBasketballSpawner] No key wall detected in the current room. Spawning at default position instead.");
                SpawnAtDefaultPosition();
                return;
            }

            Debug.Log($"[ARBasketballSpawner] Key wall detected at position {keyWall.transform.position}, scale: {wallScale}");

            // Calculate position and rotation
            Vector3 wallCenter = keyWall.transform.position;
            Vector3 spawnPosition = wallCenter + keyWall.transform.up * heightOffset + keyWall.transform.forward * depthOffset;
            Quaternion spawnRotation = Quaternion.LookRotation(-keyWall.transform.forward, Vector3.up);

            // Validate quaternion
            if (!IsValidQuaternion(spawnRotation))
            {
                Debug.LogError("[ARBasketballSpawner] Invalid quaternion calculated for spawn rotation. Prefab will not be spawned.");
                return;
            }

            // Spawn the prefab
            _spawnedBasketballGame = Instantiate(basketballGamePrefab, spawnPosition, spawnRotation);
            if (_spawnedBasketballGame != null)
            {
                Debug.Log($"[ARBasketballSpawner] Basketball game spawned successfully at {spawnPosition} with rotation {spawnRotation}");
            }
            else
            {
                Debug.LogError("[ARBasketballSpawner] Failed to instantiate basketball game prefab.");
            }
        }

        private void SpawnAtDefaultPosition()
        {
            Vector3 defaultPosition = new Vector3(0, 1, 0); // Example default position
            Quaternion defaultRotation = Quaternion.identity;

            Debug.Log($"[ARBasketballSpawner] Spawning prefab at default position {defaultPosition} with rotation {defaultRotation}");

            _spawnedBasketballGame = Instantiate(basketballGamePrefab, defaultPosition, defaultRotation);
            if (_spawnedBasketballGame != null)
            {
                Debug.Log("[ARBasketballSpawner] Basketball game spawned successfully at the default position.");
            }
            else
            {
                Debug.LogError("[ARBasketballSpawner] Failed to instantiate basketball game prefab at default position.");
            }
        }

        private bool IsValidQuaternion(Quaternion q)
        {
            return !float.IsNaN(q.x) && !float.IsNaN(q.y) && !float.IsNaN(q.z) && !float.IsNaN(q.w);
        }
    }
}
