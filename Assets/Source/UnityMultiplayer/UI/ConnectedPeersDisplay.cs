﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using GooglePlayGames.BasicApi.Multiplayer;

namespace UnityMultiplayer {
    public class ConnectedPeersDisplay : MonoBehaviour {

        [SerializeField]
        private Text connectedPeersText;

        private RealtimeEventHandler realtimeEventHandler;
        private List<string> connectedPeers;

        private void Start() {
            realtimeEventHandler = MultiplayerManager.RealtimeEventHandler;
            connectedPeers = new List<string>();
            Init();
        }

        private void Init() {
            realtimeEventHandler.PeerConnectedEvent.Subscribe(OnPeerConnected);
            realtimeEventHandler.PeerDisconnectedEvent.Subscribe(OnPeerDisconnected);
        }

        private void OnDestroy() {
            realtimeEventHandler.PeerConnectedEvent.Unsubscribe(OnPeerConnected);
            realtimeEventHandler.PeerDisconnectedEvent.Unsubscribe(OnPeerDisconnected);
        }

        private void OnPeerConnected(string participantID) {
            connectedPeers.Add(participantID);
        }

        private void OnPeerDisconnected(string participantID) {
            connectedPeers.Remove(participantID);
        }

        private void UpdateConnectedPeersDisplay() {
            connectedPeersText.text = connectedPeers.Aggregate("", ConnectedPeerAggregator);
        }

        private string ConnectedPeerAggregator(string connectedPeers, string connectedPeer) {
            return connectedPeers += string.Format(
                "{0}\n",
                MultiplayerManager
                    .Client
                    .GetParticipant(connectedPeer)
                    .DisplayName
            );
        }

        private void Update() {
            connectedPeersText.text =
                MultiplayerManager
                    .Client
                    .GetConnectedParticipants()
                    .Aggregate("", ParticipantNameAggregator);
        }

        private string ParticipantNameAggregator(string participantNames, Participant participant) {
            return participantNames + participant.DisplayName + "\n";
        }
    }
}