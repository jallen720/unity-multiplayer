using System;
using UnityEngine;

namespace UnityMultiplayer {

    [RequireComponent(typeof(PositionLerper))]
    public class OpponentController : MonoBehaviour, IMessageListener {
        private RealtimeMessageHandler realtimeMessageHandler;
        private PlayerController playerController;
        private PositionLerper positionLerper;
        private string opponentID;
        private float opponentXPosition;

        private void Start() {
            realtimeMessageHandler = MultiplayerManager.RealtimeMessageHandler;
            playerController = FindObjectOfType<PlayerController>();
            positionLerper = GetComponent<PositionLerper>();
            opponentID = MultiplayerManager.GetOpponent().ParticipantId;
            opponentXPosition = transform.position.x;
            Init();
        }

        private void Init() {
            InitPositionLerper();

            realtimeMessageHandler.AddMessageListener(
                MessageType.PaddlePosition,
                opponentID,
                this);
        }

        private void InitPositionLerper() {
            positionLerper.XPositionGetter = () => opponentXPosition;
            positionLerper.SpeedGetter = playerController.GetSpeed;
        }

        private void OnDestroy() {
            realtimeMessageHandler.RemoveMessageListener(
                MessageType.PaddlePosition,
                opponentID,
                this);
        }

        void IMessageListener.OnReceivedMessage(byte[] message) {
            opponentXPosition = GetMirroredOpponentXPosition(message);
        }

        private float GetMirroredOpponentXPosition(byte[] message) {
            return -BitConverter.ToSingle(message, MessageUtil.MESSAGE_DATA_START_INDEX);
        }
    }
}