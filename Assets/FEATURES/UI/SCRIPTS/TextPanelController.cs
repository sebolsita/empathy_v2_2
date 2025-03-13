using UnityEngine;
using TMPro;

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
    private TextMeshPro textComponent;

    [Header("Panel Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 0.3f, 0); // Editable offset for position adjustments
    [SerializeField] private Vector3 rotationOffset = new Vector3(0, 180, 90); // Editable offset for rotation adjustments
    [SerializeField] private float rotationSpeed = 5f; // Rotation smoothness

    private Transform npcHead; // The NPC's head to follow

    private void Start()
    {
        // Automatically find the TextMeshPro component inside the panel
        textComponent = panel.GetComponentInChildren<TextMeshPro>();

        if (textComponent == null)
        {
            Debug.LogError("[TextPanelController] TextMeshPro component not found!");
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
    /// Updates the displayed text.
    /// </summary>
    public void SetText(string dialogueText)
    {
        if (textComponent != null)
        {
            textComponent.text = dialogueText;
            Debug.Log($"[TextPanelController] Updated panel text: {dialogueText}");
        }
        else
        {
            Debug.LogWarning("[TextPanelController] Text component reference is missing!");
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
