using System;
using System.Collections;
using UnityEngine;

namespace Tableau.Base {

	/*
	 * Generally means the main play area for a game. Zones usually get put on top of these (or
	 * the spatial map, sometimes. e.g. the side lines in chess where taken pieces end up)
	 */
	public class Board : MonoBehaviour, Gazeable, Draggable {
		
		protected Zone[] zones;
		public bool draggable = false;

		/*
		 * When the game scene loads, stores all attached Zones for convenience. (By 'attached' I
		 * mean the zones that you drag and drop onto the Board object in the hierarchy.)
		 */
		public void Start() {
			zones = GetComponents<Zone>();
		}

		public Zone[] GetZones() {
			Zone[] copy = new Zone[zones.length];
			for (int i = 0; i < zones.length; i++) {
				copy[i] = zones[i];
			}
			return copy;
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
	}
}
