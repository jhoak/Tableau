using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tableau.Base.Management;

public class CameraStart : MonoBehaviour {

    public float x, y, z;

	// Use this for initialization
	void Start () {
        MPGameManager mgr = GameObject.FindObjectOfType<MPGameManager>();
        if (!mgr.isServer) {
            this.gameObject.transform.position = new Vector3(x, y, z);
            this.gameObject.transform.Rotate(new Vector3(0, 180, 0));
        }
	}
}
