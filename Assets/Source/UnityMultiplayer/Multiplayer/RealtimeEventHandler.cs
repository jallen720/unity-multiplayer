﻿using GooglePlayGames.BasicApi.Multiplayer;
using System;
using System.Linq;
using UnityUtils.EventUtils;

namespace UnityMultiplayer {
    public class RealtimeEventHandler : RealTimeMultiplayerListener {
        public readonly Event<bool> RoomConnectedEvent;
        public readonly Event RoomLeftEvent;
        public readonly Event<string> PeerConnectedEvent;
        public readonly Event<string> PeerDisconnectedEvent;
        public readonly Event<bool, string, byte[]> RealtimeMessageEvent;

        public RealtimeEventHandler() {
            RoomConnectedEvent = new Event<bool>();
            RoomLeftEvent = new Event();
            PeerConnectedEvent = new Event<string>();
            PeerDisconnectedEvent = new Event<string>();
            RealtimeMessageEvent = new Event<bool, string, byte[]>();
        }

        void RealTimeMultiplayerListener.OnRoomSetupProgress(float percent) {
            DebugUtil.Log("Setup: " + percent + "%");
        }

        void RealTimeMultiplayerListener.OnRoomConnected(bool connectedSuccessfully) {
            DebugUtil.Log(
                connectedSuccessfully
                ? "Successfully connected to the room"
                : "Failed to connect to the room"
            );

            RoomConnectedEvent.Trigger(connectedSuccessfully);
        }

        void RealTimeMultiplayerListener.OnLeftRoom() {
            DebugUtil.Log("User has left the room");
            RoomLeftEvent.Trigger();
        }

        void RealTimeMultiplayerListener.OnParticipantLeft(Participant participant) {
            DebugUtil.Log(participant.DisplayName + " declined the invitation or left");
        }

        void RealTimeMultiplayerListener.OnPeersConnected(string[] participantIDs) {
            PeersMessage(participantIDs, "connected");
            Array.ForEach(participantIDs, PeerConnectedEvent.Trigger);
        }

        void RealTimeMultiplayerListener.OnPeersDisconnected(string[] participantIDs) {
            PeersMessage(participantIDs, "disconnected");
            Array.ForEach(participantIDs, PeerDisconnectedEvent.Trigger);
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
            string participantID,
            byte[] data)
        {
            RealtimeMessageEvent.Trigger(isReliable, participantID, data);
        }
    }
}