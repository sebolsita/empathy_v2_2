using System.Collections;
using UnityEngine;
/*using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
*/
namespace Amused.Interactions
{
    public class SimpleDoorTouch : MonoBehaviour
    {
        #region PUBLIC PROPERTIES
        public Transform door;
        public float openAngle = -120f;
        public float closedAngle = 0f;
        public float rotationSpeed = 3f;

        [Header("Audio")]
        public AudioSource audioSource;
        public AudioClip openSound;
        public AudioClip closeSound;
        public float closeSoundOffset = -1f; // Offset in seconds before the door finishes moving

        [Header("Haptic Feedback")]
        public float hapticIntensity = 0.5f;
        public float hapticDuration = 0.1f;
        #endregion

        #region PRIVATE VARIABLES
        private bool isOpen = false;
        private bool isMoving = false;
        private Coroutine rotateCoroutine;
        #endregion

        #region UNITY METHODS
        private void OnTriggerEnter(Collider other)
        {
            if (isMoving) return;

            if (other.CompareTag("PlayerHand"))
            {
                //TriggerHapticFeedback(other);

                string side = GetInteractionSide(other.transform.position);
                ToggleDoor(side);
            }
        }
        #endregion

        #region PRIVATE METHODS
        private void ToggleDoor(string side)
        {
            if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);

            float targetAngle = isOpen ? closedAngle : openAngle;

            if (!isOpen && side == "Outside")
            {
                targetAngle = -openAngle;
            }

            rotateCoroutine = StartCoroutine(RotateDoor(targetAngle));
            PlayDoorSound(isOpen);
            isOpen = !isOpen;
        }

        private string GetInteractionSide(Vector3 touchPoint)
        {
            Vector3 doorToTouch = touchPoint - door.position;
            float dot = Vector3.Dot(doorToTouch, door.forward);
            return dot > 0 ? "Outside" : "Inside";
        }

        private void PlayDoorSound(bool opening)
        {
            if (audioSource == null) return;

            audioSource.clip = opening ? openSound : closeSound;
            audioSource.Play();
        }

/*        private void TriggerHapticFeedback(Collider other)
        {
            XRDirectInteractor interactor = other.GetComponentInParent<XRDirectInteractor>();

            if (interactor != null)
            {
                var hapticImpulse = interactor.GetComponent<HapticImpulsePlayer>();
                if (hapticImpulse != null)
                {
                    hapticImpulse.SendHapticImpulse(hapticIntensity, hapticDuration);
                }
            }
        }*/

        #endregion

        #region COROUTINES
        private IEnumerator RotateDoor(float targetAngle)
        {
            isMoving = true;
            float startAngle = door.localEulerAngles.y;
            float elapsedTime = 0f;
            bool closeSoundPlayed = false;

            while (elapsedTime < 1f)
            {
                float angle = Mathf.LerpAngle(startAngle, targetAngle, elapsedTime);
                door.localEulerAngles = new Vector3(0, angle, 0);
                elapsedTime += Time.deltaTime * rotationSpeed;

                if (!isOpen && !closeSoundPlayed && elapsedTime >= (1f + closeSoundOffset))
                {
                    PlayDoorSound(false);
                    closeSoundPlayed = true;
                }

                yield return null;
            }

            door.localEulerAngles = new Vector3(0, targetAngle, 0);
            isMoving = false;
        }
        #endregion
    }
}
