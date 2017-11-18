using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tableau.Base {

    public class Deck : TableauObject {
        [SerializeField]
        private Card[] deck;
        private bool draggable = false;

        [SerializeField]
        public int deckSize;

        private int cardsUsed;

        // Use this for initialization
        void Start() {
            base.Start();
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
    }
}
