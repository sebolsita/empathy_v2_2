using UnityEngine;

namespace Amused.XR
{
    /// <summary>
    /// Detects when the player enters the onboarding trigger area.
    /// </summary>
    public class OnboardingTrigger : MonoBehaviour
    {
        private OnboardingController onboardingController;

        private void Start()
        {
            onboardingController = FindObjectOfType<OnboardingController>();
            if (onboardingController == null)
            {
                Debug.LogError("[OnboardingTrigger] OnboardingController not found in the scene!");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("[OnboardingTrigger] Player entered the highlighted area. Proceeding to next step.");
                onboardingController.ProceedToNextStep();
                gameObject.SetActive(false); // Disable trigger after activation
            }
        }
    }
}

