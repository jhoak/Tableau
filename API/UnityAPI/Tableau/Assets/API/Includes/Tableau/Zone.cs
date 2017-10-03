using System;
using System.Collections;
using UnityEngine;

namespace Tableau.Base {

    /*
     * Represents a special place where a Piece(s) can be placed. Usually a Zone may have special
     * rules associated with it (e.g. you can only put a card deck in a deck zone).
     * Note that Pieces may still be placed outside of a Zone (e.g. a Board, or even the spatial
     * map). It's just that those areas generally will not have a special effect or interaction with
     * these Pieces.
     */
    public abstract class Zone : MonoBehaviour {
        // TODO make hoverable, clickable (Sounds + animations), draggable (ie move chess sidelines)
        // TODO clamp piece(s) (if clamp is true) when zone is moved (presumably with board) / on
        // game start
        // TODO define piece manager that defines how pieces should be physically arranged, +whether
        // they should also clamp (and whether they should clamp to the center)
        // TODO physics (move with board)
        // TODO logging

        /* Add a Piece to this Zone, and return true if successful. Otherwise, return false. */
        public abstract bool Add(Piece p);

        /* Remove the given Piece from this Zone. Return true if successful, otherwise false. */
        public abstract bool Release(Piece p);

        /* Return true if the given object is equivalent to this Zone, otherwise false. */
        public abstract bool Equals(GameObject o);
    }
}
