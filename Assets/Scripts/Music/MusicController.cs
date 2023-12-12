using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    [SerializeField] private float bpm;
    public int offset;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Intervals[] intervals;

    // Attempt of creating a chart
    public static MusicController musicControllerInstance;
    public int inputDelayInMilliseconds;
    public string fileLocation;
    public float noteTime;
    public float noteSpawnY;
    public float noteTapY;

    public float noteDespawnY {
        get { return noteTapY - (noteSpawnY - noteTapY); }
    }

    void Update() {
        foreach(Intervals _interval in intervals) {
            // Debug.Log("Music" + audioSource.timeSamples);
            float samepledTime = (audioSource.timeSamples + offset) / (audioSource.clip.frequency * _interval.getIntervalLength(bpm));
            _interval.checkForNewInterval(samepledTime);
        }
    }
}

[System.Serializable]
public class Intervals {
    [SerializeField] private float steps;
    [SerializeField] private UnityEvent trigger;
    private int lastInterval;

    public float getIntervalLength(float bpm) {
        return 60f / (bpm * steps);
    }

    public void checkForNewInterval(float interval) {
        if(Mathf.FloorToInt(interval) != lastInterval) {
            lastInterval =Mathf.FloorToInt(interval);
            trigger.Invoke();
        }
    }
}
