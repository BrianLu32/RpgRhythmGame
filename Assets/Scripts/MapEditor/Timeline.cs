using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;
using System;

public class Timeline : MonoBehaviour
{
    // public Timeline timelineInstance;
    private TimelineController controller;
    public AudioSource audioSource;
    public bool pauseAudio;
    public float bpm;

    public BeatValue currentBeatValue = (BeatValue)5; // quarter note interval

    public float offset = 0f;
    public List<float> sampleSets = new();

    public GameObject sampleMarker;
    private Vector3 sampleSpawnInterval = new(0f, 0f, 5f);

    void Start()
    {
        // timelineInstance = this;
        controller = GetComponent<TimelineController>();
        controller.OnBeatValueChange += BeatValueChangeHandler;

        CreateTimelineMarkers(bpm, (int)currentBeatValue, offset);
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
        float samplePeriod = 60f / (audioBpm * BeatDecimalValues.values[beatValue]) * audioSource.clip.frequency;

        float sampleOffset = 0f;
        if(offset != 0f) {
            sampleOffset = 60f / (audioBpm * offset) * audioSource.clip.frequency;
            if(offset < 0f) {
                sampleOffset = samplePeriod - sampleOffset;
            }
        }

        float currentSample = samplePeriod;
        controller.timelineMarkerArraySize = audioSource.clip.length;
        for(int i = 0; i < audioSource.clip.length; i++) {
            sampleSets.Add(currentSample + sampleOffset);
            currentSample += samplePeriod;

            GameObject marker = Instantiate(sampleMarker, sampleSpawnInterval, Quaternion.identity, transform);
            Marker markerScript = marker.GetComponent<Marker>();
            SpriteRenderer markerSpriteRenderer = marker.GetComponent<SpriteRenderer>();
            Vector3 currentMarkerScale = marker.transform.localScale;

            markerScript.timeStamp = sampleSets[i];
            marker.name = "Marker " + i;
            switch(i % 4)
            {
                case 0:
                    markerScript.beatValue = BeatValue.WholeBeat;
                    // markerScript.yScaleMultiplier = 1.0f;
                    markerSpriteRenderer.color = Color.white;
                    break;
                case 1:
                    markerScript.beatValue = BeatValue.QuarterBeat;
                    // markerScript.yScaleMultiplier = 1.0f;
                    markerSpriteRenderer.color = Color.cyan;
                    break;
                case 2:
                    marker.transform.localScale = new Vector3(currentMarkerScale.x, currentMarkerScale.y * 1.5f, currentMarkerScale.z);
                    // markerScript.yScaleMultiplier = 1.5f;
                    markerScript.beatValue = BeatValue.HalfBeat;
                    markerSpriteRenderer.color = Color.red;
                    break;
                case 3:
                    markerScript.beatValue = BeatValue.QuarterBeat;
                    // markerScript.yScaleMultiplier = 1.0f;
                    markerSpriteRenderer.color = Color.cyan;
                    break;
                default:
                    break;
            }
            if(i % 16 == 0) {
                marker.transform.localScale = new Vector3(currentMarkerScale.x, currentMarkerScale.y * 2, currentMarkerScale.z);
                // markerScript.yScaleMultiplier = 2.0f;
            }
            sampleSpawnInterval += new Vector3(2f, 0f, 0f);
        }
    }

    public void PlayMusic(float currentTimeStamp, bool isControllerScrolling) {
        if(audioSource.isPlaying && !isControllerScrolling) {
            audioSource.Pause();
        }
        else if(audioSource.isPlaying && isControllerScrolling) {
            audioSource.timeSamples = (int)currentTimeStamp;
            audioSource.Pause();
            audioSource.Play();
        }
        else if(!audioSource.isPlaying && isControllerScrolling) {
            audioSource.timeSamples = (int)currentTimeStamp;
        }
        else {
            audioSource.Play();
        }
    }

    public void RestartMusic() {
        audioSource.timeSamples = 0;
    }
}
