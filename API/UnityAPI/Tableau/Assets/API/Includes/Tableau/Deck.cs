using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Tableau.Base {

    public class Deck : TableauObject {
        [SerializeField]
        private Card[] deck;
        private bool draggable = false;

        [SerializeField]
        public int deckSize;

        private int cardsUsed;

        public override void Setup() {
            // no need to do anything
        }

        public Deck(Card[] cards) {
            deck = cards;
        }

        public void Shuffle() {
            for(int i = deck.Length - 1; i > 0; i--) {
                int rand = UnityEngine.Random.Range(0, i + 1);
                Card temp = deck[i];
                deck[i] = deck[rand];
                deck[rand] = temp;
            }
        }

        public int CardsLeft() {
            return deck.Length - cardsUsed;
        }

        public Card DealCard() {
            if (CardsLeft() == 0) {
                Debug.LogWarning("There are no cards left in the deck.");
            }
            cardsUsed++;
            return deck[cardsUsed - 1];
        }

        public void ResetDeck() {
            cardsUsed = 0;
        }

        public override bool Equals(GameObject o) {
            try {
                Piece p = o.GetComponent<Piece>();
                return p == this;
            } catch (Exception x) {
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
        public override void OnGazeEnter(CursorEvent e) { }

        public override void OnGazeExit(CursorEvent e) { }

        public override void OnTapEnter(CursorEvent e) { }

        public override void OnTapExit(CursorEvent e) { }

        public override void WarnIfOversized() {
            Vector3 size = new Vector3(-1, -1, -1);
            try {
                size = GetComponent<Renderer>().bounds.size;
            } catch (Exception x) {
                try {
                    size = GetComponent<Collider>().bounds.size;
                } catch (Exception y) {
                    // don't do anything...we'll let the programmer figure it out at this point
                }
            } finally {
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

        public override string Serialize() {
            int id = this.GetInstanceID();
            Vector3 pos = this.gameObject.transform.position;
            string[] posVals = { pos.x + "", pos.y + "", pos.z + "" };
            Quaternion rot = this.gameObject.transform.rotation;
            string[] rotVals = { rot.w + "", rot.x + "", rot.y + "", rot.z + "" };
            string cardIds = "";
            foreach (Card c in deck) {
                cardIds += c.GetInstanceID() + ",";
            }
            cardIds = cardIds.Substring(0, cardIds.Length - 1);
            return String.Format(
                "id={0},drag={1},pos={2},rot={3},cardsUsed={4},cards={5};",
                id,
                draggable,
                String.Join(":", posVals),
                String.Join(":", rotVals),
                cardsUsed,
                cardIds
            );
        }

        public static Deck DeserializeDeck(string serializedObject) {
            Deck[] decks = GameObject.FindObjectsOfType<Deck>();
            Match m = Regex.Match(
                serializedObject,
                "^id=(.*?),drag=(.*?),pos=(.*?),rot=(.*?),cardsUsed=(.*?),cards=(.*?);$"
            );
            int id = int.Parse(m.Groups[1].Value);
            foreach (Deck d in decks) {
                if (d.GetInstanceID() == id) {
                    d.draggable = bool.Parse(m.Groups[2].Value);
                    d.cardsUsed = int.Parse(m.Groups[5].Value);

                    // now break down pos, rot, and cards
                    string[] posVals = m.Groups[3].Value.Split(':');
                    d.gameObject.transform.position = new Vector3(
                        float.Parse(posVals[0]),
                        float.Parse(posVals[1]),
                        float.Parse(posVals[2])
                    );
                    string[] rotVals = m.Groups[4].Value.Split(':');
                    d.gameObject.transform.rotation = new Quaternion(
                        float.Parse(rotVals[0]),
                        float.Parse(rotVals[1]),
                        float.Parse(rotVals[2]),
                        float.Parse(rotVals[3])
                    );
                    string[] cardIds = m.Groups[6].Value.Split(',');
                    d.deck = new Card[cardIds.Length];
                    int addedCards = 0;
                    Card[] cardsInScene = GameObject.FindObjectsOfType<Card>();
                    foreach (string cid in cardIds) {
                        foreach (Card c in cardsInScene) {
                            if (int.Parse(cid) == c.GetInstanceID()) {
                                d.deck[addedCards] = c;
                                addedCards++;
                                break;
                            }
                        }
                    }
                    return d;
                }
            }
            return null;
        }
    }
}
