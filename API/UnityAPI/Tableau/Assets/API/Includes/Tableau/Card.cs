using System;
using System.Collections;
using UnityEngine;

namespace Tableau.Base {

    /*
     * A very basic representation of a playing card.
     */
    public class Card : Piece {

        public void Start() {
            base.Start();
        }

        public override void WarnIfOversized() {
            Vector3 size = new Vector3(-1,-1,-1);
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
                if (size != new Vector3(-1, -1, -1) && (size.x > .0635 || size.y > .0889 || size.z > .01)) {
                    Debug.LogWarning(string.Format(
                        "This card might be too big (%d, %d, %d)!",
                        size.x,
                        size.y,
                        size.z
                    ));
                }
            }
        }
    }
}
