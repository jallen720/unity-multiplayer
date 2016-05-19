using GooglePlayGames.BasicApi.Multiplayer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMultiplayer {

    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class BallController : MonoBehaviour {

        // For some reason it takes 2 fixed updates for the new velocity from a collision to take
        // effect. 
        //
        // NOTE: In testing, when using a rigidbody that had gravity on instead of using a constant
        // velocity, the new velocity from the collision would happen immediately.
        private const uint FIXED_UPDATES_UNTIL_POST_COLLISION_VELOCITY = 2U;

        [SerializeField]
        private float speed;

        private new Rigidbody2D rigidbody;
        private RealtimeMessageHandler realtimeMessageHandler;
        private IRealTimeMultiplayerClient client;
        private List<byte> collisionMessage;
        private string opponentID;
        private WaitForFixedUpdate fixedUpdateWait;

        private void Start() {
            rigidbody = GetComponent<Rigidbody2D>();
            realtimeMessageHandler = MultiplayerManager.RealtimeMessageHandler;
            client = MultiplayerManager.Client;
            collisionMessage = new List<byte>();
            opponentID = MultiplayerManager.GetOpponent().ParticipantId;
            fixedUpdateWait = new WaitForFixedUpdate();
            Init();
        }

        private void Init() {
            realtimeMessageHandler.SubscribeMessageListener(
                MessageType.BallCollision,
                opponentID,
                OnReceivedCollision);

            InitVelocity();
        }

        private void OnDestroy() {
            realtimeMessageHandler.UnsubscribeMessageListener(
                MessageType.BallCollision,
                opponentID,
                OnReceivedCollision);
        }

        private void InitVelocity() {
            rigidbody.velocity = Vector2.one * GetInitVelocityDirection() * speed;
        }

        private int GetInitVelocityDirection() {
            return MultiplayerManager.IsHost() ? 1 : -1;
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            if (IsCollisionWithPlayerPaddle(collision)) {
                StartCoroutine(EmitCollisionRoutine());
            }
        }

        private bool IsCollisionWithPlayerPaddle(Collision2D collision) {
            return collision.gameObject.name == "Player";
        }


        private IEnumerator EmitCollisionRoutine() {
            yield return StartCoroutine(WaitForPostCollisionVelocityRoutine());
            EmitCollision();
        }

        private IEnumerator WaitForPostCollisionVelocityRoutine() {
            for (var i = 0U; i < FIXED_UPDATES_UNTIL_POST_COLLISION_VELOCITY; i++) {
                yield return fixedUpdateWait;
            }
        }

        private void EmitCollision() {
            MessageUtil.InitMessage(collisionMessage, (byte)MessageType.BallCollision);
            collisionMessage.AddRange(BitConverter.GetBytes(transform.position.x));
            collisionMessage.AddRange(BitConverter.GetBytes(transform.position.y));
            collisionMessage.AddRange(BitConverter.GetBytes(rigidbody.velocity.x));
            collisionMessage.AddRange(BitConverter.GetBytes(rigidbody.velocity.y));

            client.SendMessageToAll(
                reliable: false,
                data: collisionMessage.ToArray());
        }

        private void OnReceivedCollision(byte[] message) {
            const int X_POSITION_INDEX = MessageUtil.MESSAGE_DATA_START_INDEX;
            const int Y_POSITION_INDEX = X_POSITION_INDEX + sizeof(float);
            const int X_VELOCITY_INDEX = Y_POSITION_INDEX + sizeof(float);
            const int Y_VELOCITY_INDEX = X_VELOCITY_INDEX + sizeof(float);

            transform.position = new Vector3(
                -BitConverter.ToSingle(message, X_POSITION_INDEX),
                -BitConverter.ToSingle(message, Y_POSITION_INDEX)
            );

            rigidbody.velocity = new Vector3(
                -BitConverter.ToSingle(message, X_VELOCITY_INDEX),
                -BitConverter.ToSingle(message, Y_VELOCITY_INDEX)
            );
        }
    }
}