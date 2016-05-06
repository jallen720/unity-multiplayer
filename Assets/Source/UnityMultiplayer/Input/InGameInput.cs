using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityMultiplayer {
    public class InGameInput : MonoBehaviour {
        private KeyObserver keyObserver;

        private void Start() {
            keyObserver = new KeyObserver(KeyCode.Escape, LeaveGame);
        }

        private void Update() {
            keyObserver.CheckInput();
        }

        private void LeaveGame() {
            SceneManager.LoadScene("MainMenu");
        }
    }
}