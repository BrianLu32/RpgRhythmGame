using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using MidiNote = Melanchall.DryWetMidi.Interaction.Note;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public GameObject notePrefab;
    List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();

    int spawnIndex = 0;
    int inputIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnIndex < timeStamps.Count) {
            // Want to spawn our notes x time before player taps it
            if(MusicController.GetAudioSourceTime() >= timeStamps[spawnIndex] - MusicController.musicControllerInstance.noteTime) {
                GameObject note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;                                                                                                                                                                                                                                                           
            }
        }

        if(inputIndex < timeStamps.Count) {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = MusicController.musicControllerInstance.marginOfErrorInSeconds;
            double audioTime = MusicController.GetAudioSourceTime() - (MusicController.musicControllerInstance.inputDelayInMilliseconds / 1000f);

            if(Input.GetKeyDown(input)) {
                if(Math.Abs(audioTime - timeStamp) < marginOfError) {
                    Hit();
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
                else {
                    // I think this is less than perfect
                }
            }
            if(timeStamp + marginOfError <= audioTime) {
                Miss();
                inputIndex ++; 
            }
        }
    }

    public void SetTimeStamps(MidiNote[] array) {
        foreach(MidiNote note in array) {
            if(note.NoteName == noteRestriction) {
                MetricTimeSpan metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, MusicController.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }

    private void Hit() {
        ScoreManager.Hit();
    }

    private void Miss() {
        ScoreManager.Miss();
    }
}
