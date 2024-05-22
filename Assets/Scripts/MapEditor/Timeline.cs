using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;
using System;

public class Timeline : MonoBehaviour
{
    private TimelineController controller;
    [SerializeField] private AudioManager audioManager;

    private float secPerBeat;
    public BeatValue currentBeatValue = (BeatValue)1; // quarter note interval
    public List<float> sampleSets = new();
    public List<GameObject> markerSets = new();

    [SerializeField] private GameObject sampleMarker;
    private Vector3 sampleSpawnInterval = new(0f, 0f, 5f);

    void Start()
    {
        secPerBeat = 60f / audioManager.bpm;

        controller = GetComponent<TimelineController>();
        controller.OnBeatValueChange += BeatValueChangeHandler;

        CreateTimelineMarkers((int)currentBeatValue, audioManager.offset);
    }

    void Update() {
        
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

    private void CreateTimelineMarkers(int beatValue, float offset)
    {
        AudioClip song = audioManager.song.clip;

        // Need to remove indexing in the future
        int index = 0;
        for(float i = 0; i < song.length * BeatDecimalValues.values[beatValue]; i += secPerBeat) {
            sampleSets.Add(audioManager.convertSongPosToSamplePos(i));

            GameObject marker = Instantiate(sampleMarker, sampleSpawnInterval, Quaternion.identity, transform);
            Marker markerScript = marker.GetComponent<Marker>();
            SpriteRenderer markerSpriteRenderer = marker.GetComponent<SpriteRenderer>();
            Vector3 currentMarkerScale = marker.transform.localScale;

            markerScript.timeStamp = audioManager.convertSongPosToSamplePos(i);
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

            markerSets.Add(marker);
        }
    }
    /******* End of Timeline Section *******/
}
