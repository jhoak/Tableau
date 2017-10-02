using System;
using System.Collections;
using UnityEngine;

namespace Tableau.Base {

    public abstract class Zone : MonoBehaviour {
        // TODO make hoverable, clickable (Sounds + animations)
        // TODO clamp piece(s) (if clamp is true) when zone is moved (presumably with board) / on
        // game start
        // TODO define piece manager that defines how pieces should be physically arranged, whether
        // they should also clamp
    }

    public class SingleZone : Zone {

        private Piece piece;

        public Piece GetPiece() {
            return piece;
        }

        public bool IsEmpty() {
            return piece == null;
        }

        public bool CanAddPiece() {
            return this.IsEmpty();
        }

        public bool AddPiece(Piece p) {
            if ((p == null) || (!this.isEmpty())) {
                return false;
            }
            else {
                piece = p;
                return true;
            }
        }

        public bool CanReleasePiece() {
            return !this.IsEmpty();
        }

        public bool ReleasePiece() {
            if (piece != null) {
                piece = null;
                return true;
            }
            return false;
        }

        public void Start() {
            Piece[] pieces = GetComponents<Piece>();
            // Can't have more than 1 piece in a SingleZone
            if (pieces.length > 1) {
                throw new Exception("Zone " + this.name + " cannot hold more than 1 piece!");
            }
            else if (pieces.length == 1) {
                piece = pieces[0];
            }
        }
    }

    public class MultiZone : Zone {

        private Piece[] pieces;

        public Piece[] GetPieces() {
            return pieces;
        }

        public bool IsEmpty() {
            return pieces.length == 0;
        }

        public bool IsFull() {
            return false;
        }

        public bool CanAddPiece() {
            return !this.IsFull();
        }

        public bool AddPiece(Piece p) {
            if (!this.CanAddPiece()) {
                return false;
            }
            else {
                int i;
                for (i = 0; i < pieces.length; i++) {
                    if (pieces[i] == null) {
                        pieces[i] = p;
                        return true;
                    }
                }
                Piece[] newPieces = new Piece[pieces.length * 2];
                for (int j = 0; j < pieces.length; j++) {
                    newPieces[j] = pieces[j];
                }
                pieces = newPieces;
                pieces[i] = p;
                return true;
            }
        }

        public bool CanAddPieces() {
            return !this.IsFull();
        }

        public bool AddPieces(Piece[] ps) {
            int pieceIndex = 0, mainIndex = 0;
            while (pieceIndex < ps.length) {
                Piece toAdd = ps[pieceIndex];
                if ((mainIndex < pieces.length) && pieces[mainIndex] == null) {
                    pieces[mainIndex] = toAdd;
                    pieceIndex++;
                    mainIndex++;
                }
                else if (mainIndex < pieces.length) {
                    mainIndex++;
                }
                else {
                    Piece[] newPieces = new Piece[(int)((pieces.length + ps.length) * 1.2)];
                    for (int j = 0; j < pieces.length; j++) {
                        newPieces[j] = pieces[j];
                    }
                    pieces = newPieces;
                }
            }
            return true;
        }

        public bool CanReleasePiece(Piece p) {
            if (p == null) {
                return false;
            }
            else {
                bool inArray = false;
                for (int i = 0; i < pieces.length; i++) {
                    if (pieces[i].equals(p)) {
                        inArray = true;
                        break;
                    }
                }
                return inArray;
            }
        }

        public bool ReleasePiece(Piece p) {
            if (p == null) {
                return false;
            }
            else {
                for (int i = 0; i < pieces.length; i++) {
                    if (pieces[i].equals(p)) {
                        pieces[i] = null;
                        return true;
                    }
                }
                return false;
            }
        }

        public bool CanReleaseAllPieces() {
            return !this.isEmpty();
        }

        public bool ReleaseAllPieces() {
            if (pieces.length == 0) {
                return false;
            }
            else {
                released = false;
                for (int i = 0; i < pieces.length; i++) {
                    if (pieces[i] != null) {
                        pieces[i] = null;
                        released = true;
                    }
                }
                return released;
            }
        }

        public void Start() {
            pieces = GetComponents<Piece>();
        }
    }
}
