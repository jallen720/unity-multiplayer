using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityMultiplayer {

    [RequireComponent(typeof(KeyObserver))]
    public class InGameInput : MonoBehaviour {
        private void Start() {
            GetComponent<KeyObserver>().AddKey(new KeyObserver.Key {
                code = KeyCode.Escape,
                onActiveCallback = LeaveGame
            });
        }

        private void LeaveGame() {
            SceneManager.LoadScene("MainMenu");
        }
    }
}