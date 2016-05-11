using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMultiplayer {
    public class OpponentController : MonoBehaviour, IMessageListener {
        private Dictionary<string, List<IMessageListener>> participantMessageListeners;
        private PlayerController playerController;
        private string opponentID;
        private float positionX;

        private void Start() {
            participantMessageListeners =
                MultiplayerManager.RealtimeEventHandler.ParticipantMessageListeners;

            playerController = FindObjectOfType<PlayerController>();
            opponentID = MultiplayerManager.GetOpponent().ParticipantId;
            positionX = transform.position.x;
            Init();
        }

        private void Init() {
            if (participantMessageListeners.ContainsKey(opponentID)) {
                participantMessageListeners[opponentID].Add(this);
            }
            else {
                participantMessageListeners.Add(opponentID, new List<IMessageListener> { this });
            }
        }

        private void OnDestroy() {
            participantMessageListeners[opponentID].Remove(this);
        }

        private void Update() {
            UpdatePosition();
        }

        private void UpdatePosition() {
            Vector3 position = transform.position;
            position.x = LerpedX(position.x);
            transform.position = position;
        }

        private float LerpedX(float currentX) {
            return Mathf.Lerp(currentX, positionX, Time.deltaTime * playerController.Speed);
        }

        void IMessageListener.OnReceivedMessage(byte[] message) {
            MessageUtil.ValidateMessage(message);
            positionX = BitConverter.ToSingle(message, MessageUtil.MESSAGE_DATA_START_INDEX);
        }
    }
}