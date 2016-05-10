using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using System.Collections.Generic;
using UnityUtils.Managers;

namespace UnityMultiplayer {
    public class MultiplayerManager : Singleton<MultiplayerManager> {
        private PlayGamesPlatform playGamesPlatform;
        private Authenticator authenticator;
        private RealtimeListener realtimeListener;

        public MultiplayerManager() {
            InitPlayGamesPlatform();
            playGamesPlatform = PlayGamesPlatform.Instance;
            authenticator = new Authenticator(playGamesPlatform);
            realtimeListener = new RealtimeListener();
        }

        private void InitPlayGamesPlatform() {
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
        }

        // Static members

        public static Authenticator Authenticator {
            get {
                return Instance.authenticator;
            }
        }

        public static RealtimeListener RealtimeListener {
            get {
                return Instance.realtimeListener;
            }
        }

        public static void StartMatchmaking() {
            Instance.playGamesPlatform.RealTime.CreateQuickGame(
                minOpponents: 1,
                maxOpponents: 1,
                variant: 0,
                listener: RealtimeListener
            );
        }

        public static void LeaveRoom() {
            Instance.playGamesPlatform.RealTime.LeaveRoom();
        }

        public static List<Participant> GetConnectedParticipants() {
            return Instance.playGamesPlatform.RealTime.GetConnectedParticipants();
        }

        public static Participant GetUser() {
            return Instance.playGamesPlatform.RealTime.GetSelf();
        }
    }
}