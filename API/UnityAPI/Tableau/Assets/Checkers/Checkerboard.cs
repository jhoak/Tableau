using System;
using System.Collections;
using UnityEngine;
using Tableau.Base;

namespace Checkers {

	public class Checkerboard : Board {

		void Start() {
			base.Start();
			// Auto-generate zones
			Vector3 pos = this.gameObject.position;
			for (int i = 0; i < 8; i++) {
				for (int j = 0; j < 8; j++) {
					GameObject zoneCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					this.zones[i][j] = zoneCube.AddComponent(typeof(BasicZone));
					zoneCube.position = new Vector3(pos.x + i, pos.y, pos.z + j);
				}
			}

		}

	}
}
