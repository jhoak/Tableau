using System;
using System.Collections;
using UnityEngine;
using Tableau.Base;

namespace Checkers {

	public class Checkerboard : Board {

		public override void Setup() {
			base.Setup();
            // Auto-generate zones
            zones = new BasicZone[64];
			Vector3 pos = this.gameObject.transform.position;
			for (int i = 0; i < 8; i++) {
				for (int j = 0; j < 8; j++) {
					GameObject zoneCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    BasicZone zone = zoneCube.AddComponent<BasicZone>();
                    zoneCube.transform.parent = this.gameObject.transform;
                    this.zones[8 * i + j] = zone;
					zoneCube.transform.localPosition = new Vector3(i, 0, j);
				}
			}

		}

	}
}
