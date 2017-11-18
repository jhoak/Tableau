using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Tableau.Base;
using Tableau.Base.Event;

namespace Tableau.Base.Net {

    public class NetworkManager : UnityEngine.Networking.NetworkManager {

        const int maxPlayers = 2;
        Player[] players = new Player[3];
        private int playerIndex = 0;

        [SyncVar]
        public int turnPlayerId = getStartingPlayerId();
        // now: don't accept state-changing commands (i.e. moves) from client unless it's their turn

        // todo custom attribute for server callback?
        void Start() {
            // subscribe to all the general events
            EventManager em = EventManager.instance;
            em.StartListening(GeneralEvents.LoadStart, HandleLoadStart);
            em.StartListening(GeneralEvents.LoadEnd, HandleLoadEnd);
            em.StartListening(GeneralEvents.GameStart, HandleGameStart);
            em.StartListening(GeneralEvents.GameEnd, HandleGameEnd);
            em.StartListening(GeneralEvents.GameRestart, HandleGameRestart);
            em.StartListening("ChangingTurn", HandleChangingTurn);
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
            Application.LoadLevel(Application.loadedLevel);
        }

        // todo does this need a custom attribute?
        void HandleChangingTurn() {
            turnPlayerId = getNextPlayerId();
        }

        public virtual int getStartingPlayerId() {
            return players[0].playerId;
        }

        public virtual int getNextPlayerId() {
            playerIndex = (playerIndex + 1) % players.Length;
            return players[playerIndex].playerId;
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {

            for(int i = 1; i < maxPlayers + 1; i++) {
                if (numSlots[i] == null) {
                    GameObject playerObj = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                    Player player = playerObj.getComponent<Player>();
                    player.playerId = i;
                    players[i] = player;
                    NetworkServer.AddPlayerForConnection(conn, playerObj, playerControllerId);
                    return;
                }
            }
            conn.Disconnect();
        }

        public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController playerController) {
            playerController player = playerController.gameObject.GetComponent<Player>();
            players[player.playerId] = null;
            base.OnserverRemovePlayer(conn, playerController);
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
