using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyRotation : MonoBehaviour {
    // OSC
    public OSC osc;
    public string oscInput;

    // Variable
    private Material sky;
    private float timer;
    private float timerUpdate;
    private float rotation;
    private float rotationUpdate;
    public float rotationTarget;

    void Start() {
        // OSC
        if (osc) {
            osc.SetAddressHandler(oscInput, set);
        }

        // Variable
        sky = RenderSettings.skybox;
        timer = 0f;
        timerUpdate = 0.05f;
        rotation = 0f;
        rotationUpdate = 0.3f;
        rotationTarget = 0f;
    }

    void Update() {
        // Update Timer
        timer += Time.deltaTime;

        // Update Rotation
        if (timer > timerUpdate) {
            rotation = Mathf.MoveTowards(rotation, rotationTarget, rotationUpdate);
            sky.SetFloat("_Rotation", rotation);
            timer = 0f;
        }

        // Debug.Log("Timer: " + timer + ", " + "Rotation: " + rotation + ", " + "Rotation Target: " + rotationTarget); // Debug
    }

    void set(OscMessage input) {
        rotationTarget = input.GetFloat(0);
    }
}
