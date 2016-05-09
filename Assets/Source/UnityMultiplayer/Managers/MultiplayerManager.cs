using GooglePlayGames;
using System;
using System.Collections.Generic;
using UnityUtils.Managers;

namespace UnityMultiplayer {
    public class MultiplayerManager : Singleton<MultiplayerManager> {
        private PlayGamesPlatform playGamesPlatform;
        private List<IAuthStateListener> authStateListeners;
        private RealtimeListener realtimeListener;

        public MultiplayerManager() {
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            playGamesPlatform = PlayGamesPlatform.Instance;
            authStateListeners = new List<IAuthStateListener>();
            realtimeListener = new RealtimeListener();
        }

        // Static members

        public static void SignInAndStartMPGame() {
            if (!IsAuthenticated()) {
                Instance.playGamesPlatform.localUser.Authenticate(HandleAuthentication);
            }
            else {
                DebugUtil.Log("You're already signed in.");
                StartMatchmaking();
            }
        }

        private static void HandleAuthentication(bool authenticatedSuccessfully) {
            if (authenticatedSuccessfully) {
                DebugUtil.Log(
                    "We're signed in! Welcome "
                    + Instance.playGamesPlatform.localUser.userName
                );

                UpdateAuthStateListeners(IsAuthenticated());
                StartMatchmaking();
            }
            else {
                DebugUtil.Log("Oh... we're not signed in.");
            }
        }

        public static void AddAuthStateListener(IAuthStateListener authStateListener) {
            Instance.authStateListeners.Add(authStateListener);
            authStateListener.UpdateAuthState(IsAuthenticated());
        }

        public static void RemoveAuthStateListener(IAuthStateListener authStateListener) {
            Instance.authStateListeners.Remove(authStateListener);
        }

        private static bool IsAuthenticated() {
            return Instance.playGamesPlatform.localUser.authenticated;
        }

        private static void UpdateAuthStateListeners(bool isAuthenticated) {
            foreach (IAuthStateListener authStateListener in Instance.authStateListeners) {
                ValidateAuthStateListener(authStateListener);
                authStateListener.UpdateAuthState(isAuthenticated);
            }
        }

        private static void ValidateAuthStateListener(IAuthStateListener authStateListener) {
            if (authStateListener == null) {
                throw new Exception("MultiplayerManager contains a null authStateListener");
            }
        }

        public static void SignOut() {
            Instance.playGamesPlatform.SignOut();
            UpdateAuthStateListeners(IsAuthenticated());
        }

        private static void StartMatchmaking() {
            Instance.playGamesPlatform.RealTime.CreateQuickGame(
                minOpponents : 1,
                maxOpponents : 1,
                variant      : 0,
                listener     : Instance.realtimeListener
            );
        }
    }
}