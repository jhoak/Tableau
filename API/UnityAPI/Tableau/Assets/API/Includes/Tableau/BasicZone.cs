using System;
using System.Collections;
using UnityEngine;

namespace Tableau.Base {

    public class BasicZone : Zone {

        public const int maxOccupants;
        private static const int DEFAULT_LEN = 8;

        private Piece[] occupants;
        private int numOccupants;

        public void Start() {
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
    }
}
