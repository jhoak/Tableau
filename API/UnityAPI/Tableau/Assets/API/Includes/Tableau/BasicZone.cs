using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

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

        public override void Setup() {
            base.Setup();
            int initialLen = (maxOccupants != 0) ? maxOccupants : DEFAULT_LEN;
            occupants = new Piece[initialLen];
            numOccupants = 0;
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
        public override bool Add(Piece p) {
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
        public override bool Release(Piece p) {
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

        public override bool Equals(GameObject o) {
            try {
                BasicZone bz = o.GetComponent<BasicZone>();
                return bz == this;
            }
            catch (Exception x) {
                return false;
            }
        }

        public override void OnDragStart(CursorEvent e) {
            if (draggable) {
                Vector3 cursorPosition = e.cursorPosition;
                gameObject.transform.position = cursorPosition;
            }
        }

        public override void OnDragEnd(CursorEvent e) {
            // do nothing (basically just stop moving)
        }

        // do nothing on gaze or tap (can be overridden, of course)
        public override void OnGazeEnter(CursorEvent e) {}

        public override void OnGazeExit(CursorEvent e) {}

        public override void OnTapEnter(CursorEvent e) {}

        public override void OnTapExit(CursorEvent e) {}

        public override void WarnIfOversized() {
            Vector3 size = new Vector3(-1,-1,-1);
            try {
                size = GetComponent<Renderer>().bounds.size;
            }
            catch (Exception x) {
                try {
                    size = GetComponent<Collider>().bounds.size;
                }
                catch (Exception y) {
                    // don't do anything...we'll let the programmer figure it out at this point
                }
            }
            finally {
                // Note: 1 unit corresponds to 1 meter in the real world, here
                if (size != new Vector3(-1, -1, -1) && (size.x > 1 || size.y > 1 || size.z > 1)) {
                    Debug.LogWarning(string.Format(
                        "This zone might be too big (%d, %d, %d)!",
                        size.x,
                        size.y,
                        size.z
                    ));
                }
            }
        }

        public override string Serialize() {
            // Get values to put in format string
            int id = this.GetInstanceID();
            Vector3 pos = this.transform.position;
            string[] posVals = { pos.x + "", pos.y + "", pos.z + ""};
            string posString = String.Join(":", posVals);
            Quaternion rot = this.transform.rotation;
            string[] rotVals = { rot.w + "", rot.x + "", rot.y + "", rot.z + "" };
            string rotString = String.Join(":", rotVals);
            string occs = "";
            foreach (Piece p in occupants) {
                occs += p.GetInstanceID() + ",";
            }
            occs = occs.Substring(0, occs.Length - 1);
            return String.Format(
                "id={0},drag={1},pos={2},rot={3},occs={4};",
                id,
                draggable,
                posString,
                rotString,
                occs
            );
        }

        public static BasicZone DeserializeBasicZone(string serializedObject) {
            BasicZone[] zones = GameObject.FindObjectsOfType<BasicZone>();
            Match m = Regex.Match(
                serializedObject,
                "^id=(.*?),drag=(.*?),pos=(.*?),rot=(.*?),occs=(.*?);$"
            );
            int id = int.Parse(m.Groups[1].Value);
            foreach (BasicZone B in zones) {
                if (B.GetInstanceID() == id) {
                    B.draggable = bool.Parse(m.Groups[2].Value);

                    // now break down pos, rot, and occs
                    string[] posVals = m.Groups[3].Value.Split(':');
                    B.gameObject.transform.position = new Vector3(
                        float.Parse(posVals[0]),
                        float.Parse(posVals[1]),
                        float.Parse(posVals[2])
                    );
                    string[] rotVals = m.Groups[4].Value.Split(':');
                    B.gameObject.transform.rotation = new Quaternion(
                        float.Parse(rotVals[0]),
                        float.Parse(rotVals[1]),
                        float.Parse(rotVals[2]),
                        float.Parse(rotVals[3])
                    );
                    string[] occIds = m.Groups[5].Value.Split(',');
                    B.occupants = new Piece[occIds.Length];
                    int addedOccs = 0;
                    Piece[] pieces = GameObject.FindObjectsOfType<Piece>();
                    foreach (string oid in occIds) {
                        foreach (Piece p in pieces) {
                            if (int.Parse(oid) == p.GetInstanceID()) {
                                B.occupants[addedOccs] = p;
                                addedOccs++;
                                break;
                            }
                        }
                    }
                    B.numOccupants = addedOccs;
                    return B;
                }
            }
            return null;
        }
    }
}
