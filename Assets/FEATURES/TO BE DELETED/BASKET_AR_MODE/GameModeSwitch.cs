using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace starskyproductions.playground.ui
{
    public class GameModeSwitcher : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI modeText;

        private void Start()
        {
            UpdateModeText();
        }

        public void LoadARScene() => SceneManager.LoadScene("AR_BASKET");
        public void LoadVRScene() => SceneManager.LoadScene("VR_BASKET_DESERT_HANGAR");
        public void QuitApplication() => Application.Quit();

        public void UpdateModeText()
        {
            if (modeText != null)
            {
                modeText.text = SceneManager.GetActiveScene().name.Contains("AR") ? "AR" : "VR";
            }
        }
    }
}
