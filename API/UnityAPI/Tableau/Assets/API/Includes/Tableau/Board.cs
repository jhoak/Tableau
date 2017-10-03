using System;
using System.Collections;
using UnityEngine;

namespace Tableau.Base {

	public class Board : MonoBehaviour {
        // TODO comment
		// TODO make hoverable, clickable, draggable?? (Sounds + animations)
		// TODO clamp zones to proper places on scene start, when board is moved
		// TODO make sure Start can be easily redefined for multiplayer
		// TODO physics

		private Zone[] zones;

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
