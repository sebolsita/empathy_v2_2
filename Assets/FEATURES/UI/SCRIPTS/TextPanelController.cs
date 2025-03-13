using UnityEngine;
using TMPro;
using Amused.XR;

/// <summary>
/// Controls positioning and text display for the NPC dialogue panel.
/// Ensures the panel follows the NPC's head and rotates to face the player properly.
/// </summary>
public class TextPanelController : MonoBehaviour
{
    #region Singleton Implementation

    public static TextPanelController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    #endregion

    [Header("References")]
    [SerializeField] private Transform panel; // Assign only the panel in the inspector
    [SerializeField] private Transform player; // Assign the player's head/camera
    [SerializeField] private LettersController lettersController; // Handles text animation

    [Header("Panel Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 0.3f, 0); // Editable offset for position adjustments
    [SerializeField] private Vector3 rotationOffset = new Vector3(0, 180, 90); // Editable offset for rotation adjustments
    [SerializeField] private float rotationSpeed = 5f; // Rotation smoothness

    private Transform npcHead; // The NPC's head to follow

    private void Start()
    {
        if (lettersController == null)
        {
            lettersController = panel.GetComponentInChildren<LettersController>();
        }

        if (lettersController == null)
        {
            Debug.LogError("[TextPanelController] LettersController component not found!");
        }
    }

    private void LateUpdate()
    {
        if (npcHead != null)
        {
            UpdatePanelPosition();
        }
    }

    /// <summary>
    /// Assigns the active speaker, ensuring the panel follows the correct NPC.
    /// </summary>
    public void SetActiveSpeaker(Transform newNpcHead)
    {
        npcHead = newNpcHead;
        Debug.Log("[TextPanelController] Active speaker set.");
    }

    /// <summary>
    /// Displays the dialogue text using LettersController with proper speed settings.
    /// </summary>
    public void DisplayTextWithTyping(string dialogueText, float audioDuration, bool isEstimated, System.Action onComplete)
    {
        if (lettersController != null)
        {
            lettersController.StartDisplayingText(dialogueText, audioDuration, isEstimated, onComplete);

            Debug.Log($"[TextPanelController] Displaying text with letters controller: {dialogueText} (Estimated: {isEstimated})");
        }
        else
        {
            Debug.LogWarning("[TextPanelController] LettersController is missing!");
        }
    }


    /// <summary>
    /// Updates the panel's position above the NPC and ensures it faces the player correctly.
    /// </summary>
    private void UpdatePanelPosition()
    {
        // Move panel relative to NPC head with configurable offset
        panel.position = npcHead.position + offset;

        // Make panel face the player, but **only rotate on the Y-axis**
        Vector3 directionToPlayer = player.position - panel.position;
        directionToPlayer.y = 0; // Prevents tilting
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Apply smooth rotation + user-defined rotation offset
        panel.rotation = Quaternion.Slerp(panel.rotation, targetRotation * Quaternion.Euler(rotationOffset), Time.deltaTime * rotationSpeed);
    }
}
