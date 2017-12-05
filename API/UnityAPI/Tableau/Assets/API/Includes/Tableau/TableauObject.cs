using System;
using System.Collections;
using UnityEngine;

namespace Tableau.Base {

    public abstract class TableauObject : MonoBehaviour, Gazeable, Draggable {

        /* Required interface methods */
        public abstract void OnGazeEnter(CursorEvent e);
        public abstract void OnGazeExit(CursorEvent e);
        public abstract void OnTapEnter(CursorEvent e);
        public abstract void OnTapExit(CursorEvent e);
        public abstract void OnDragStart(CursorEvent e);
        public abstract void OnDragEnd(CursorEvent e);

        /*
         * The start shared by all TableauObjects in general. Should be called by subclasses 1st.
         */
        public void Start() {
            WarnIfOversized();
        }

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

        /*public abstract string Serialize();

        public static TableauObject Deserialize(string serializedObject) {
            // "Abstract static" methods don't exist, so this seems like the next best thing.
            // Require subclasses to override this, by throwing an error if they *don't* override it.
            // To be clear, this is *not* some temporary code intended to be rewritten later.
            throw new InvalidOperationException("Class does not implement Deserialize(string)!");
        }*/
    }
}
