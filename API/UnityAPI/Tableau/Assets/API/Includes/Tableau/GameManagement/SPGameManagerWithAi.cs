using UnityEngine;
using System.Collections;
using HoloToolkit.Unity.InputModule;
using Tableau.Base;

namespace Tableau.Base.Management {
    // For use in single-player games against one AI player.
    public class SPGameManagerWithAi : MonoBehaviour {

        public bool isPlayersTurn;

        // Player input controllers
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

            isPlayersTurn = PickFirstPlayer();
            if (isPlayersTurn) {
                EventManager.TriggerEvent("OwnTurn");
            }
            else {
                EventManager.TriggerEvent("EnemyTurn");
            }
        }

        public virtual bool PickFirstPlayer() {
            int i = Random.Range(0, 2);
            return i == 0;
        }

        // the honor system...
        public virtual void ChangeTurn() {
            isPlayersTurn = !isPlayersTurn;
            if (isPlayersTurn) {
                EventManager.TriggerEvent("OwnTurn");
            }
            else {
                EventManager.TriggerEvent("EnemyTurn");
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
    }
}
