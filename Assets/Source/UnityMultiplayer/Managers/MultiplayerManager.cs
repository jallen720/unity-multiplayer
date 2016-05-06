using GooglePlayGames;
using UnityEngine;
using UnityUtils.Managers;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace UnityMultiplayer.Managers {
    public class MultiplayerManager : Singleton<MultiplayerManager> {
        private PlayGamesPlatform playGamesPlatform;

        public MultiplayerManager() {
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            playGamesPlatform = PlayGamesPlatform.Instance;
        }

        // Static members

        public static void SignInAndStartMPGame() {
            if (!Instance.playGamesPlatform.localUser.authenticated) {
                Instance.playGamesPlatform.localUser.Authenticate(HandleAuthentication);
            }
            else {
                Debug.Log("You're already signed in.");
                StartGame();
            }
        }

        private static void HandleAuthentication(bool authenticatedSuccessfully) {
            if (authenticatedSuccessfully) {
                Debug.Log("We're signed in! Welcome " + Instance.playGamesPlatform.localUser.userName);
                StartGame();
            }
            else {
                Debug.Log("Oh... we're not signed in.");
            }
        }

        private static void StartGame() {
            USceneManager.LoadScene("Game");
        }
    }
}