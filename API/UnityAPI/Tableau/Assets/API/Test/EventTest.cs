using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EventTest : MonoBehaviour {

    private Func<void> listener1; 
    private Func<void> listener2; 
    private Func<void> listener3;

    private void Awake() {

        listener1 = new Action(function1);
        listener2 = new Action(function2);
        listener3 = new Action(function3);

        StartCoroutine(invokeTest());
    }

    IEnumerator invokeTest() {
        WaitForSeconds waitTime = new WaitForSeconds(0.5f);
        
        while (true) {
            yield return waitTime;
            EventManager.TriggerEvent("test");
            yield return waitTime;
            EventManager.TriggerEvent("move");
            yield return waitTime;
            EventManager.TriggerEvent("time");
        }
    }

    void OnEnable()
    {
        //Register Directly to function
        EventManager.StartListening("test", function1);
        EventManager.StartListening("move", function2);
        EventManager.StartListening("time", function3);
    }

    void OnDisable()
    {
        //Un-Register Directly to function
        EventManager.StopListening("test", function1);
        EventManager.StopListening("move", function2);
        EventManager.StopListening("time", function3);
    }

    void function1()
    {
        Debug.Log("function1 was called!");
    }

    void function2()
    {
        Debug.Log("function2 was called!");
    }

    void function3()
    {
        Debug.Log("function3 was called!");
    }
}
