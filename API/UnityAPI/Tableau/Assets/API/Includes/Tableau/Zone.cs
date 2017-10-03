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
        // TODO logging

        public abstract bool Add(Piece p);

        public abstract bool Release(Piece p);

        public abstract bool Equals(GameObject o);
    }
}