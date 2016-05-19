using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityMultiplayer {
    public class LobbyController : MonoBehaviour {
        private RealtimeEventHandler realtimeEventListener;

        private void Start() {
            realtimeEventListener = MultiplayerManager.RealtimeEventHandler;
            Init();
        }

        private void Init() {
            realtimeEventListener.RoomConnectedSuccessEvent.Subscribe(OnRoomConnected);

            MultiplayerManager.StartMatchmaking(
                minOpponents: 1,
                maxOpponents: 1,
                variant: 0
            );
        }

        private void OnDestroy() {
            realtimeEventListener.RoomConnectedSuccessEvent.Unsubscribe(OnRoomConnected);
        }

        private void OnRoomConnected() {
            SceneManager.LoadScene("Game");
        }
    }
}