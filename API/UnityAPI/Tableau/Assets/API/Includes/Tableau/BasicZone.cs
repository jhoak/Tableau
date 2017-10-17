using System;
using System.Collections;
using UnityEngine;
using Tableau.Util;

namespace Tableau.Base {

    /*
     * A very basic implementation of a Zone. This can store any kind of Piece, up to a given max
     * number of pieces. If the max is zero, then any number of Pieces may be stored.
     */
    public class BasicZone : Zone {
        /* If the max is 0, then the 'occupants' array will be initialized to this length. */
        private const int DEFAULT_LEN = 8;

        public const int maxOccupants = 8;
        private Piece[] occupants;
        private int numOccupants;
        public bool draggable = false;
        private int id;



        // This executes when the game scene loads.
        public override void Start() {
            base.Start();
            int initialLen = (maxOccupants != 0) ? maxOccupants : DEFAULT_LEN;
            occupants = new Piece[initialLen];
            numOccupants = 0;
            id = IDManager.getNewZoneID();
        }

        public Piece[] GetPieces() {
            Piece[] actualOccupants = new Piece[numOccupants];
            for (int occIndex = 0, actualIndex = 0; occIndex < occupants.Length; occIndex++) {
                if (occupants[occIndex] != null) {
                    actualOccupants[actualIndex] = occupants[occIndex];
                    actualIndex++;
                }
            }
            return actualOccupants;
        }

        public bool IsEmpty() {
            return numOccupants == 0;
        }

        public bool IsFull() {
            return (maxOccupants != 0) && (numOccupants == maxOccupants);
        }

        public bool CanAdd(Piece p) {
            return !IsFull() && !PieceInZone(p);
        }

