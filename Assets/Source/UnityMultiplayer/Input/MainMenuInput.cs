using UnityEngine;

namespace UnityMultiplayer {
    public class MainMenuInput : MonoBehaviour {
        private KeyObserver keyObserver;

        private void Start() {
            keyObserver = new KeyObserver(KeyCode.Escape, GameUtil.Quit);
        }

        private void Update() {
            keyObserver.CheckInput();
        }
    }
}