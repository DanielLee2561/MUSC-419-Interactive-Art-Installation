using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplayer : MonoBehaviour {
    // OSC
    public OSC osc;
    public string oscInput;
    
    // Variable
    public GameObject[] players;
    private int playersSize;

    void Start() {
        // OSC
        if (osc) {
            osc.SetAddressHandler(oscInput, player);
        }

        // Initialize Variable
        playersSize = 2;

        // Initialize Players
        players = new GameObject[playersSize];
        for (int index = 0; index < playersSize; index++) {
            players[index] = new GameObject("Player " + index);
            players[index].AddComponent<Player>();
        }
    }

    void player(OscMessage input) {
        // OSC
        int id = input.GetInt(0);
        float x = input.GetFloat(1);
        float z = input.GetFloat(2);
        float strength = input.GetFloat(3);

        // Ripple
        Player player = players[id].GetComponent<Player>();
        player.update(id, x, z, strength);
        player.generateRipple();
    }
}
