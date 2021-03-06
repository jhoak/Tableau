﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Tableau.Base;
using Tableau.Base.Event;
using UnityEngine.SceneManagement;

namespace Tableau.Base.Net {

    public class NetworkManager : UnityEngine.Networking.NetworkManager {

        const int maxPlayers = 2;
        static Player[] players = new Player[3];

        // todo custom attribute for server callback?
        void Start() {
            // subscribe to all the general events
            EventManager.StartListening(GeneralEvents.LoadStart, HandleLoadStart);
            EventManager.StartListening(GeneralEvents.LoadEnd, HandleLoadEnd);
            EventManager.StartListening(GeneralEvents.GameStart, HandleGameStart);
            EventManager.StartListening(GeneralEvents.GameEnd, HandleGameEnd);
            EventManager.StartListening(GeneralEvents.GameRestart, HandleGameRestart);
            EventManager.StartListening("ChangingTurn", HandleChangingTurn);
        }

        // todo custom attribute for server callback?
        void HandleLoadStart() {
            // do nothing...
        }

        // todo custom attribute for server callback?
        void HandleLoadEnd() {
            // do nothing...
        }

        // todo custom attribute for server callback?
        void HandleGameStart() {
            // do nothing...
        }

        // todo custom attribute for server callback?
        void HandleGameEnd() {
            // do nothing...
        }

        // todo custom attribute for server callback?
        void HandleGameRestart() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // todo does this need a custom attribute?
        void HandleChangingTurn() {
            // do nothing...
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
            for(int i = 1; i < maxPlayers + 1; i++) {
                if (players[i] == null) {
                    GameObject playerObj = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                    Player player = playerObj.GetComponent<Player>();
                    player.playerId = i;
                    players[i] = player;
                    NetworkServer.AddPlayerForConnection(conn, playerObj, playerControllerId);
                    return;
                }
            }
            conn.Disconnect();
        }

        public override void OnServerRemovePlayer(
            NetworkConnection conn,
            UnityEngine.Networking.PlayerController playerController
        ) {
            Player player = playerController.gameObject.GetComponent<Player>();
            players[player.playerId] = null;
            base.OnServerRemovePlayer(conn, playerController);
        }

        public override void OnServerDisconnect(NetworkConnection conn) {
            foreach (UnityEngine.Networking.PlayerController playerController in conn.playerControllers) {
                Player player = playerController.gameObject.GetComponent<Player>();
                players[player.playerId] = null;
            }

            base.OnServerDisconnect(conn);
        }
    }
}
