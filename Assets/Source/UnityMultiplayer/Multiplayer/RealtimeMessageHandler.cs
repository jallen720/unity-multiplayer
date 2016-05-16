using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityMultiplayer {
    public class RealtimeMessageHandler : IRealtimeMessageListener {
        private delegate void MessageRouter(
            bool isReliable,
            string participantID,
            byte[] message);

        private readonly List<byte> validMessageTypes;

        public Dictionary<MessageType, Dictionary<string, List<IMessageListener>>> MessageListeners {
            get;
            private set;
        }

        public RealtimeMessageHandler() {
            validMessageTypes = new List<byte>();

            MessageListeners =
                new Dictionary<MessageType, Dictionary<string, List<IMessageListener>>> {
                    { MessageType.BallPosition   , new Dictionary<string, List<IMessageListener>>() },
                    { MessageType.PaddlePosition , new Dictionary<string, List<IMessageListener>>() },
                };

            InitValidMessageTypes();
        }

        private void InitValidMessageTypes() {
            validMessageTypes.AddRange(Enum.GetValues(typeof(MessageType)).Cast<byte>());
        }

        void IRealtimeMessageListener.OnReceivedRealtimeMessage(
            bool isReliable,
            string participantID,
            byte[] message)
        {
            MessageUtil.ValidateMessage(message, validMessageTypes);

            TriggerMessageListeners(
                GetMessageType(message),
                participantID,
                message);
        }

        private MessageType GetMessageType(byte[] message) {
            return (MessageType)MessageUtil.GetMessageType(message);
        }

        private void TriggerMessageListeners(
            MessageType messageType,
            string participantID,
            byte[] message)
        {
            GetMessageListeners(messageType, messageListeners => {
                if (messageListeners.ContainsKey(participantID)) {
                    foreach (IMessageListener messageListener in messageListeners[participantID]) {
                        messageListener.OnReceivedMessage(message);
                    }
                }
            });
        }

        public void AddMessageListener(
            MessageType messageType,
            string participantID,
            IMessageListener messageListener)
        {
            GetMessageListeners(messageType, messageListeners => {
                if (messageListeners.ContainsKey(participantID)) {
                    messageListeners[participantID].Add(messageListener);
                }
                else {
                    messageListeners.Add(
                        participantID,
                        new List<IMessageListener> { messageListener });
                }
            });
        }

        private void ValidateHasListeners(MessageType messageType) {
            if (!MessageListeners.ContainsKey(messageType)) {
                throw new Exception(string.Format(
                    "Message type {0} has not been setup in {1}",
                    messageType,
                    typeof(RealtimeMessageHandler)
                ));
            }
        }

        public void RemoveMessageListener(
            MessageType messageType,
            string participantID,
            IMessageListener messageListener)
        {
            GetMessageListeners(messageType, messageListeners => {
                if (messageListeners.ContainsKey(participantID)) {
                    messageListeners[participantID].Remove(messageListener);
                }
                else {
                    throw new Exception(string.Format(
                        "trying to remove message listener for participant {0} but no listeners " +
                        "exist for that participant",
                        participantID
                    ));
                }
            });
        }

        private void GetMessageListeners(
            MessageType messageType,
            Action<Dictionary<string, List<IMessageListener>>> callback)
        {
            ValidateHasListeners(messageType);
            callback(MessageListeners[messageType]);
        }
    }

    public enum MessageType : byte {
        PaddlePosition,
        BallPosition,
    }
}