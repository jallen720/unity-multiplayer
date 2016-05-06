using GooglePlayGames;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils.Managers;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace UnityMultiplayer {
    public class MultiplayerManager : Singleton<MultiplayerManager> {
        private PlayGamesPlatform playGamesPlatform;
        private List<IAuthStateListener> authStateListeners;

        public MultiplayerManager() {
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            playGamesPlatform = PlayGamesPlatform.Instance;
            authStateListeners = new List<IAuthStateListener>();
        }

        // Static members

        public static void SignInAndStartMPGame() {
            if (!IsAuthenticated()) {
                Instance.playGamesPlatform.localUser.Authenticate(HandleAuthentication);
            }
            else {
                Debug.Log("You're already signed in.");
                StartGame();
            }
        }
                                                  
        private static void HandleAuthentication(bool authenticatedSuccessfully) {
            if (authenticatedSuccessfully) {
                Debug.Log(
                    "We're signed in! Welcome "
                    + Instance.playGamesPlatform.localUser.userName
                );

                UpdateAuthStateListeners(IsAuthenticated());
                StartGame();
            }
            else {
                Debug.Log("Oh... we're not signed in.");
            }
        }

        private static void StartGame() {
            USceneManager.LoadScene("Game");
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
    }
}