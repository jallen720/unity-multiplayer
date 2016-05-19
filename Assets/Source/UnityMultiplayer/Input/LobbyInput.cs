using UnityEngine;

namespace UnityMultiplayer {

    [RequireComponent(typeof(KeyObserver))]
    public class LobbyInput : MonoBehaviour {
        private KeyObserver keyObserver;

        private void Start() {
            keyObserver = GetComponent<KeyObserver>();
            Init();
        }

        private void Init() {
            keyObserver.AddKey(new KeyObserver.Key {
                code = KeyCode.Escape,
                onActiveCallback = LeaveGame
            });
        }

        private void LeaveGame() {
            keyObserver.RemoveKey(KeyCode.Escape);
            MultiplayerManager.Client.LeaveRoom();
        }
    }
}