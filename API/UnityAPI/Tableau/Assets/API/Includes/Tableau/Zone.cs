using System;
using System.Collections;
using UnityEngine;

namespace Tableau.Base {

    public abstract class Zone : MonoBehaviour {
        // TODO comment
        // TODO make hoverable, clickable (Sounds + animations), draggable (ie move chess sidelines)
        // TODO clamp piece(s) (if clamp is true) when zone is moved (presumably with board) / on
        // game start
        // TODO define piece manager that defines how pieces should be physically arranged, +whether
        // they should also clamp (and whether they should clamp to the center)
        // TODO physics (move with board)
    }

    public class BasicZone : Zone {
        
        // fail (can)adds if piece already in zone
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
            return (maxOccupants == 0) ? false : (numOccupants == maxOccupants);
        }

        public bool CanAddPiece(Piece p) {
            return !IsFull() && !PieceInZone(p);
        }

        public bool AddPiece(Piece p) {
            if ((p == null) || !CanAddPiece(p)) {
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

        public bool CanAddPieces(Piece[] ps) {
            if (ps == null || ps.length == 0) {
                return false;
            }
            else if (maxOccupants != 0 && ps.length > maxOccupants - numOccupants) {
                return false;
            }
            else {
                foreach (Piece p in ps) {
                    if (!CanAddPiece(p)) {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool AddPieces(Piece[] ps) {
            if (!CanAddPieces(ps)) {
                return false;
            }
            else {
                while (ps.length > (occupants.length - numOccupants)) {
                    ResizeOccupantsArray();
                }
                return true;
            }
        }

        public bool CanReleasePiece(Piece p) {
            return PieceInZone(p);
        }

        public bool ReleasePiece(Piece p) {
            if (!CanReleasePiece(p)) {
                return false;
            }
            else {
                for (int i = 0; i < occupants.length; i++) {
                    if (p.equals(occupants[i])) {
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
                if (p.equals(occupants[i])) {
                    return true;
                }
            }
            return false;
        }
    }
}
