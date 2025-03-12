using UnityEngine;

/// <summary>
/// Controls positioning and text display for the NPC dialogue panel.
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
    [SerializeField] private Transform npcHead;
    [SerializeField] private TMPro.TextMeshPro textComponent;

    private void Update()
    {
        if (npcHead != null)
        {
            UpdatePanelPosition();
        }
    }

    public void SetActiveSpeaker(Transform newNpcHead)
    {
        npcHead = newNpcHead;
        Debug.Log("[TextPanelController] Active speaker set to new NPC head.");
    }

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

    private void UpdatePanelPosition()
    {
        transform.position = npcHead.position + new Vector3(0, 0.3f, 0);
        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
    }
}
