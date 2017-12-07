using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Tableau.Base;

namespace Tableau.Base.Net {
    
    public class CardGamePlayer : NetworkBehaviour {
        public int maxHandSize = 5;

        //Use [SyncVar] to indicate which variables should be synchronized.
        [SyncVar]
        public int playerId;

        private SyncListString handCards = new SyncListString();


        public override void OnStartClient() {
            
        }

        public override void OnNetworkDestroy() {
        }

        public override void OnStartServer() {

        }

        public override void OnStartLocalPlayer() {

        }

        [Server]
        public void ServerAddCard(string serializedCard) {
            if (handCards.Count + 1 < maxHandSize) {
                handCards.Add(serializedCard);
                RpcAddCard(serializedCard);
            }
        }
        //RPC = Remote Procedure Call. The way to perform an action across a network.
        //[ClientRpc] is called on the server, but runs on the client.
        [ClientRpc]
        void RpcAddCard(string serializedCard) {
            if (!isServer) {
                // this was already done for host player
                handCards.Add(serializedCard);
            }
        }

        [Server]
        public void ServerClearCards() {
            handCards.Clear();

            RpcClearCards();
        }

        [ClientRpc]
        private void RpcClearCards() {
            if (!isServer) {
                // this was already done for host player
                handCards.Clear();
            }
        }

        [Client]
        public void ShowCards() {
            foreach (string serializedCard in handCards) {
                Card c = Card.DeserializeCard(serializedCard);
                c.hidden = false;
            }
        }

        [ClientRpc]
        public void RpcYourTurn(bool isYourTurn) {
            // make player who is having current turn green
            Color c = new Color(1, 1, 1, 0.5f);
            if (isYourTurn)
                c = Color.green;
            
        }

        //there is also a [Command] tag which means that the command will be sent from the player
        //object on the client to the player objects on the server. You must also add the 'Cmd' prefix.
        //Commands should not be sent every frame, this would cause a lot of network traffic.

    }
}
