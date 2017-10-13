using System;
using System.Collections;
using UnityEngine;

namespace Tableau.Base {

    /*
     * A very basic implementation of a Zone. This can store any kind of Piece, up to a given max
     * number of pieces. If the max is zero, then any number of Pieces may be stored.
     */
    public class BasicZone : Zone {
        /* If the max is 0, then the 'occupants' array will be initialized to this length. */
        private static const int DEFAULT_LEN = 8;

        public const int maxOccupants = 8;
        private Piece[] occupants;
        private int numOccupants;
        public bool draggable = false;

        // This executes when the game scene loads.
        public void Start() {
            base.Start();
            int initialLen = (maxOccupants != 0) ? maxOccupants : DEFAULT_LEN;
            occupants = new Piece[initialLen];
            numOccupants = 0;
        }

        public Piece[] GetPieces() {
            Piece[] actualOccupants = new Piece[numOccupants];
            for (int occIndex = 0, actualIndex = 0; occIndex < occupants.length; occIndex++) {
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
        public bool Add(Piece p) {
            if ((p == null) || !CanAdd(p)) {
                return false;
            }
            else {
                if (numOccupants == occupants.length) {
                    ResizeOccupantsArray();
                }
                for (int i = 0; i < occupants.length; i++) {
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
            if (ps == null || ps.length == 0) {
                return false;
            }
            else if (maxOccupants != 0 && ps.length > maxOccupants - numOccupants) {
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
                while (ps.length > (occupants.length - numOccupants)) {
                    ResizeOccupantsArray();
                }
                return true;
            }
        }

        public bool CanRelease(Piece p) {
            return PieceInZone(p);
        }

        // Attempts to remove the given piece from the zone. Returns true if successful, else false.
        public bool Release(Piece p) {
            if (!CanRelease(p)) {
                return false;
            }
            else {
                for (int i = 0; i < occupants.length; i++) {
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
                for (int i = 0; i < occupants.length; i++) {
                    occupants[i] = null;
                }
                return true;
            }
        }

        private void ResizeOccupantsArray() {
            Piece[] newOccs = new Piece[occupants.length * 2];
            for (int i = 0; i < occupants.length; i++) {
                newOccs[i] = occupants[i];
            }
            occupants = newOccs;
        }

        private bool PieceInZone(Piece p) {
            if (p == null) {
                return false;
            }
            for (int i = 0; i < occupants.length; i++) {
                if (p.Equals(occupants[i])) {
                    return true;
                }
            }
            return false;
        }

        public bool Equals(GameObject o) {
            try {
                return ((BasicZone)o) == this;
            }
            catch (Exception x) {
                return false;
            }
        }

        public void OnDragStart(CursorEvent e) {
            if (draggable) {
                Vector3 cursorPosition = e.cursorPosition;
                gameObject.transform.position = cursorPosition;
            }
        }

        public void OnDragExit(CursorEvent e) {
            // do nothing (basically just stop moving)
        }

        // do nothing on gaze or tap (can be overridden, of course)
        public void OnGazeEnter(CursorEvent e) {}

        public void OnGazeExit(CursorEvent e) {}

        public void OnTapEnter(CursorEvent e) {}

        public void OnTapExit(CursorEvent e) {}

        public void WarnIfOversized() {
            Vector3 size = null;
            try {
                size = GetComponent<Renderer>().bounds.size;
            }
            catch (Exception x) {
                try {
                    size = GetComponent<Collider>().bounds.size;
                }
                catch (Exception x) {
                    // don't do anything...we'll let the programmer figure it out at this point
                }
            }
            finally {
                // Note: 1 unit corresponds to 1 meter in the real world, here
                if (size != null && (size.x > 1 || size.y > 1 || size.z > 1)) {
                    Debug.LogWarning(
                        "This board might be too big (%d, %d, %d)!",
                        size.x,
                        size.y,
                        size.z
                    );
                }
            }
        }
    }
}
