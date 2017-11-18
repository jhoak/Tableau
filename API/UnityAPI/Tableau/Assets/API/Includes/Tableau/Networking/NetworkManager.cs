﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Tableau.Base.Net {

    public class NetworkManager : UnityEngine.Networking.NetworkManager {

        const int maxPlayers = 2;
        static Player[] players = new Player[3];
        private int playerIndex = 0;

        //[SyncVar] ******************Can only use SyncVar in NetworkBehaviour classes.
        //public int turnPlayerId = getStartingPlayerId();
        // now: don't accept state-changing commands (i.e. moves) from client unless it's their turn

        public static int getStartingPlayerId() {
            return players[0].playerId;
        }

        public virtual int getNextPlayerId() {
            playerIndex = (playerIndex + 1) % players.Length;
            return players[playerIndex].playerId;
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

        public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController playerController) {
            Player player = playerController.gameObject.GetComponent<Player>();
            players[player.playerId] = null;
            base.OnServerRemovePlayer(conn, playerController);
        }

        public override void OnServerDisconnect(NetworkConnection conn) {
            foreach (PlayerController playerController in conn.playerControllers) {
                Player player = playerController.gameObject.GetComponent<Player>();
                players[player.playerId] = null;
            }

            base.OnServerDisconnect(conn);
        }
    }
}