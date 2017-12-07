using System;
using System.Collections;
using System.Text.RegularExpressions;
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
        public override void Setup() {
            zones = GetComponents<Zone>();
        }

        public Zone[] GetZones() {
            Zone[] copy = new Zone[zones.Length];
            for (int i = 0; i < zones.Length; i++) {
                copy[i] = zones[i];
            }
            return copy;
        }

        public override bool Equals(GameObject o) {
            try {
                Board b = o.GetComponent<Board>();
                return b == this;
            }
            catch (Exception x) {
                return false;
            }
        }

        public override void OnDragStart(CursorEvent e) {
            if (draggable) {
                Vector3 cursorPosition = e.cursorPosition;
                gameObject.transform.position = cursorPosition;
            }
        }

        public override void OnDragEnd(CursorEvent e) {
            // do nothing (basically just stop moving)
        }

        // do nothing on gaze or tap (can be overridden, of course)
        public override void OnGazeEnter(CursorEvent e) {}

        public override void OnGazeExit(CursorEvent e) {}

        public override void OnTapEnter(CursorEvent e) {}

        public override void OnTapExit(CursorEvent e) {}

        public override void WarnIfOversized() {
            Vector3 size = new Vector3(-1, -1, -1);
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
                if (size != new Vector3(-1, -1, -1) && (size.x > 1 || size.y > 1 || size.z > 1)) {
                    Debug.LogWarning(string.Format(
                        "This board might be too big (%d, %d, %d)!",
                        size.x,
                        size.y,
                        size.z
                    ));
                }
            }
        }

        public override string Serialize() {
            int id = this.GetInstanceID();
            Vector3 pos = this.gameObject.transform.position;
            string[] posVals = { pos.x + "", pos.y + "", pos.z + "" };
            Quaternion rot = this.gameObject.transform.rotation;
            string[] rotVals = { rot.w + "", rot.x + "", rot.y + "", rot.z + "" };
            string zoneIds = "";
            foreach (Zone z in zones) {
                zoneIds += z.GetInstanceID() + ",";
            }
            zoneIds = zoneIds.Substring(0, zoneIds.Length - 1);
            return String.Format(
                "id={0},drag={1},pos={2},rot={3},zones={4};",
                id,
                draggable,
                String.Join(":", posVals),
                String.Join(":", rotVals),
                zoneIds
            );
        }

        public static Board DeserializeBoard(string serializedObject) {
            Board[] boards = GameObject.FindObjectsOfType<Board>();
            Match m = Regex.Match(
                serializedObject,
                "^id=(.*?),drag=(.*?),pos=(.*?),rot=(.*?),zones=(.*?);$"
            );
            int id = int.Parse(m.Groups[1].Value);
            foreach (Board B in boards) {
                if (B.GetInstanceID() == id) {
                    B.draggable = bool.Parse(m.Groups[2].Value);

                    // now break down pos, rot, and zones
                    string[] posVals = m.Groups[3].Value.Split(':');
                    B.gameObject.transform.position = new Vector3(
                        float.Parse(posVals[0]),
                        float.Parse(posVals[1]),
                        float.Parse(posVals[2])
                    );
                    string[] rotVals = m.Groups[4].Value.Split(':');
                    B.gameObject.transform.rotation = new Quaternion(
                        float.Parse(rotVals[0]),
                        float.Parse(rotVals[1]),
                        float.Parse(rotVals[2]),
                        float.Parse(rotVals[3])
                    );
                    string[] zoneIds = m.Groups[5].Value.Split(',');
                    B.zones = new Zone[zoneIds.Length];
                    int addedZones = 0;
                    Zone[] zonesInScene = GameObject.FindObjectsOfType<Zone>();
                    foreach (string zid in zoneIds) {
                        foreach (Zone z in zonesInScene) {
                            if (int.Parse(zid) == z.GetInstanceID()) {
                                B.zones[addedZones] = z;
                                addedZones++;
                                break;
                            }
                        }
                    }
                    return B;
                }
            }
            return null;
        }
    }
}
