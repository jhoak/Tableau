using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPanel : MonoBehaviour {

    private string toDisplay;
    private TextMesh mesh;
    private bool changed;

	public void Start() {
        toDisplay = "Waiting to initialize...";
        mesh = this.GetComponentInParent<TextMesh>();
        mesh.text = toDisplay;
        changed = false;
    }

    public void UpdateText(string newText) {
        changed = !toDisplay.Equals(newText);
        toDisplay = newText;
    }
	
	public void Update () {
		if (changed) {
            mesh.text = toDisplay;
            changed = false;
        }
	}
}
