using System;
using UnityEngine;
using Tableau.Base.Net;

namespace Tableau.Base.Flow {

    public class BasicTurnBasedManager : GameManager {

       // public const BasicTurnBasedManager Instance = new BasicTurnBasedManager();
        private int playerTurn = -1;

        public BasicTurnBasedManager() {}
        /*
        void Start() {
            base.Start();
            Player[] players = (Player[])FindObjectsOfType(typeof(Player));
            playerTurn = new System.Random().Next(0, players.Length);
        }
        */
    }

}
