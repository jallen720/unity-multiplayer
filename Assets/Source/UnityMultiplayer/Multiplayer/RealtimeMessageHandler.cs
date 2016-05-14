using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityMultiplayer {
    public class RealtimeMessageHandler : IRealtimeMessageListener {
        private delegate void MessageRouter(
            bool isReliable,
            string participantID,
            byte[] message);

        private readonly Dictionary<MessageTypes, MessageRouter> messageTypeRouters;
        private readonly List<byte> validMessageTypes;

        public Dictionary<string, List<IMessageListener>> MessageListeners {
            get;
            private set;
        }

        public RealtimeMessageHandler() {
            messageTypeRouters = new Dictionary<MessageTypes, MessageRouter> {
                { MessageTypes.BallPosition    , RouteBallPositionMessage   },
                { MessageTypes.PaddlePosition , RoutePaddlePositionMessage },
            };

            validMessageTypes = new List<byte>();
            MessageListeners = new Dictionary<string, List<IMessageListener>>();
            InitValidMessageTypes();
        }

        private void InitValidMessageTypes() {
            validMessageTypes.AddRange(Enum.GetValues(typeof(MessageTypes)).Cast<byte>());
        }

        void IRealtimeMessageListener.OnReceivedRealtimeMessage(
            bool isReliable,
            string participantID,
            byte[] message)
        {
            MessageUtil.ValidateMessage(message, validMessageTypes);
            messageTypeRouters[GetMessageType(message)](isReliable, participantID, message);
        }

        private MessageTypes GetMessageType(byte[] message) {
            return (MessageTypes)MessageUtil.GetMessageType(message);
        }

        private void RouteBallPositionMessage(bool _, string __, byte[] message) {

        }

        private void RoutePaddlePositionMessage(bool _, string participantID, byte[] message) {
            if (MessageListeners.ContainsKey(participantID)) {
                TriggerMessageListeners(participantID, message);
            }
        }

        private void TriggerMessageListeners(string participantID, byte[] message) {
            foreach (IMessageListener messageListener in MessageListeners[participantID]) {
                messageListener.OnReceivedMessage(message);
            }
        }

        public void AddMessageListener(string participantID, IMessageListener messageListener) {
            if (MessageListeners.ContainsKey(participantID)) {
                MessageListeners[participantID].Add(messageListener);
            }
            else {
                MessageListeners.Add(
                    participantID,
                    new List<IMessageListener> { messageListener });
            }
        }

        public void RemoveMessageListener(string participantID, IMessageListener messageListener) {
            if (MessageListeners.ContainsKey(participantID)) {
                MessageListeners[participantID].Remove(messageListener);
            }
            else {
                throw new Exception(string.Format(
                    "trying to remove message listener for participant {0} but no listeners " +
                    "exist for that participant",
                    participantID));
            }
        }
    }

    public enum MessageTypes : byte {
        PaddlePosition,
        BallPosition,
    }
}