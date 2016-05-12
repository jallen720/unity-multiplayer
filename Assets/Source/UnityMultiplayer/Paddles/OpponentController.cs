using System;
using UnityEngine;

namespace UnityMultiplayer {

    [RequireComponent(typeof(PositionLerper))]
    public class OpponentController : MonoBehaviour, IMessageListener {
        private RealtimeEventHandler realtimeEventHandler;
        private PlayerController playerController;
        private PositionLerper positionLerper;
        private string opponentID;
        private float opponentXPosition;

        private void Start() {
            realtimeEventHandler = MultiplayerManager.RealtimeEventHandler;
            playerController = FindObjectOfType<PlayerController>();
            positionLerper = GetComponent<PositionLerper>();
            opponentID = MultiplayerManager.GetOpponent().ParticipantId;
            opponentXPosition = transform.position.x;
            Init();
        }

        private void Init() {
            positionLerper.XPositionGetter = () => opponentXPosition;
            positionLerper.SpeedGetter = playerController.GetSpeed;
            realtimeEventHandler.AddParticipantMessageListener(opponentID, this);
        }

        private void OnDestroy() {
            realtimeEventHandler.RemoveParticipantMessageListener(opponentID, this);
        }

        void IMessageListener.OnReceivedMessage(byte[] message) {
            MessageUtil.ValidateMessage(message);
            opponentXPosition = GetMirroredOpponentXPosition(message);
        }

        private float GetMirroredOpponentXPosition(byte[] message) {
            return -BitConverter.ToSingle(message, MessageUtil.MESSAGE_DATA_START_INDEX);
        }
    }
}