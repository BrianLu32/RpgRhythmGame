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
        controller.OnVariableChange += VariableChangeHandler;

        CreateTimelineMarkers(bpm, (int)currentBeatValue, offset);
    }

    private void VariableChangeHandler(int newVal) 
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
        for(int i = 0; i < audioSource.clip.length; i++) {
            sampleSets.Add(currentSample + sampleOffset);
            currentSample += samplePeriod;

            // sampleMarker.GetComponent<Marker>().timeStamp = currentSample + sampleOffset;
            // sampleMarker.GetComponent<Marker>().beatValue = currentBeatValue;
            GameObject marker = Instantiate(sampleMarker, sampleSpawnInterval, Quaternion.identity, transform);
            marker.GetComponent<Marker>().timeStamp = sampleSets[i];
            marker.name += i;
            
            switch(i % 4)
            {
                case 0:
                    marker.GetComponent<Marker>().beatValue = BeatValue.WholeBeat;
                    marker.GetComponent<SpriteRenderer>().color = Color.white;
                    break;
                case 1:
                    marker.GetComponent<Marker>().beatValue = BeatValue.QuarterBeat;
                    marker.GetComponent<SpriteRenderer>().color = Color.cyan;
                    break;
                case 2:
                    marker.transform.localScale = new Vector3(marker.transform.localScale.x, marker.transform.localScale.y * 1.5f, marker.transform.localScale.z);
                    marker.GetComponent<Marker>().beatValue = BeatValue.HalfBeat;
                    marker.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case 3:
                    marker.GetComponent<Marker>().beatValue = BeatValue.QuarterBeat;
                    marker.GetComponent<SpriteRenderer>().color = Color.cyan;
                    break;
                default:
                    break;
            }
            if(i % 16 == 0) {
                marker.transform.localScale = new Vector3(marker.transform.localScale.x, marker.transform.localScale.y * 2, marker.transform.localScale.z);
            }
            sampleSpawnInterval += new Vector3(2f, 0f, 0f);
        }
    }
}
