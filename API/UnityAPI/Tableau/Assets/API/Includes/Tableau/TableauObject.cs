using System;
using System.Collections;
using UnityEngine;

namespace Tableau.Base {

    public abstract class TableauObject : MonoBehaviour, Gazeable, Draggable {

        public void Start() {
            Setup();
            WarnIfOversized();
        }

        public abstract void Setup();

        /* Required interface methods */
        public abstract void OnGazeEnter(CursorEvent e);
        public abstract void OnGazeExit(CursorEvent e);
        public abstract void OnTapEnter(CursorEvent e);
        public abstract void OnTapExit(CursorEvent e);
        public abstract void OnDragStart(CursorEvent e);
        public abstract void OnDragEnd(CursorEvent e);

        /*
         * An object-specific method that gets the object's size and logs a warning if the object is
         * likely too big for the HoloLens environment. To find out how big "too big" is, I
         * recommend testing a scene in a real HoloLens that contains several objects of the same
         * type at different sizes, from pretty small to pretty large in the editor.
         *
         * (((Please don't fudge the numbers for this.)))
         */
        public abstract void WarnIfOversized();

        public abstract bool Equals(GameObject o);

        public abstract string Serialize();
    }
}
