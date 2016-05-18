using System;
using System.Collections.Generic;
using System.Linq;
using UnityUtils.EventUtils;

namespace UnityMultiplayer {
    public class RealtimeMessageHandler {
        private delegate void MessageRouter(
            bool isReliable,
            string participantID,
            byte[] message);

        private List<byte> validMessageTypes;
        private Dictionary<MessageType, Dictionary<string, Event<byte[]>>> MessageEvents;

        public RealtimeMessageHandler(RealtimeEventHandler realtimeEventHandler) {
            validMessageTypes = new List<byte>();

            MessageEvents =
                new Dictionary<MessageType, Dictionary<string, Event<byte[]>>> {
                    { MessageType.BallCollision  , new Dictionary<string, Event<byte[]>>() },
                    { MessageType.PaddlePosition , new Dictionary<string, Event<byte[]>>() },
                };

            InitValidMessageTypes();
            realtimeEventHandler.RealtimeMessageEvent.Subscribe(OnRealtimeMessage);
        }

        private void InitValidMessageTypes() {
            validMessageTypes.AddRange(Enum.GetValues(typeof(MessageType)).Cast<byte>());
        }

        private void OnRealtimeMessage(bool isReliable, string participantID, byte[] message) {
            MessageUtil.ValidateMessage(message, validMessageTypes);

            TriggerMessageEvents(
                GetMessageType(message),
                participantID,
                message);
        }

        private MessageType GetMessageType(byte[] message) {
            return (MessageType)MessageUtil.GetMessageType(message);
        }

        private void TriggerMessageEvents(
            MessageType messageType,
            string participantID,
            byte[] message)
        {
            GetMessageEvents(messageType, messageEvents => {
                if (messageEvents.ContainsKey(participantID)) {
                    messageEvents[participantID].Trigger(message);
                }
            });
        }

        public void SubscribeMessageListener(
            MessageType messageType,
            string participantID,
            Event<byte[]>.Listener messageListener)
        {
            GetMessageEvents(messageType, messageEvents => {
                if (!messageEvents.ContainsKey(participantID)) {
                    messageEvents.Add(participantID, new Event<byte[]>());
                }

                messageEvents[participantID].Subscribe(messageListener);
            });
        }

        private void ValidateHasEvents(MessageType messageType) {
            if (!MessageEvents.ContainsKey(messageType)) {
                throw new Exception(string.Format(
                    "Message type {0} has not been setup in {1}",
                    messageType,
                    typeof(RealtimeMessageHandler)
                ));
            }
        }

        public void UnsubscribeMessageListener(
            MessageType messageType,
            string participantID,
            Event<byte[]>.Listener messageListener)
        {
            GetMessageEvents(messageType, messageEvents => {
                if (messageEvents.ContainsKey(participantID)) {
                    messageEvents[participantID].Unsubscribe(messageListener);
                }
                else {
                    throw new Exception(string.Format(
                        "trying to remove message listener from events for participant {0} but " +
                        "no listeners exist for that participant",
                        participantID
                    ));
                }
            });
        }

        private void GetMessageEvents(
            MessageType messageType,
            Action<Dictionary<string, Event<byte[]>>> callback)
        {
            ValidateHasEvents(messageType);
            callback(MessageEvents[messageType]);
        }
    }

    public enum MessageType : byte {
        PaddlePosition,
        BallCollision,
    }
}