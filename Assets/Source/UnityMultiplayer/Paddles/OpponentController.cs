using System;
using UnityEngine;

namespace UnityMultiplayer {

    [RequireComponent(typeof(PositionLerper))]
    public class OpponentController : MonoBehaviour {
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

            realtimeMessageHandler.SubscribeMessageListener(
                MessageType.PaddlePosition,
                opponentID,
                OnReceivedMessage);
        }

        private void InitPositionLerper() {
            positionLerper.XPositionGetter = () => opponentXPosition;
            positionLerper.SpeedGetter = playerController.GetSpeed;
        }

        private void OnDestroy() {
            realtimeMessageHandler.UnsubscribeMessageListener(
                MessageType.PaddlePosition,
                opponentID,
                OnReceivedMessage);
        }

        private void OnReceivedMessage(byte[] message) {
            opponentXPosition = GetMirroredOpponentXPosition(message);
        }

        private float GetMirroredOpponentXPosition(byte[] message) {
            return -BitConverter.ToSingle(message, MessageUtil.MESSAGE_DATA_START_INDEX);
        }
    }
}