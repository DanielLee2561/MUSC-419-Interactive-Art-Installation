using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // Scale
    private float xMin;
    private float xMax;
    private float zMin;
    private float zMax;
    private float strengthMin;
    private float strengthMax;
    private float strengthScale;

    // Variable
    private GameObject rippleObject;
    private int id;
    private Vector3 position;
    private float strength;
    private float lifetime;

    void Start() {
        // Initialize Variable
        rippleObject = GameObject.Find("Ripple");
        position = new Vector3(0f, 0f, 0f);
        strength = 0f;
        lifetime = 10f;

        // Scale
        xMin = -45f;
        xMax = 45f;
        zMin = -20f;
        zMax = 20f;
        strengthMin = 0f;
        strengthMax = 9f;
        strengthScale = 0.5f;
    }

    public void update(int id, float x, float z, float strength) {
        this.id = id;
        position.x = Mathf.Lerp(xMin, xMax, x / 100);
        position.z = Mathf.Lerp(zMin, zMax, z / 100);
        this.strength = scaleStrength(strength);
    }

    public void generateRipple() {
        GameObject ripple = Instantiate(rippleObject);
        configureRipple(ripple);
        Destroy(ripple, lifetime);
    }

    private void configureRipple(GameObject ripple) {
        // Variable
        ParticleSystem particle = ripple.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particle.main;

        // Configure
        ripple.name = "Ripple " + id;
        ripple.transform.position = position;
        main.startSize = strength;
    }

    private float scaleStrength(float strength) {
        // Variable
        float strengthMaxB = Mathf.Pow(100f, strengthScale);
        float strengthB = Mathf.Pow(strength, strengthScale);

        // Scale (non-linear)
        return Mathf.Lerp(strengthMin, strengthMax, strengthB / strengthMaxB);
    }
}
