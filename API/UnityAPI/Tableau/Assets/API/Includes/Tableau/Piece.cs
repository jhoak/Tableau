using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Tableau.Base {

    /*
     * Represents an arbitrary game piece. This could be a card, a piece in a board game (e.g. the
     * horse or the thimble in Monopoly), or anything else really.
     */
    public class Piece : TableauObject {

        protected Zone occupiedZone; // the Zone that this Piece is in
        public bool draggable = false;

        public override void Setup() {
            // no need to do anything
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
                    rayVector = new Vector3(0, -10, 0); // TODO :^)
            if (Physics.Raycast(rayStart, rayVector, out hitInfo)) {
                // 2) we hit something! is it a zone?
                GameObject rayCollidedWith = hitInfo.collider.gameObject;
                try {
                    Zone rayCollidedZone = rayCollidedWith.GetComponent<Zone>();
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
                GameObject collidedWith = cp.otherCollider.gameObject;
                Zone z2 = collidedWith.GetComponent<Zone>();
                return z.Equals(z2);
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
                    return false;
                }
            }
        }

        public override bool Equals(GameObject o) {
            try {
                Piece p = o.GetComponent<Piece>();
                return p == this;
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
                if (size != new Vector3(-1,-1,-1) && (size.x > 1 || size.y > 1 || size.z > 1)) {
                    Debug.LogWarning(string.Format(
                        "This piece might be too big (%d, %d, %d)!",
                        size.x,
                        size.y,
                        size.z
                    ));
                }
            }
        }

        public override string Serialize() {
            string id = this.GetInstanceID().ToString(),
                   zoneId = (occupiedZone != null) ? occupiedZone.GetInstanceID().ToString() : "";
            return String.Format(
                "id={0},drag={1},zoneId={2};",
                id,
                draggable,
                zoneId
            );
        }

        public static Piece DeserializePiece(string serializedObject) {
            Piece[] allPieces = GameObject.FindObjectsOfType<Piece>();
            Match m = Regex.Match(serializedObject, "^id=(.*?),drag=(.*?),zoneId=(.*?);$");
            string id = m.Groups[1].Value,
                   draggable = m.Groups[2].Value,
                   zoneId = m.Groups[3].Value;
            foreach (Piece p in allPieces) {
                if (p.GetInstanceID() == int.Parse(id)) {
                    p.draggable = bool.Parse(draggable);
                    p.occupiedZone = null;
                    if (!zoneId.Equals("")) {
                        // Find zone with given ID
                        Zone[] zones = GameObject.FindObjectsOfType<Zone>();
                        foreach (Zone z in zones) {
                            if (z.GetInstanceID() == int.Parse(zoneId)) {
                                p.occupiedZone = z;
                            }
                        }
                    }

                    return p;
                }
            }
            return null;
        }
    }
}
