using System;
using System.Collections;
using UnityEngine;

namespace Tableau.Base {

	public class Board : MonoBehaviour {
		// TODO make hoverable, clickable, draggable?? (Sounds + animations)
		// TODO clamp zones to proper places on scene start, when board is moved
		// TODO make sure "OnSceneStart" can be easily redefined for multiplayer
		
		public Zone[] zones;
	}
}
