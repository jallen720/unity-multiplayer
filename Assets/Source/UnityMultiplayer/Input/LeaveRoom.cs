using UnityEngine;

namespace UnityMultiplayer {

    [RequireComponent(typeof(KeyObserver))]
    public class LeaveRoom : MonoBehaviour {
        private void Start() {
            GetComponent<KeyObserver>().AddKey(new KeyObserver.Key {
                code = KeyCode.Escape,
                onActiveCallback = MultiplayerManager.LeaveRoom
            });
        }
    }
}