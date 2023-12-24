using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using MidiNote = Melanchall.DryWetMidi.Interaction.Note;

public class DdrManager : MonoBehaviour
{
    public static DdrManager ddrManagerInstance;
    public static MidiFile midiFile;
    private readonly string fileLocation = "DreamerTest.mid";

    public int inputDelayInMilliseconds;
    public double marginOfErrorInSeconds;
    public float noteTime;
    public Vector3 noteSpawnPos;
    public Vector3 noteTapPos;

    public int numberOfLanes;
    private readonly List<GameObject> lanes = new();
    private readonly Vector3[] lanePositions = { new(-1f, 0f, 0f), new(1f, 0f, 0f) };

    private GameObject judgementLine;

    public Vector3 NoteDespawnPos {
        get { return noteTapPos - (noteSpawnPos - noteTapPos); }
    }

    // Start is called before the first frame update
    void Start()
    {
        ddrManagerInstance = this;
        judgementLine = Resources.Load<GameObject>("Prefabs/LanePrefabs/JudgementLine");

        // for(int i = 0; i < numberOfLanes; i++) {
        //    GameObject newLane = Resources.Load<GameObject>("Prefabs/LanePrefabs/Lane");
        //     newLane.name = "Lane" + i;
        //     newLane.GetComponent<Lane>().input = inputKeyCodes[i];
        //     newLane.GetComponent<Lane>().noteRestriction = noteRestrictions[i];
        //     lanes.Add(newLane);
        //     Debug.Log(newLane.name);
        //     Instantiate(newLane, lanePositions[i], Quaternion.Euler(0, 0, 0));
        // }
        LoadLanes();
        // LoadMusicController();
        LoadScoreMananger();
        Instantiate(judgementLine, transform);

        ReadMidiFile();
    }

    private void LoadLanes() {
        for(int i = 0; i < numberOfLanes; i++) {
            GameObject newLane = new("Lane" + i);
            newLane.AddComponent<Lane>();
            newLane.GetComponent<Lane>().input = Configs.GetKeyCode(i);
            newLane.GetComponent<Lane>().noteRestriction = Configs.GetMidiNoteName(i);
            newLane.transform.parent = transform;
            newLane.transform.position = lanePositions[i];
            lanes.Add(newLane);
        }
    }

    private void LoadMusicController() {
        GameObject musicController = new("MusicController");
        musicController.AddComponent<MusicController>();
        musicController.transform.parent = transform;
    }

    private void LoadScoreMananger() {
        GameObject scoreManager = new("ScoreManager");
        scoreManager.AddComponent<ScoreManager>();
        scoreManager.transform.parent = transform;
    }

    private void ReadMidiFile() {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        ICollection<MidiNote> notes = midiFile.GetNotes();
        MidiNote[] array = new MidiNote[notes.Count];
        notes.CopyTo(array, 0);

        SetTimestamps(array);
    }

    public void SetTimestamps(MidiNote[] array) {
        foreach(GameObject lane in lanes) {
            lane.GetComponent<Lane>().SetTimeStamps(array);
        }
    }
}
