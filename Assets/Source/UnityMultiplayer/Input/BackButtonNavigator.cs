using UnityEngine;
using UnityEngine.SceneManagement;
using UnityUtils.InputUtils;

namespace UnityMultiplayer {

    [RequireComponent(typeof(KeyObserver))]
    public class BackButtonNavigator : MonoBehaviour {

        [SerializeField]
        private string backScene;

        private void Start() {
            GetComponent<KeyObserver>().AddKey(new KeyObserver.Key {
                code = KeyCode.Escape,
                onActiveCallback = LeaveGame
            });
        }

        private void LeaveGame() {
            SceneManager.LoadScene(backScene);
        }
    }
}