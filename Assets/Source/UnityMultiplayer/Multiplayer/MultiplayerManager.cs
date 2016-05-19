using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using System.Linq;
using UnityUtils.Managers;

namespace UnityMultiplayer {
    public class MultiplayerManager : Singleton<MultiplayerManager> {
        private PlayGamesPlatform playGamesPlatform;
        private Authenticator authenticator;
        private RealtimeEventHandler realtimeEventHandler;
        private RealtimeMessageHandler realtimeMessageHandler;
        private IRealTimeMultiplayerClient client;

        public MultiplayerManager() {
            playGamesPlatform = PlayGamesPlatform.Instance;
            authenticator = new Authenticator(playGamesPlatform);
            realtimeEventHandler = new RealtimeEventHandler();
            realtimeMessageHandler = new RealtimeMessageHandler(realtimeEventHandler);
            Init();
        }

        private void Init() {
            InitPlayGamesPlatform();
            authenticator.AuthStateUpdatedEvent.Subscribe(OnAuthStateUpdated);
        }

        private void InitPlayGamesPlatform() {
            PlayGamesPlatform.DebugLogEnabled = false;
            PlayGamesPlatform.Activate();
        }

        private void OnAuthStateUpdated(bool isAuthenticated) {
            client = isAuthenticated ? playGamesPlatform.RealTime : null;
        }

        // Static members

        public static Authenticator Authenticator {
            get {
                return Instance.authenticator;
            }
        }

        public static RealtimeEventHandler RealtimeEventHandler {
            get {
                return Instance.realtimeEventHandler;
            }
        }

        public static RealtimeMessageHandler RealtimeMessageHandler {
            get {
                return Instance.realtimeMessageHandler;
            }
        }

        public static IRealTimeMultiplayerClient Client {
            get {
                return Instance.client;
            }
        }

        public static void StartMatchmaking(uint minOpponents, uint maxOpponents, uint variant) {
            Client.CreateQuickGame(
                minOpponents,
                maxOpponents,
                variant,
                RealtimeEventHandler
            );
        }

        public static Participant GetOpponent() {
            return Client.GetConnectedParticipants().Find(IsNotUser);
        }

        private static bool IsNotUser(Participant participant) {
            return participant.ParticipantId != Client.GetSelf().ParticipantId;
        }

        public static bool IsHost() {
            return Client.GetConnectedParticipants().Last().ParticipantId ==
                   Client.GetSelf().ParticipantId;
        }
    }
}