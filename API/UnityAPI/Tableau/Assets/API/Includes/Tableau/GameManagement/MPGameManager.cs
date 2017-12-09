using UnityEngine;
using System.Collections;
using HoloToolkit.Unity.InputModule;
using UnityEngine.Networking;
using Tableau.Base;

namespace Tableau.Base.Management {
    // For use in 2-player multiplayer games.
    public class MPGameManager : NetworkBehaviour {

        [SyncVar] public int turnPlayerId = -1; // 0 or 1; 0 is host, 1 is client
        private int changedToId = -2; // gets set on update after turn change
        public InteractionInputSource gestureSource;
        public ButtonController mouseButtonController;
        public ButtonController.ButtonType bcType;

        // Use this for initialization
        void Start() {
            gestureSource = GameObject.FindObjectOfType<InteractionInputSource>();
            mouseButtonController = GameObject.FindObjectOfType<ButtonController>();
            bcType = mouseButtonController.buttonType;
            EventManager.StartListening("OwnTurn", HandleOwnTurnStart);
            EventManager.StartListening("EnemyTurn", HandleEnemyTurnStart);
            EventManager.StartListening("Win", HandleWin);
            EventManager.StartListening("Loss", HandleLoss);
            EventManager.StartListening("Draw", HandleDraw);

            if (isServer) {
                turnPlayerId = PickFirstPlayer();
            }
        }

        public virtual int PickFirstPlayer() {
            return Random.Range(0, 2);
        }

        public virtual void ChangeTurn() {
            if ((isServer && turnPlayerId == 0) || (!isServer && turnPlayerId == 1)) {
                turnPlayerId = 1 - turnPlayerId;
            }
        }

        public virtual void HandleOwnTurnStart() {
            // switched to our turn! start accepting gesture inputs (not including Gaze) and mouse input
            gestureSource.StartGestureRecognizer();
            mouseButtonController.buttonType = bcType;
        }

        public virtual void HandleEnemyTurnStart() {
            // their turn...
            gestureSource.StopGestureRecognizer();
            mouseButtonController.buttonType = ButtonController.ButtonType.None;
        }

        public virtual void HandleWin() { }
        public virtual void HandleLoss() { }
        public virtual void HandleDraw() { }

        private void Update() {
            if (turnPlayerId == -1) {
                throw new System.Exception("Uninitialized turn ID!");
            }
            if (turnPlayerId != changedToId) {
                changedToId = turnPlayerId;
                if ((isServer && turnPlayerId == 0) || (!isServer && turnPlayerId == 1)) {
                    EventManager.TriggerEvent("OwnTurn");
                }
                else {
                    EventManager.TriggerEvent("EnemyTurn");
                }
            }
        }
    }
}
