using GooglePlayGames.BasicApi.Multiplayer;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityMultiplayer {

    [RequireComponent(typeof(SpriteRenderer))]
    public class PaddleColorSelector : MonoBehaviour {
        public void SetPaddleColor(Participant participant) {
            GetComponent<SpriteRenderer>().color =
                PaddleConfig.PaddleColors[GetParticipantIndex(participant)];
        }

        private int GetParticipantIndex(Participant participant) {
            var participantIDs = new List<string>(
                MultiplayerManager
                    .Client
                    .GetConnectedParticipants()
                    .Select(connectedParticipant => connectedParticipant.ParticipantId)
            );

            participantIDs.Sort();
            return participantIDs.IndexOf(participant.ParticipantId);
        }
    }
}