using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Player : NetworkBehaviour {

    public PlayerPanel playerPanel;

    public int maxHandSize = 5;

    [SyncVar]
    public Card[] handCards;

    [SyncVar]
    public int playerId;


    public override void OnStartClient() {

        playerPanel = CardManager.singleton.playerPanels[playerId];
        playerPanel.gameObject.SetActive(true);
    }

    public override void OnNetworkDestroy() {
        if (playerPanel != null) {
            playerPanel.ClearCards();
            playerPanel.gameObject.SetActive(false);
        }
    }

    public override void OnStartServer() {
        CardManager.singleton.AddPlayer(this);
    }

    public override void OnStartLocalPlayer() {
        CardManager.singleton.localPlayer = this;
    }

    [Server]
    public void ServerAddCard(CardId newCard) {
        if (handCards.Length + 1 < maxHandSize) {
            handCards.Add(newCard);
            RpcAddCard(newCard);
        }
    }

    [ClientRpc]
    void RpcAddCard(CardId newCard) {
        if (!isServer) {
            // this was already done for host player
            handCards.Add(newCard);
        }

        CalculateScore();
        playerPanel.AddCard(newCard, cardScore);
    }


    public void MsgAddCard(CardId cardId) {
        handCards.Add(cardId);
        CalculateScore();
        playerPanel.AddCard(cardId, cardScore);
    }

    [Server]
    public void ServerClearCards() {
        handCards.Clear();
        cardScore = 0;

        RpcClearCards();
    }

    [ClientRpc]
    private void RpcClearCards() {
        if (!isServer) {
            // this was already done for host player
            handCards.Clear();
        }
        playerPanel.ClearCards();
        cardScore = 0;
        CardManager.singleton.paidAmount.text = "0";
    }

    [Client]
    public void ShowCards() {
        var card = handCards[0];
        card.hidden = false;
        handCards[0] = card;

        playerPanel.ShowCard(0);
        CalculateScore();
        playerPanel.SetScore(cardScore);

    }

    [ClientRpc]
    public void RpcYourTurn(bool isYourTurn) {
        // make player who is having current turn green
        Color c = new Color(1, 1, 1, 0.5f);
        if (isYourTurn)
            c = Color.green;

        playerPanel.GetComponent<PlayerPanel>().ColorImage(c);

        if (isYourTurn && isLocalPlayer) {
            CardManager.singleton.EnablePlayHandButtons();
        } else {
            CardManager.singleton.ClientDisableAllButtons();
        }
    }

}
