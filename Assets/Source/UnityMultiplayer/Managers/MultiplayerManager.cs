using GooglePlayGames;
using UnityUtils.Managers;

namespace UnityMultiplayer {
    public class MultiplayerManager : Singleton<MultiplayerManager> {
        private PlayGamesPlatform playGamesPlatform;
        private Authenticator authenticator;
        private Matchmaker matchmaker;

        public MultiplayerManager() {
            InitPlayGamesPlatform();
            playGamesPlatform = PlayGamesPlatform.Instance;
            authenticator = new Authenticator(playGamesPlatform);
            matchmaker = new Matchmaker(playGamesPlatform);
            Init();
        }

        private void InitPlayGamesPlatform() {
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
        }

        private void Init() {
            authenticator.AddAuthStateListener(matchmaker);
        }

        // Static members

        public static Authenticator Authenticator {
            get {
                return Instance.authenticator;
            }
        }
    }
}