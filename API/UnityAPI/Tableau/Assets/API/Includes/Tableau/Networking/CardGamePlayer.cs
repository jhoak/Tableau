using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Tableau.Base;

namespace Tableau.Base.Net {
    
    public class CardGamePlayer : Player {
        /*
        public int maxHandSize = 5;

        //Use [SyncVar] to indicate which variables should be synchronized.
        [SyncVar]
        public Card[] handCards;


        public override void OnStartClient() {
            
        }

        public override void OnNetworkDestroy() {
        }

        public override void OnStartServer() {

        }

        public override void OnStartLocalPlayer() {

        }

        [Server]
        public void ServerAddCard(Card newCard) {
            if (handCards.Length + 1 < maxHandSize) {
                handCards.Add(newCard);
                RpcAddCard(newCard);
            }
        }
        //RPC = Remote Procedure Call. The way to perform an action across a network.
        //[ClientRpc] is called on the server, but runs on the client.
        [ClientRpc]
        void RpcAddCard(Card newCard) {
            if (!isServer) {
                // this was already done for host player
                handCards.Add(newCard);
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
            var card = handCards[0];
            card.hidden = false;
            handCards[0] = card;
            

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
        //Commands should not be sent every frame, this would cause a lot of network traffic.*/

    }
}
