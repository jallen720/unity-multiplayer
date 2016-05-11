﻿using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityUtils.Managers;

namespace UnityMultiplayer {
    public class MultiplayerManager : Singleton<MultiplayerManager>, IAuthStateListener {
        private PlayGamesPlatform playGamesPlatform;
        private Authenticator authenticator;
        private RealtimeEventHandler realtimeEventHandler;
        private IRealTimeMultiplayerClient client;

        public MultiplayerManager() {
            playGamesPlatform = PlayGamesPlatform.Instance;
            authenticator = new Authenticator(playGamesPlatform);
            realtimeEventHandler = new RealtimeEventHandler();
            Init();
        }

        private void Init() {
            InitPlayGamesPlatform();
            authenticator.AuthStateListeners.Add(this);
        }

        private void InitPlayGamesPlatform() {
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
        }

        void IAuthStateListener.OnAuthStateUpdated(bool isAuthenticated) {
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

        public static IRealTimeMultiplayerClient Client {
            get {
                return Instance.client;
            }
        }

        public static void StartMatchmaking() {
            Client.CreateQuickGame(
                minOpponents: 1,
                maxOpponents: 1,
                variant: 0,
                listener: RealtimeEventHandler
            );
        }

        public static string GetUserParticipantID() {
            return Client.GetSelf().ParticipantId;
        }
    }
}