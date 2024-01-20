using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BeatSynchronizer : MonoBehaviour
{
    public float bpm = 120f;		// Tempo in beats per minute of the audio clip.
	public float startDelay = 1f;	// Number of seconds to delay the start of audio playback.
	public delegate void AudioStartAction(double syncTime);
	public static event AudioStartAction OnAudioStart;
    // Start is called before the first frame update
    void Start()
    {
        // double initTime = AudioSettings.dspTime;
        // GetComponent<AudioSource>().PlayScheduled(initTime + startDelay);
        // OnAudioStart?.Invoke(initTime + startDelay);
    }
}
