using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Tableau.Base.Net {
    public class TableauNetworkManager : NetworkManager {

        const int maxPlayers = 2;
        Player[] players = new Player[3];

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