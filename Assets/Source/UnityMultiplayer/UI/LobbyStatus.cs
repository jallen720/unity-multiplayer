using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

namespace UnityMultiplayer {
    public class LobbyStatus : MonoBehaviour, IRoomConnectedListener {

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
            realtimeEventHandler.RoomConnectedListeners.Add(this);
        }

        private void OnDestroy() {
            realtimeEventHandler.RoomConnectedListeners.Remove(this);
        }

        void IRoomConnectedListener.OnRoomConnected() {
            ShowOpponent();
            DisableSpinner();
        }

        private void ShowOpponent() {
            lobbyMessage.text = string.Format("YOUR OPPONENT IS: {0}", GetOpponentName());
        }

        private void DisableSpinner() {
            spinner.gameObject.SetActive(false);
        }

        private string GetOpponentName() {
            return MultiplayerManager
                .Client
                .GetConnectedParticipants()
                .Find(IsNotUser)
                .DisplayName;
        }

        private bool IsNotUser(Participant participant) {
            return participant.ParticipantId != MultiplayerManager.GetUserParticipantID();
        }
    }
}