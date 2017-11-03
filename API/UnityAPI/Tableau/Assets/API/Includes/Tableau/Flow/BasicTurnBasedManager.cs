using System;
using UnityEngine;

namespace Tableau.Base.Flow {

    public class BasicTurnBasedManager : GameManager {

        public static const BasicTurnBasedManager Instance = new BasicTurnBasedManager();
        private int playerTurn = -1;

        protected BasicTurnBasedManager() {}

        void Start() {
            Base.Start();
            Player[] players = (Player[])FindObjectsOfType(typeof(Player));
            playerTurn = new Random().Next(0, players.length);
        }

    }

}
