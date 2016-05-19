using UnityEngine;
using UnityEngine.UI;

namespace UnityMultiplayer {
    public class LobbyStatus : MonoBehaviour {

        [SerializeField]
        private Text lobbyMessage;

        [SerializeField]
        private Image spinner;

        private RealtimeEventHandler realtimeEventHandler;

        private void Start() {
            realtimeEventHandler = MultiplayerManager.RealtimeEventHandler;
            Init();
        }

        private void Init() {
            realtimeEventHandler.RoomConnectedEvent.Subscribe(OnRoomConnected);
        }

        private void OnDestroy() {
            realtimeEventHandler.RoomConnectedEvent.Unsubscribe(OnRoomConnected);
        }

        private void OnRoomConnected(bool connectedSuccessfully) {
            lobbyMessage.text = GetRoomStatusMessage(connectedSuccessfully);
            DisableSpinner();
        }

        private string GetRoomStatusMessage(bool connectedSuccessfully) {
            return connectedSuccessfully
                   ? "YOUR OPPONENT IS: " + MultiplayerManager.GetOpponent().DisplayName
                   : "FAILED TO CONNECT TO ROOM";
        }

        private void DisableSpinner() {
            spinner.gameObject.SetActive(false);
        }
    }
}