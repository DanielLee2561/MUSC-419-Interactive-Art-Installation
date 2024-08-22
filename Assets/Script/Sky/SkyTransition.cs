using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State {
    Day,
    Sunset,
    Night,
    DayT,
    SunsetT,
    NightT
}

public class SkyTransition : MonoBehaviour {
    // OSC
    public OSC osc;
    public string oscSkyState;
    // Sky
    private Material sky;
    // Timer (General)
    private float timer;
    private float timerTransition;
    // Timer (Variable)
    private float timeDay = 3000f;
    private float timeSunset = 10f;
    private float timeNight = 10f;
    private float timeDayT = 10f;
    private float timeSunsetT = 10f;
    private float timeNightT = 10f;
    // State
    private State state;

    void Start() {
        // Initialize OSC
        if (osc) {
            osc.SetAddressHandler(oscSkyState, skyState);
        }
        // Initialize Variables
        sky = RenderSettings.skybox;
        // Reset
        updateState(State.Day, timeDay);
    }

    // Sky state
    void skyState(OscMessage skyState) {
        State state = (State) skyState.GetInt(0);
        float timeout = skyState.GetFloat(1);
        updateState(state, timeout);
    }

    void Update()
    {
        // Update Timer
        timer += Time.deltaTime;

        // State (Day)
        if (state == State.Day) {
            if (timer > timerTransition) {
                updateState(State.DayT, timeDayT);
            }
        }

        // State (Day T)
        else if (state == State.DayT) {
            sky.SetFloat("_lerpDS", lerp(timeSunsetT));
            if (timer > timerTransition) {
                updateState(State.Sunset, timeSunset);
            }
        }

        // State (Sunset)
        else if (state == State.Sunset) {
            if (timer > timerTransition) {
                updateState(State.SunsetT, timeSunsetT);
            }
        }

        // State (SunsetT)
        else if (state == State.SunsetT) {
            sky.SetFloat("_lerpSN", lerp(timeSunsetT));
            if (timer > timerTransition) {
                updateState(State.Night, timeNight);
            }
        }

        // State (Night)
        else if (state == State.Night) {
            if (timer > timerTransition) {
                updateState(State.NightT, timeNightT);
            }
        }

        // State (NightT)
        else {
            sky.SetFloat("_lerpND", lerp(timeNightT));
            if (timer > timerTransition) {
                updateState(State.Day, timeDay);
            }
        }

        // Debug.Log("State: " + state + ", " + "Timer: " + timer); // DEBUG
    }

    void updateSky(float lerpDS, float lerpSN, float lerpND) {
        sky.SetFloat("_lerpDS", lerpDS);
        sky.SetFloat("_lerpSN", lerpSN);
        sky.SetFloat("_lerpND", lerpND);
    }
    void updateState(State state, float timeout) {
        this.state = state;
        timer = 0f;
        timerTransition = timeout;

        // Update Sky
        if (state == State.Day || state == State.DayT) {
            updateSky(0, 0, 0);
        } else if (state == State.Sunset || state == State.SunsetT) {
            updateSky(1, 0, 0);
        } else {
            updateSky(1, 1, 0);
        }
    }

    float lerp(float timerIncrement) {
        float min = 0f;
        float max = timerTransition;
        float lerp = Map(timer, min, max, 0, 1);
        return lerp;
    }

    float Map(float value, float min1, float max1, float min2, float max2) {
        return (value - min1) * (max2 - min2) / (max1 - min1) + min2;
    }
}
