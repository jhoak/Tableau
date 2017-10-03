using System;
using System.Collections;
using UnityEngine;

namespace Tableau.Base {

	public class Piece : MonoBehaviour {
		// TODO comment
		// TODO support more than just falling onto a tile, also what if being dragged?
		// TODO make hoverable, clickable, draggable?? (Sounds + animations)
		// TODO physics
		// TODO logging

		private Zone occupiedZone; // the Zone that this Piece is in

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
					Zone rayCollidedZone = (Zone)rayCollidedWith;
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

		private bool CollidedWithZone(Zone z, ContactPoint cp) {
			try {
				Zone collidedWith = (Zone)cp.otherCollider.gameObject;
				return z.Equals(collidedWith);
			}
			catch (Exception x) {
				return false;
			}
		}

		void OnCollisionExit() {
			LeaveCurrentZone();
		}

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
				}
			}
		}

		public bool Equals(GameObject o) {
			try {
				return ((Piece)o) == this;
			}
			catch (Exception x) {
				return false;
			}
		}
	}
}
