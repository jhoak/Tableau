using System;
using System.Collections;
using UnityEngine;

namespace Tableau.Base {

	/*
	 * Generally means the main play area for a game. Zones usually get put on top of these (or
	 * the spatial map, sometimes. e.g. the side lines in chess where taken pieces end up)
	 */
	public class Board : MonoBehaviour {
		
		protected Zone[] zones;

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
	}
}
