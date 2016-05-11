using GooglePlayGames.BasicApi.Multiplayer;
using System.Collections.Generic;
using System.Linq;

namespace UnityMultiplayer {
    public class RealtimeEventHandler : RealTimeMultiplayerListener {
        public List<IRoomConnectedListener> RoomConnectedListeners {
            get;
            private set;
        }

        public List<IPeerListener> PeerListeners {
            get;
            private set;
        }

        public Dictionary<string, List<IMessageListener>> ParticipantMessageListeners {
            get;
            private set;
        }

        public RealtimeEventHandler() {
            RoomConnectedListeners = new List<IRoomConnectedListener>();
            PeerListeners = new List<IPeerListener>();
            ParticipantMessageListeners = new Dictionary<string, List<IMessageListener>>();
        }

        void RealTimeMultiplayerListener.OnRoomSetupProgress(float percent) {
            DebugUtil.Log(string.Format("Setup: {0}%", percent));
        }

        void RealTimeMultiplayerListener.OnRoomConnected(bool success) {
            if (success) {
                DebugUtil.Log("Successfully connected to the room");
                TriggerRoomConnectedListeners();
            }
            else {
                DebugUtil.Log("Failed to connect to the room");
            }
        }

        private void TriggerRoomConnectedListeners() {
            foreach (IRoomConnectedListener roomConnectedListener in RoomConnectedListeners) {
                roomConnectedListener.OnRoomConnected();
            }
        }

        void RealTimeMultiplayerListener.OnLeftRoom() {
            DebugUtil.Log("User has left the room");
        }

        void RealTimeMultiplayerListener.OnParticipantLeft(Participant participant) {
            DebugUtil.Log(string.Format(
                "{0} declined the invitation or left",
                participant.DisplayName
            ));
        }

        void RealTimeMultiplayerListener.OnPeersConnected(string[] participantIDs) {
            PeersMessage(participantIDs, "connected");
            TriggerPeersConnectedListeners(participantIDs);
        }

        void RealTimeMultiplayerListener.OnPeersDisconnected(string[] participantIDs) {
            PeersMessage(participantIDs, "disconnected");
            TriggerPeersDisconnectedListeners(participantIDs);
        }

        private void PeersMessage(string[] participantIDs, string peerStatus) {
            DebugUtil.Log(participantIDs.Aggregate(
                "the following peers " + peerStatus + ":\n",
                PeersMessageAggregator
            ));
        }

        private string PeersMessageAggregator(string message, string participantID) {
            return message + "    " + participantID + "\n";
        }

        private void TriggerPeersConnectedListeners(string[] participantIDs) {
            foreach (string participantID in participantIDs) {
                foreach (IPeerListener peerListener in PeerListeners) {
                    peerListener.OnPeerConnected(participantID);
                }
            }
        }

        private void TriggerPeersDisconnectedListeners(string[] participantIDs) {
            foreach (string participantID in participantIDs) {
                foreach (IPeerListener peerListener in PeerListeners) {
                    peerListener.OnPeerDisconnected(participantID);
                }
            }
        }

        void RealTimeMultiplayerListener.OnRealTimeMessageReceived(
            bool isReliable,
            string participantID,
            byte[] data)
        {
            CheckTriggerParticipantMessageListeners(participantID, data);
        }

        private void CheckTriggerParticipantMessageListeners(string participantID, byte[] data) {
            if (ParticipantMessageListeners.ContainsKey(participantID)) {
                TriggerParticipantMessageListeners(participantID, data);
            }
        }

        private void TriggerParticipantMessageListeners(string participantID, byte[] data) {
            foreach (IMessageListener messageListener in ParticipantMessageListeners[participantID]) {
                messageListener.OnReceivedMessage(data);
            }
        }
    }
}