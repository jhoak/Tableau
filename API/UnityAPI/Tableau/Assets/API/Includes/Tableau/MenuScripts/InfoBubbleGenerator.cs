using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBubbleGenerator : MonoBehaviour {

    public float offset = 0.5f;
    private GameObject bubbleObject;
    private GameObject otherCameraObject;

	// Use this for initialization
	void Start() {
        // Initialize text bubble
        bubbleObject = new GameObject();
        TextMesh mesh = bubbleObject.AddComponent<TextMesh>();
        MeshRenderer renderer = bubbleObject.AddComponent<MeshRenderer>();
        InfoBubble bubble = bubbleObject.AddComponent<InfoBubble>();

        // Also find the other camera for easy access
        Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
        GameObject guess = cameras[0].gameObject;
        otherCameraObject = (this.gameObject != guess) ? guess : cameras[1].gameObject;
    }

    void Update() {
        // Update position based on this camera's position
        Vector3 thisPos = this.gameObject.transform.position;
        bubbleObject.transform.position = new Vector3(thisPos.x, thisPos.y + offset, thisPos.z);

        // Update rotation based on other camera's position
        Vector3 otherPos = otherCameraObject.transform.position;
        Vector3 lookVec = new Vector3(0, otherPos.y - thisPos.y, otherPos.z - thisPos.z);
        bubbleObject.transform.rotation = Quaternion.LookRotation(lookVec);
    }
}
