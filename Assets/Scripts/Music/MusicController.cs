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

    public List<float> timeStamps = new();
    public float step;

    // Attempt of creating a chart
    public static MusicController musicControllerInstance;
    public float songDelayInSeconds;

    public static MidiFile midiFile;
    void Start() {
        musicControllerInstance = this;

        // audioSource = Resources.Load<AudioSource>("Songs/AIKA_Starry_Eyed_Dreamer");

        Invoke(nameof(StartSong), songDelayInSeconds);

        // Debug.Log(audioSource.clip.samples);
        // Debug.Log(60 / (bpm * step) * audioSource.clip.frequency);
    }

    void Update() {
        // Debug.Log("Music: " + audioSource.timeSamples);
        foreach(Intervals _interval in intervals) {
            float samepledTime = (audioSource.timeSamples + offset) / (audioSource.clip.frequency * _interval.getIntervalLength(bpm));
            Debug.Log("Sampled Time: " + samepledTime);
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
            // Debug.Log(interval);
            trigger.Invoke();
        }
    }
}
