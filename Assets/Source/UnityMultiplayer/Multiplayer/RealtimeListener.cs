using GooglePlayGames.BasicApi.Multiplayer;
using System.Collections.Generic;
using System.Linq;

namespace UnityMultiplayer {
    public class RealtimeListener : RealTimeMultiplayerListener {
        public List<IRoomConnectedListener> RoomConnectedListeners {
            get;
            private set;
        }

        public List<ILeftRoomListener> LeftRoomListeners {
            get;
            private set;
        }

        public RealtimeListener() {
            RoomConnectedListeners = new List<IRoomConnectedListener>();
            LeftRoomListeners = new List<ILeftRoomListener>();
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
            DebugUtil.Log("Left the room");
            //TriggerLeftRoomListeners();
        }

        //private void TriggerLeftRoomListeners() {
        //    foreach (ILeftRoomListener leftRoomListener in LeftRoomListeners) {
        //        leftRoomListener.OnLeftRoom();
        //    }
        //}

        void RealTimeMultiplayerListener.OnParticipantLeft(Participant participant) {
            DebugUtil.Log(string.Format("{0} left the room", participant.DisplayName));
        }

        void RealTimeMultiplayerListener.OnPeersConnected(string[] participantIds) {
            PeersMessage(participantIds, "connected");
        }

        void RealTimeMultiplayerListener.OnPeersDisconnected(string[] participantIds) {
            PeersMessage(participantIds, "disconnected");
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

        void RealTimeMultiplayerListener.OnRealTimeMessageReceived(
            bool isReliable,
            string senderId,
            byte[] data)
        {
            DebugUtil.Log(string.Format(
                "Realtime message received:\n" +
                "    is reliable: {0}\n" +
                "    sender ID: {1}\n" +
                "    data: {2}\n",
                isReliable,
                senderId,
                data
            ));
        }
    }
}