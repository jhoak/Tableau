using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tableau.Base {
    public class EventManager : MonoBehaviour {
        
        //we can change this from being a string, to maybe taking a class or struct to pass along parameters
        private Dictionary<string, Action> eventDictionary;

        private static EventManager eventManager;

        public static EventManager instance {
            get {
                if (!eventManager) {
                    eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;
                    if (!eventManager) {
                        Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                    } else {
                        eventManager.Init();
                    }
                }
                 return eventManager;
            }
           
        }

        void Init() {

            if(eventDictionary == null) {
                eventDictionary = new Dictionary<string, Action>();
            }

        }

        public static void StartListening(string eventName, Action listener) {

            Action thisEvent;

            if(instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
                //add another event to existing one
                thisEvent += listener;
                //Update the dictionary
                instance.eventDictionary[eventName] = thisEvent;
            } else {
                //add event to dictionary
                thisEvent += listener;
                instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action listener) {
            if (eventManager == null) return;
            Action thisEvent;
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
                //remove event from existing one
                thisEvent -= listener;

                //update dictionary
                instance.eventDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName) {
            Action thisEvent = null;
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.Invoke();
                // OR USE instance.eventDictionary[eventName]();
            }
        }

        //can be set to anything depending on the game.
        public enum GameTurnState
    	{
    		ShufflingDeck,
    		DealingCards,
    		PlayingPlayerHand,
            ChangingTurn,
    		Complete
    	};

    }
}
