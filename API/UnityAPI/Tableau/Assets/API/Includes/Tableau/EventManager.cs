using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Tableau;

public class EventManager : MonoBehaviour {
    
    //we can change this from being a string, to maybe taking a class or struct to pass along parameters
    private Dictionary<string, Function> eventDictionary;

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
            eventDictionary = new Dictionary<string, Function>();
        }

    }

    public static void StartListening(string eventName, Function listener) {

        Function thisEvent;

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

    public static void StopListening(string eventName, Function listener) {
        if (eventManager == null) return;
        Function thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
            //remove event from existing one
            thisEvent -= listener;

            //update dictionary
            instance.eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(string eventName) {
        Function thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.Invoke();
        }
    }

}
