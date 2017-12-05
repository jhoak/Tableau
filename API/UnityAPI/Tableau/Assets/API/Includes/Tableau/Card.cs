using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Tableau.Base {

    /*
     * A very basic representation of a playing card.
     */
    public class Card : Piece {

        public bool hidden = true;

        public void Start() {
            base.Start();
        }

        public void showCard(bool inp){
            hidden = inp;
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

        public string Serialize() {
            int id = this.GetInstanceID();
            return String.Format(
                "id={0},hidden={1};",
                id,
                hidden
            );
        }

        public static Card Deserialize(string serializedObject) {
            Card[] allCards = GameObject.FindObjectsOfType<Card>();
            Match m = Regex.Match(serializedObject, "^id=(.*?),hidden=(.*?);$");
            string id = m.Groups[1].Value,
                   hidden = m.Groups[2].Value;
            foreach (Card c in allCards) {
                if (c.GetInstanceID() == int.Parse(id)) {
                    c.hidden = bool.Parse(hidden);
                    return c;
                }
            }
            return null;
        }
    }
}
