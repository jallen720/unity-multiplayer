using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityMultiplayer {
    public class LobbyController : MonoBehaviour {
        private RealtimeEventHandler realtimeListener;

        private void Start() {
            realtimeListener = MultiplayerManager.RealtimeEventHandler;
            Init();
        }

        private void Init() {
            realtimeListener.RoomConnectedEvent.Subscribe(OnRoomConnected);

            MultiplayerManager.StartMatchmaking(
                minOpponents: 1,
                maxOpponents: 1,
                variant: 0
            );
        }

        private void OnDestroy() {
            realtimeListener.RoomConnectedEvent.Unsubscribe(OnRoomConnected);
        }

        private void OnRoomConnected() {
            SceneManager.LoadScene("Game");
        }
    }
}