using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

namespace UnityMultiplayer {
    public class LobbyUI : MonoBehaviour, IRoomConnectedListener {

        [SerializeField]
        private Text lobbyMessage;

        [SerializeField]
        private Image spinner;

        private RealtimeListener realtimeListener;

        private void Start() {
            realtimeListener = MultiplayerManager.RealtimeListener;
            Init();
        }

        private void Init() {
            realtimeListener.RoomConnectedListeners.Add(this);
        }

        private void OnDestroy() {
            realtimeListener.RoomConnectedListeners.Remove(this);
        }

        void IRoomConnectedListener.OnRoomConnected() {
            lobbyMessage.text = string.Format("YOUR OPPONENT IS: {0}", GetOpponentName());
            spinner.gameObject.SetActive(false);
        }

        private string GetOpponentName() {
            return MultiplayerManager
                .GetConnectedParticipants()
                .Find(IsNotUser)
                .DisplayName;
        }

        private bool IsNotUser(Participant participant) {
            return participant.ParticipantId != MultiplayerManager.GetUser().ParticipantId;
        }
    }
}