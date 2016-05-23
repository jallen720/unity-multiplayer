using UnityEngine;
using UnityEngine.SceneManagement;
using UnityUtils.InputUtils;

namespace UnityMultiplayer {

    [RequireComponent(typeof(KeyObserver))]
    public class LobbyInput : MonoBehaviour {
        private KeyObserver keyObserver;
        private RealtimeEventHandler realtimeEventHandler;

        private void Start() {
            keyObserver = GetComponent<KeyObserver>();
            realtimeEventHandler = MultiplayerManager.RealtimeEventHandler;
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
            realtimeEventHandler.RoomLeftEvent.Subscribe(ReturnToMainMenu);
            MultiplayerManager.Client.LeaveRoom();
        }

        private void ReturnToMainMenu() {
            DebugUtil.Log("Returning to main menu after room left");
            SceneManager.LoadScene("MainMenu");
        }

        private void OnDestroy() {
            realtimeEventHandler.RoomLeftEvent.Unsubscribe(ReturnToMainMenu);
        }
    }
}