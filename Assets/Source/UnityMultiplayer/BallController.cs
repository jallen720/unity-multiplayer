using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMultiplayer {

    [RequireComponent(typeof(Rigidbody2D))]
    public class BallController : MonoBehaviour {

        [SerializeField]
        private float speed;

        [SerializeField]
        private int positionEmissionsPerSec;

        private new Rigidbody2D rigidbody2D;
        private WaitForSeconds positionEmissionIntervalWait;
        private List<byte> positionMessage;

        private void Start() {
            const int POSITION_MESSAGE_SIZE = 6;

            ValidatePositionEmissionsPerSec();
            rigidbody2D = GetComponent<Rigidbody2D>();
            positionEmissionIntervalWait = new WaitForSeconds(1f / positionEmissionsPerSec);
            positionMessage = new List<byte>(POSITION_MESSAGE_SIZE);
            Init();
            StartCoroutine(PositionEmissionRoutine());
        }

        private void Init() {
            rigidbody2D.velocity = Vector2.one * speed;
        }

        private void ValidatePositionEmissionsPerSec() {
            if (positionEmissionsPerSec < 1) {
                throw new Exception(string.Format(
                    "position emissions / second must be greater than 0; was {0}",
                    positionEmissionsPerSec
                ));
            }
        }

        private IEnumerator PositionEmissionRoutine() {
            while (true) {
                yield return positionEmissionIntervalWait;
                EmitPosition();
            }
        }

        private void EmitPosition() {
            MessageUtil.InitMessage(positionMessage, (byte)MessageTypes.BallPosition);
            positionMessage.AddRange(BitConverter.GetBytes(transform.position.x));
            positionMessage.AddRange(BitConverter.GetBytes(transform.position.y));
            MultiplayerManager.Client.SendMessageToAll(false, positionMessage.ToArray());
        }
    }
}