        // Attempts to add a given piece to the zone. Returns true if successful, else false.
<<<<<<< HEAD
        override
        public bool Add(Piece p) {
=======
        public override bool Add(Piece p) {
>>>>>>> cef93b454ce2ae6cece9a0de19263fbe8cf1881e
            if ((p == null) || !CanAdd(p)) {
                return false;
            }
            else {
                if (numOccupants == occupants.Length) {
                    ResizeOccupantsArray();
                }
                for (int i = 0; i < occupants.Length; i++) {
                    if (occupants[i] == null) {
                        occupants[i] = p;
                        numOccupants++;
                        return true;
                    }
                }
            }
            return false; // shouldn't happen... but needed for compiler I think?
        }

        public bool CanAdd(Piece[] ps) {
            if (ps == null || ps.Length == 0) {
                return false;
            }
            else if (maxOccupants != 0 && ps.Length > maxOccupants - numOccupants) {
                return false;
            }
            else {
                foreach (Piece p in ps) {
                    if (!CanAdd(p)) {
                        return false;
                    }
                }
                return true;
            }
        }

        // Attempts to add the given pieces to the zone. Returns true if successful, else false.
        public bool Add(Piece[] ps) {
            if (!CanAdd(ps)) {
                return false;
            }
            else {
                while (ps.Length > (occupants.Length - numOccupants)) {
                    ResizeOccupantsArray();
                }
                return true;
            }
        }

        public bool CanRelease(Piece p) {
            return PieceInZone(p);
        }

        // Attempts to remove the given piece from the zone. Returns true if successful, else false.
<<<<<<< HEAD
        override
        public bool Release(Piece p) {
=======
        public override bool Release(Piece p) {
>>>>>>> cef93b454ce2ae6cece9a0de19263fbe8cf1881e
            if (!CanRelease(p)) {
                return false;
            }
            else {
                for (int i = 0; i < occupants.Length; i++) {
                    if (p.Equals(occupants[i])) {
                        occupants[i] = null;
                        numOccupants--;
                        return true;
                    }
                }
            }
            return false; // shouldn't happen but needed for compiler?
        }

        public bool CanReleaseAll() {
            return numOccupants != 0;
        }

        // Attempts to remove all pieces from the zone. Returns true if successful, else false.
        public bool ReleaseAll() {
            if (!CanReleaseAll()) {
                return false;
            }
            else {
                for (int i = 0; i < occupants.Length; i++) {
                    occupants[i] = null;
                }
                return true;
            }
        }

        private void ResizeOccupantsArray() {
            Piece[] newOccs = new Piece[occupants.Length * 2];
            for (int i = 0; i < occupants.Length; i++) {
                newOccs[i] = occupants[i];
            }
            occupants = newOccs;
        }

        private bool PieceInZone(Piece p) {
            if (p == null) {
                return false;
            }
            for (int i = 0; i < occupants.Length; i++) {
                if (p.Equals(occupants[i])) {
                    return true;
                }
            }
            return false;
        }

<<<<<<< HEAD
        //Unity thinks there is an error bc of this cast. We'll have to find a different way to do equals;
        override
        public bool Equals(GameObject o) {
            BasicZone otherPiece = o.GetComponent<BasicZone>();
            if (otherPiece.getID() == this.id){
                return true;
            }else{
=======
        public override bool Equals(GameObject o) {
            try {
                BasicZone bz = o.GetComponent<BasicZone>();
                return bz == this;
            }
            catch (Exception x) {
>>>>>>> cef93b454ce2ae6cece9a0de19263fbe8cf1881e
                return false;
            }  
        }

<<<<<<< HEAD
        override
        public void OnDragStart(CursorEvent e) {
=======
        public override void OnDragStart(CursorEvent e) {
>>>>>>> cef93b454ce2ae6cece9a0de19263fbe8cf1881e
            if (draggable) {
                Vector3 cursorPosition = e.cursorPosition;
                gameObject.transform.position = cursorPosition;
            }
        }

<<<<<<< HEAD
        override
        public void OnDragEnd(CursorEvent e) {
=======
        public override void OnDragEnd(CursorEvent e) {
>>>>>>> cef93b454ce2ae6cece9a0de19263fbe8cf1881e
            // do nothing (basically just stop moving)
        }

        // do nothing on gaze or tap (can be overridden, of course)
<<<<<<< HEAD
        override
        public void OnGazeEnter(CursorEvent e) {}
        override
        public void OnGazeExit(CursorEvent e) {}
        override
        public void OnTapEnter(CursorEvent e) {}
        override
        public void OnTapExit(CursorEvent e) {}

        override
        public void WarnIfOversized() {
            Vector3 size = new Vector3(0, 0, 0);
=======
        public override void OnGazeEnter(CursorEvent e) {}

        public override void OnGazeExit(CursorEvent e) {}

        public override void OnTapEnter(CursorEvent e) {}

        public override void OnTapExit(CursorEvent e) {}

        public override void WarnIfOversized() {
            Vector3 size = new Vector3(-1,-1,-1);
>>>>>>> cef93b454ce2ae6cece9a0de19263fbe8cf1881e
            try {
                size = GetComponent<Renderer>().bounds.size;
            }
            catch (Exception x) {
                try {
                    size = GetComponent<Collider>().bounds.size;
                }
<<<<<<< HEAD
                catch (Exception ex) {
=======
                catch (Exception y) {
>>>>>>> cef93b454ce2ae6cece9a0de19263fbe8cf1881e
                    // don't do anything...we'll let the programmer figure it out at this point
                }
            }
            finally {
                // Note: 1 unit corresponds to 1 meter in the real world, here
<<<<<<< HEAD
                if (size != null && (size.x > 1 || size.y > 1 || size.z > 1)) {
                    Debug.LogWarning(
                        "This board might be too big (" + size.x + ", " + size.y + ", " + size.z + ")!"
                    );
=======
                if (size != new Vector3(-1, -1, -1) && (size.x > 1 || size.y > 1 || size.z > 1)) {
                    Debug.LogWarning(string.Format(
                        "This zone might be too big (%d, %d, %d)!",
                        size.x,
                        size.y,
                        size.z
                    ));
>>>>>>> cef93b454ce2ae6cece9a0de19263fbe8cf1881e
                }
            }
        }

         override public int getID(){
            return id;
        }
    }
}
