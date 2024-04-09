using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;
using System;

public class Timeline : MonoBehaviour
{
    private TimelineController controller;
    [SerializeField] private ScrubTimeline scrubTimeline;
    [SerializeField] private AudioManager audioManager;

    private float secPerBeat;
    public float samplePeriod;
    private float sampleRate;
    public float samplesPerTick;

    public BeatValue currentBeatValue = (BeatValue)5; // quarter note interval

    public List<float> sampleSets = new();

    [SerializeField] private GameObject sampleMarker;
    private Vector3 sampleSpawnInterval = new(0f, 0f, 5f);

    void Start()
    {
        /* MAYBE TODO: Consider AudioSettings.dspTime for consistent measurement of timing regardless of when the music starts */
        sampleRate = AudioSettings.outputSampleRate;
        // Debug.Log(sampleRate); //48000
        // Debug.Log(AudioSettings.dspTime);
        // Debug.Log("Samples Per Tick: " + sampleRate * 60f / bpm);
        // Debug.Log("Sample: " + AudioSettings.dspTime * sampleRate);

        secPerBeat = 60f / audioManager.bpm;

        controller = GetComponent<TimelineController>();
        controller.OnBeatValueChange += BeatValueChangeHandler;

        CreateTimelineMarkers(audioManager.bpm, (int)currentBeatValue, audioManager.offset);
        UpdateScrubTimelineMaxValue();
    }

    void Update() {
        UpdateScrubTimelinePosition();
    }

    private void BeatValueChangeHandler(int newVal) 
    {
        int timelineChildCount = gameObject.transform.childCount;
        for(int i = 0; i < timelineChildCount; i++) {
            GameObject marker =  transform.GetChild(i).gameObject;
            if(marker.GetComponent<Marker>().beatValue < (BeatValue)newVal) {
                marker.SetActive(false);
            }
            else {
                marker.SetActive(true);
            }
        }
    }

    private void CreateTimelineMarkers(float audioBpm, int beatValue, float offset)
    {
        AudioClip song = audioManager.song.clip;

        // Need to remove indexing in the future
        int index = 0;
        for(float i = 0; i < song.length; i += secPerBeat) {
            sampleSets.Add((i * 1000) + offset);

            GameObject marker = Instantiate(sampleMarker, sampleSpawnInterval, Quaternion.identity, transform);
            Marker markerScript = marker.GetComponent<Marker>();
            SpriteRenderer markerSpriteRenderer = marker.GetComponent<SpriteRenderer>();
            Vector3 currentMarkerScale = marker.transform.localScale;

            markerScript.timeStamp = (i * 1000) + offset;
            markerScript.sampleSetIndex = index;
            marker.name = "Marker " + index;
            switch(index % 4)
            {
                case 0:
                    markerScript.beatValue = BeatValue.WholeBeat;
                    markerSpriteRenderer.color = Color.white;
                    break;
                case 1:
                    markerScript.beatValue = BeatValue.QuarterBeat;
                    markerSpriteRenderer.color = Color.cyan;
                    break;
                case 2:
                    marker.transform.localScale = new Vector3(currentMarkerScale.x, currentMarkerScale.y * 1.5f, currentMarkerScale.z);
                    markerScript.beatValue = BeatValue.HalfBeat;
                    markerSpriteRenderer.color = Color.red;
                    break;
                case 3:
                    markerScript.beatValue = BeatValue.QuarterBeat;
                    markerSpriteRenderer.color = Color.cyan;
                    break;
                default:
                    break;
            }
            if(index % 16 == 0) {
                marker.transform.localScale = new Vector3(currentMarkerScale.x, currentMarkerScale.y * 2, currentMarkerScale.z);
            }
            sampleSpawnInterval += new Vector3(2f, 0f, 0f);
            index++;
        }
    }
    /******* End of Timeline Section *******/



    /******* Start of Scrub Timeline Section *******/
    private void UpdateScrubTimelineMaxValue()
    {
        scrubTimeline.SetMaxValue(audioManager.song.clip.samples);
    }

    public void ScrubMusic(int currentTimesample) {
        audioManager.song.timeSamples = currentTimesample;
    }

    private void UpdateScrubTimelinePosition() {
        scrubTimeline.SetSliderPosition(audioManager.song.timeSamples);
    }
     /******* End of Scrub Timeline Section *******/
}
