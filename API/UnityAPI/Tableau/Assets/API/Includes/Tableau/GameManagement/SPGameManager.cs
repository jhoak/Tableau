using UnityEngine;
using System.Collections;
using Tableau.Base;

namespace Tableau.Base.Management {
    // Single-player game manager. ONLY for use in games with no other players or AIs.
    // (unless you feel like starting from scratch)
    public class SPGameManager : MonoBehaviour {

        public void Start() {
            EventManager.StartListening("Win", HandleWin);
            EventManager.StartListening("Loss", HandleLoss);
        }

        public virtual void HandleWin() {
            // user-defined
        }

        public virtual void HandleLoss() {
            // user-defined
        }
    }
}
