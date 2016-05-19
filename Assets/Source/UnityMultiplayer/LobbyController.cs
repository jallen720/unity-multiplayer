using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityMultiplayer {
    public class LobbyController : MonoBehaviour {

        [SerializeField]
        private float loadSceneDelaySecs;

        private RealtimeEventHandler realtimeEventListener;

        private void Start() {
            realtimeEventListener = MultiplayerManager.RealtimeEventHandler;
            Init();
        }

        private void Init() {
            realtimeEventListener.RoomConnectedEvent.Subscribe(OnRoomConnected);

            MultiplayerManager.StartMatchmaking(
                minOpponents: 1,
                maxOpponents: 1,
                variant: 0
            );
        }

        private void OnDestroy() {
            realtimeEventListener.RoomConnectedEvent.Unsubscribe(OnRoomConnected);
        }

        private void OnRoomConnected(bool connectedSuccessfully) {
            StartCoroutine(LoadSceneDelayRoutine(
                connectedSuccessfully
                ? "Game"
                : "MainMenu"
            ));
        }

        private IEnumerator LoadSceneDelayRoutine(string sceneToLoad) {
            yield return new WaitForSeconds(loadSceneDelaySecs);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}