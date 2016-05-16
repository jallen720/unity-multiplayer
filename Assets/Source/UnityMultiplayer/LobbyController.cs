using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityMultiplayer {
    public class LobbyController : MonoBehaviour, IRoomConnectedListener {
        private RealtimeEventHandler realtimeListener;

        private void Start() {
            realtimeListener = MultiplayerManager.RealtimeEventHandler;
            Init();
        }

        private void Init() {
            realtimeListener.RoomConnectedListeners.Add(this);
            MultiplayerManager.StartMatchmaking();
        }

        private void OnDestroy() {
            realtimeListener.RoomConnectedListeners.Remove(this);
        }

        void IRoomConnectedListener.OnRoomConnected() {
            SceneManager.LoadScene("Game");
        }
    }
}