using GooglePlayGames.BasicApi.Multiplayer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMultiplayer {

    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class BallController : MonoBehaviour, IMessageListener {

        [SerializeField]
        private float speed;

        [SerializeField]
        private int positionEmissionsPerSec;
        
        // Used if ball is source
        private List<byte> positionMessage;
        private IRealTimeMultiplayerClient client;

        // Used if ball is receiver
        private RealtimeMessageHandler realtimeMessageHandler;
        private string opponentID;
        private Vector3 position;

        private void Start() {
            CheckInit();
        }

        private void CheckInit() {
            if (IsSource()) {
                DebugUtil.Log("Initializing ball as source");
                InitAsSource();
            }
            else {
                DebugUtil.Log("Initializing ball as receiver");
                InitAsReceiver();
            }
        }

        private bool IsSource() {
            return MultiplayerManager.IsHost();
        }

        private void InitAsSource() {
            ValidatePositionEmissionsPerSec();
            positionMessage = new List<byte>();
            client = MultiplayerManager.Client;
            InitVelocity();
            StartCoroutine(PositionEmissionRoutine());
        }

        private void ValidatePositionEmissionsPerSec() {
            if (positionEmissionsPerSec < 1) {
                throw new Exception(string.Format(
                    "position emissions / second must be greater than 0; was {0}",
                    positionEmissionsPerSec
                ));
            }
        }

        private void InitVelocity() {
            GetComponent<Rigidbody2D>().velocity = Vector2.one * speed;
        }

        private IEnumerator PositionEmissionRoutine() {
            var positionEmissionIntervalWait = new WaitForSeconds(1f / positionEmissionsPerSec);

            while (true) {
                yield return positionEmissionIntervalWait;
                EmitPosition();
            }
        }

        private void EmitPosition() {
            MessageUtil.InitMessage(positionMessage, (byte)MessageType.BallPosition);
            positionMessage.AddRange(BitConverter.GetBytes(transform.position.x));
            positionMessage.AddRange(BitConverter.GetBytes(transform.position.y));

            client.SendMessageToAll(
                reliable: false,
                data: positionMessage.ToArray());
        }

        private void InitAsReceiver() {
            realtimeMessageHandler = MultiplayerManager.RealtimeMessageHandler;
            opponentID = MultiplayerManager.GetOpponent().ParticipantId;
            position = transform.position;
            GetComponent<CircleCollider2D>().enabled = false;

            realtimeMessageHandler.AddMessageListener(
                MessageType.BallPosition,
                opponentID,
                this);

            StartCoroutine(PositionLerpRoutine());
        }

        private void OnDestroy() {
            if (realtimeMessageHandler != null) {
                realtimeMessageHandler.RemoveMessageListener(
                    MessageType.BallPosition,
                    opponentID,
                    this);
            }
        }

        private IEnumerator PositionLerpRoutine() {
            while (true) {
                transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
                yield return null;
            }
        }

        void IMessageListener.OnReceivedMessage(byte[] message) {
            const int X_POSITION_INDEX = MessageUtil.MESSAGE_DATA_START_INDEX;
            const int Y_POSITION_INDEX = X_POSITION_INDEX + sizeof(float);

            position.x = -BitConverter.ToSingle(message, X_POSITION_INDEX);
            position.y = -BitConverter.ToSingle(message, Y_POSITION_INDEX);
        }
    }
}