using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBubble : MonoBehaviour {

    private Dictionary<string, string> info;
    private TextMesh mesh;
    private bool changed;

	public void Start() {
        info = new Dictionary<string, string>();
        mesh = this.GetComponentInParent<TextMesh>();
        mesh.text = "Waiting to initialize...";
        changed = false;
    }

    public void UpdateInfo(string attribute, string value) {
        string currentValue;
        bool inDict = info.TryGetValue(attribute, out currentValue);
        info.Add(attribute, value);
        changed = changed || !inDict || !currentValue.Equals(value);
    }

    public void Update() {
        if (changed) {
            string newText = "";
            foreach (string att in info.Keys)
            {
                string val;
                info.TryGetValue(att, out val);
                newText += att + ": " + val + "\n";
            }
            mesh.text = newText;
            changed = false;
        }
    }
}
