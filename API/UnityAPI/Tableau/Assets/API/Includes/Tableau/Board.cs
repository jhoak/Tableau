using System;
using System.Collections;
using UnityEngine;

namespace Tableau.Base {

    /*
     * Generally means the main play area for a game. Zones usually get put on top of these (or
     * the spatial map, sometimes. e.g. the side lines in chess where taken pieces end up)
     */
    public class Board : TableauObject {

        protected Zone[] zones;
        public bool draggable = false;

        /*
         * When the game scene loads, stores all attached Zones for convenience. (By 'attached' I
         * mean the zones that you drag and drop onto the Board object in the hierarchy.)
         */
         override
        public void Start() {
            base.Start();
            zones = GetComponents<Zone>();
        }

        public Zone[] GetZones() {
            Zone[] copy = new Zone[zones.Length];
            for (int i = 0; i < zones.Length; i++) {
                copy[i] = zones[i];
            }
            return copy;
        }

        override
        public void OnDragStart(CursorEvent e) {
            if (draggable) {
                Vector3 cursorPosition = e.cursorPosition;
                gameObject.transform.position = cursorPosition;
            }
        }

        override
        public void OnDragEnd(CursorEvent e) {
            // do nothing (basically just stop moving)
        }

        // do nothing on gaze or tap (can be overridden, of course)
        override
        public void OnGazeEnter(CursorEvent e) {}
        
        override
        public void OnGazeExit(CursorEvent e) {}

        override
        public void OnTapEnter(CursorEvent e) {}

        override
        public void OnTapExit(CursorEvent e) {}

        override
        public void WarnIfOversized() {

            Vector3 size = new Vector3(0, 0, 0);

            try {
                size = GetComponent<Renderer>().bounds.size;
            }
            catch (Exception x) {
                try {
                    size = GetComponent<Collider>().bounds.size;
                }
                catch (Exception ex) {
                    // don't do anything...we'll let the programmer figure it out at this point
                }
            }
            finally {
                // Note: 1 unit corresponds to 1 meter in the real world, here
                if (size != null && (size.x > 1 || size.y > 1 || size.z > 1)) {
                    Debug.LogWarning(
                        "This board might be too big (" + size.x + ", " + size.y + ", " + size.z + ")!"
                    );
                }
            }
        }
    }
}
