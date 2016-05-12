using System;
using UnityEngine;

namespace UnityMultiplayer {

    [RequireComponent(typeof(PositionLerper))]
    public class OpponentController : MonoBehaviour, IMessageListener {
        private RealtimeEventHandler realtimeEventHandler;
        private PlayerController playerController;
        private PositionLerper positionLerper;
        private string opponentID;
        private float opponentPositionX;

        private void Start() {
            realtimeEventHandler = MultiplayerManager.RealtimeEventHandler;
            playerController = FindObjectOfType<PlayerController>();
            positionLerper = GetComponent<PositionLerper>();
            opponentID = MultiplayerManager.GetOpponent().ParticipantId;
            opponentPositionX = transform.position.x;
            Init();
        }

        private void Init() {
            positionLerper.PositionXGetter = () => opponentPositionX;
            positionLerper.SpeedGetter = playerController.GetSpeed;
            realtimeEventHandler.AddParticipantMessageListener(opponentID, this);
        }

        private void OnDestroy() {
            realtimeEventHandler.RemoveParticipantMessageListener(opponentID, this);
        }

        void IMessageListener.OnReceivedMessage(byte[] message) {
            MessageUtil.ValidateMessage(message);
            opponentPositionX = BitConverter.ToSingle(message, MessageUtil.MESSAGE_DATA_START_INDEX);
        }
    }
}