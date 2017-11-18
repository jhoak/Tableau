using System;
using UnityEngine;
using Tableau.Base.Event;

namespace Tableau.Base.Flow {

    public class GameManager : MonoBehaviour {

       // public const GameManager Instance = new GameManager();

        public GameManager() {}

        public void Start() {
            /*
            // subscribe to all the general events
            GeneralEvents.LoadStart.AddListener(HandleLoadStart);
            GeneralEvents.LoadEnd.AddListener(HandleLoadEnd);
            GeneralEvents.GameStart.AddListener(HandleGameStart);
            GeneralEvents.GameEnd.AddListener(HandleGameEnd);
            GeneralEvents.GameRestart.AddListener(HandleGameRestart);
            */
        }

        void HandleLoadStart() {
            // do nothing...
        }

        void HandleLoadEnd() {
            // do nothing...
        }

        void HandleGameStart() {
            // do nothing...
        }

        void HandleGameEnd() {
            // do nothing...
        }

        void HandleGameRestart() {
            Application.LoadLevel(Application.loadedLevel);
        }

    }

}
