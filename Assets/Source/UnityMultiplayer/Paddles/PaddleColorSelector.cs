using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;

namespace UnityMultiplayer {

    [RequireComponent(typeof(SpriteRenderer))]
    public class PaddleColorSelector : MonoBehaviour {
        public void SetColorFor(Participant participant) {
            GetComponent<SpriteRenderer>().color =
                PaddleConfig.PaddleColors[GetParticipantIndex(participant)];
        }

        private int GetParticipantIndex(Participant participant) {
            return MultiplayerManager
                .Client
                .GetConnectedParticipants()
                .IndexOf(participant);
        }
    }
}