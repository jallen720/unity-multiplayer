using GooglePlayGames.BasicApi.Multiplayer;
using System;
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

        public List<IRealtimeMessageListener> RealtimeMessageListeners {
            get;
            private set;
        }

        public RealtimeEventHandler() {
            RoomConnectedListeners = new List<IRoomConnectedListener>();
            PeerListeners = new List<IPeerListener>();
            RealtimeMessageListeners = new List<IRealtimeMessageListener>();
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
            TriggerPeerListeners(participantIDs, TriggerConnectPeer);
        }

        void RealTimeMultiplayerListener.OnPeersDisconnected(string[] participantIDs) {
            PeersMessage(participantIDs, "disconnected");
            TriggerPeerListeners(participantIDs, TriggerDisconnectPeer);
        }

        private void TriggerConnectPeer(IPeerListener peerListener, string participantID) {
            peerListener.OnPeerConnected(participantID);
        }

        private void TriggerDisconnectPeer(IPeerListener peerListener, string participantID) {
            peerListener.OnPeerDisconnected(participantID);
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

        private void TriggerPeerListeners(
            string[] participantIDs,
            Action<IPeerListener, string> triggerAction)
        {
            foreach (string participantID in participantIDs) {
                foreach (IPeerListener peerListener in PeerListeners) {
                    triggerAction(peerListener, participantID);
                }
            }
        }

        void RealTimeMultiplayerListener.OnRealTimeMessageReceived(
            bool isReliable,
            string participantID,
            byte[] data)
        {
            foreach (IRealtimeMessageListener realtimeMessageListener in RealtimeMessageListeners) {
                realtimeMessageListener.OnReceivedRealtimeMessage(isReliable, participantID, data);
            }
        }
    }
}