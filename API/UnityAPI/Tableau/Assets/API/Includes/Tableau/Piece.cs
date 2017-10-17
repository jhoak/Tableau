using System;
using System.Collections;
using UnityEngine;
using Tableau.Util;

namespace Tableau.Base {

    /*
     * Represents an arbitrary game piece. This could be a card, a piece in a board game (e.g. the
     * horse or the thimble in Monopoly), or anything else really.
     */
    public class Piece : TableauObject {

        private Zone occupiedZone; // the Zone that this Piece is in
        public bool draggable = false;
        private int id;

        public void Start() {
            base.Start();
            id = IDManager.getNewPieceID();
        }

        /*
         * If this Piece collided with a Zone, and it looks like the Piece should belong to the
         * Zone, then this method tries to associate the two by putting the piece in the zone. This
         * illustrates the relationship between Pieces and Zones; Pieces usually tell Zones when to
         * add and release them, although Zones sometimes have special rules that prevent this (i.e.
         * you usually can't remove an entire card deck from the deck zone (unless, perhaps, you are
         * somehow allowed to draw every single card in your deck...)).
         */
        void OnCollisionEnter(Collision c) {
            // If it is colliding with the zone directly beneath it, it's in that zone.
            // 1) Get the Zone (if any) that is straight down from this piece.
            RaycastHit hitInfo;

            Vector3 rayStart = gameObject.transform.position,
                    rayVector = new Vector3(0, -10000, 0); // TODO :^)
            if (Physics.Raycast(rayStart, rayVector, out hitInfo)) {
                // 2) we hit something! is it a zone?
                GameObject rayCollidedWith = hitInfo.collider.gameObject;
                try {
                    ///Zone rayCollidedZone = (Zone)rayCollidedWith;    This type of casting will not work with Unity.
                    Zone rayCollidedZone = new BasicZone();
                    // 3) Now see if the Collision object also has this zone.
                    foreach (ContactPoint cp in c.contacts) {
                        if (CollidedWithZone(rayCollidedZone, cp)) {
                            // 4) We collided with a zone! Now try to set it as this Piece's zone.
                            if (rayCollidedZone.Equals(occupiedZone)) {
                                return;
                            }
                            else if (LeaveCurrentZone()) {
                                if (rayCollidedZone.Add(this)) {
                                    occupiedZone = rayCollidedZone;
                                }
                                else {
                                    Debug.LogError("Couldn't enter the new zone!");
                                }
                            }
                        }
                    }
                }
                catch (Exception x) {
                    // Ray didn't collide with a zone! Don't count it.
                }
            }
        }

        /*
         * Given a Zone and a contact point, returns true if the piece collided with a Zone and this
         * Zone just happens to be the same as the given Zone. Else, returns false.
         */
        private bool CollidedWithZone(Zone z, ContactPoint cp) {
            try {
                return z.Equals(cp.otherCollider.gameObject);
            }
            catch (Exception x) {
                return false;
            }
        }

        void OnCollisionExit() {
            LeaveCurrentZone();
        }

        /*
         * Dissociates this Piece from the Zone it is occupying. This may not be possible if the
         * Zone has special rules that prevent the Piece from leaving the Zone.
         */
        private bool LeaveCurrentZone() {
            if (occupiedZone == null) {
                return true;
            }
            else {
                if (occupiedZone.Release(this)) {
                    occupiedZone = null;
                    return true;
                }
                else {
                    // TODO move piece back to its place in the zone
                    // TODO also fire an event like illegal move or something (need listeners) so as
                    //      to deal with what happens when dragging the piece
                    return true; //temporary, to shut Unity up
                }
            }
        }

        
        public bool Equals(GameObject o) {
            Piece otherPiece = o.GetComponent<Piece>();
            if (otherPiece.getID() == this.id){
                return true;
            }else{
                return false;
            }                
        }
                return false;
            }                
            if (draggable) {
                Vector3 cursorPosition = e.cursorPosition;
                gameObject.transform.position = cursorPosition;
            }
        }

        public override void OnDragEnd(CursorEvent e) {
            // do nothing (basically just stop moving)
            // do nothing (basically just stop moving)
        }

        // do nothing on gaze or tap (can be overridden, of course)
        public override void OnGazeEnter(CursorEvent e) {}

        public override void OnGazeExit(CursorEvent e) {}

        public override void OnTapEnter(CursorEvent e) {}

        public override void OnTapExit(CursorEvent e) {}

        public override void WarnIfOversized() {
            Vector3 size = new Vector3(-1, -1, -1);
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
                if (size != null && (size.x > 1 || size.y > 1 || size.z > 1)) {
                    Debug.LogWarning(
                        "This board might be too big (" + size.x + ", " + size.y + ", " + size.z + ")!"
                    );
                }
            }
        }

        override public int getID(){
            return id;
        }
    }
}
