using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : MonoBehaviour {

    private Action<EventParam> listener1; 
    private Action<EventParam> listener2; 
    private Action<EventParam> listener3;

    private void Awake() {

        listener1 = new Action<EventParam>(Function1);
        listener2 = new Action<EventParam>(Function2);
        listener3 = new Action<EventParam>(Function3);

        StartCoroutine(invokeTest());
    }

    IEnumerator invokeTest() {
        WaitForSeconds waitTime = new WaitForSeconds(0.5f);

        //Create parameter to pass to the event
        EventParam eventParam = new EventParam();
        eventParam.param1 = "hello";
        eventParam.param2 = 12;
        eventParam.param3 = -42.88f;
        eventParam.param4 = false;

        while (true) {
            yield return waitTime;
            EventManager.TriggerEvent("test", eventParam);
            yield return waitTime;
            EventManager.TriggerEvent("move", eventParam);
            yield return waitTime;
            EventManager.TriggerEvent("time", eventParam);
        }
    }

    void OnEnable()
    {
        //Register With Action variable
        EventManager.StartListening("test", listener1);
        EventManager.StartListening("move", listener2);
        EventManager.StartListening("time", listener3);

        //OR Register Directly to function
        EventManager.StartListening("test", function1);
        EventManager.StartListening("move", function2);
        EventManager.StartListening("time", function3);
    }

    void OnDisable()
    {
        //Un-Register With Action variable
        EventManager.StopListening("test", listener1);
        EventManager.StopListening("move", listener2);
        EventManager.StopListening("time", listener3);

        //OR Un-Register Directly to function
        EventManager.StopListening("test", function1);
        EventManager.StopListening("move", function2);
        EventManager.StopListening("time", function3);
    }

    void function1(EventParam eventParam)
    {
        Debug.Log("function1 was called!");
    }

    void function2(EventParam eventParam)
    {
        Debug.Log("function2 was called!");
    }

    void function3(EventParam eventParam)
    {
        Debug.Log("function3 was called!");
    }
}
