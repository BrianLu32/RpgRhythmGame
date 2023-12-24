using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiNoteName = Melanchall.DryWetMidi.MusicTheory.NoteName;

public class Configs : MonoBehaviour
{
    private static readonly KeyCode[] inputKeyCodes = { KeyCode.Z, KeyCode.M };
    private static readonly MidiNoteName[] noteRestrictions = { MidiNoteName.A, MidiNoteName.G };

    public static KeyCode GetKeyCode(int index) {
        return inputKeyCodes[index];
    }

    public static MidiNoteName GetMidiNoteName(int index) {
        return noteRestrictions[index];
    }
}
