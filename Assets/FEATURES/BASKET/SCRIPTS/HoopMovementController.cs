using UnityEngine;
using TMPro;

public class HoopMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The rate at which the hoop moves up or down.")]
    [SerializeField] private float movementStep = 0.5f;

    [Tooltip("The minimum height for the hoop.")]
    [SerializeField] private float minHeight = 1.0f;

    [Tooltip("The maximum height for the hoop.")]
    [SerializeField] private float maxHeight = 5.0f;

    [Tooltip("The speed at which the hoop transitions to the target height.")]
    [SerializeField] private float movementSpeed = 2.0f;

    [Header("UI Feedback")]
    [Tooltip("TextMeshPro object to display the hoop's current height.")]
    [SerializeField] private TextMeshProUGUI heightDisplay;

    private Vector3 targetPosition;

    private void Start()
    {
        // Set the initial target position to the current position
        targetPosition = transform.position;
        UpdateHeightDisplay(); // Initialize the display with the current height
    }

    private void Update()
    {
        // Smoothly move the hoop to the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

        // Continuously update the height display
        UpdateHeightDisplay();
    }

    public void MoveUp()
    {
        Vector3 newPosition = targetPosition + Vector3.up * movementStep;
        targetPosition = ClampHeight(newPosition);
    }

    public void MoveDown()
    {
        Vector3 newPosition = targetPosition - Vector3.up * movementStep;
        targetPosition = ClampHeight(newPosition);
    }

    private Vector3 ClampHeight(Vector3 position)
    {
        // Clamp the Y value to ensure the hoop stays within the allowed height range
        position.y = Mathf.Clamp(position.y, minHeight, maxHeight);
        return position;
    }

    private void UpdateHeightDisplay()
    {
        if (heightDisplay != null)
        {
            // Display the current Y position of the hoop as height
            heightDisplay.text = $"{transform.position.y:F2}";
        }
    }
}
