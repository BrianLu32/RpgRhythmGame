using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using MidiNote = Melanchall.DryWetMidi.Interaction.Note;
using System.IO;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    [SerializeField] private float bpm;
    public int offset;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Intervals[] intervals;

    // Attempt of creating a chart
    public static MusicController musicControllerInstance;
    public float songDelayInSeconds;
    public string fileLocation;

    public static MidiFile midiFile;
    void Start() {
        musicControllerInstance = this;
        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    void Update() {
        foreach(Intervals _interval in intervals) {
            // Debug.Log("Music" + audioSource.timeSamples);
            float samepledTime = (audioSource.timeSamples + offset) / (audioSource.clip.frequency * _interval.getIntervalLength(bpm));
            _interval.checkForNewInterval(samepledTime);
            // Debug.Log(samepledTime);
        }
    }
 
    // private void ReadMidiFile() {
    //     midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
    //     ICollection<MidiNote> notes = midiFile.GetNotes();
    //     MidiNote[] array = new MidiNote[notes.Count];
    //     notes.CopyTo(array, 0);
        
    //     // DdrManager.ddrManagerInstance.SetTimestamps(array);
    //     // foreach(Lane lane in DdrManager.ddrManagerInstance.GetLanes()) lane.SetTimeStamps(array);

        
    // }   

    private void StartSong() {
        audioSource.Play();
    }

    public static double GetAudioSourceTime() {
        return (double)(musicControllerInstance.audioSource.timeSamples + musicControllerInstance.offset) / (musicControllerInstance.audioSource.clip.frequency);
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
