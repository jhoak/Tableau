using System;
using System.Collections;
using UnityEngine;

namespace Tableau.Base {

	public class Board : MonoBehaviour {
		// TODO make hoverable, clickable, draggable?? (Sounds + animations)
		// TODO clamp zones to proper places on scene start, when board is moved
		// TODO make sure Start can be easily redefined for multiplayer

		private Zone[] zones;

		public void Start() {
			zones = GetComponents<Zone>();
		}

		public Zone[] GetZones() {
			return zones;
		}

		public void SetZones(Zone[] zs) {
			zones = zs;
		}
	}
}
