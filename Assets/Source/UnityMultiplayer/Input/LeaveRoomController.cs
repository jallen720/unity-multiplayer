using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnityMultiplayer {

    [RequireComponent(typeof(KeyObserver))]
    public class LeaveRoomController : MonoBehaviour, ILeftRoomListener {

        [SerializeField]
        private string sceneToLoad;

        [SerializeField]
        private Text lobbyMessage;

        //private RealtimeListener realtimeListener;

        //private void Start() {
        //    realtimeListener = MultiplayerManager.RealtimeListener;
        //    Init();
        //}

        //private void Init() {
        //    GetComponent<KeyObserver>().AddKey(new KeyObserver.Key {
        //        code = KeyCode.Escape,
        //        onActiveCallback = LeaveRoom
        //    });

        //    realtimeListener.LeftRoomListeners.Add(this);
        //}

        //private void LeaveRoom() {
        //    lobbyMessage.text = "LEAVING ROOM";
        //    MultiplayerManager.LeaveRoom();
        //}

        //private void OnDestroy() {
        //    realtimeListener.LeftRoomListeners.Remove(this);
        //}

        void ILeftRoomListener.OnLeftRoom() {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}