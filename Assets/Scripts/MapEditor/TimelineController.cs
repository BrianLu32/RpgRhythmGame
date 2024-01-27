using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;
using System;

public class TimelineController : MonoBehaviour
{
    Timeline timelineInstance;
    private int sampleSetIndex = 0;

    private int lastInterval;

    private LookAt lookAt;
    private int beatValueInt;
    public int NewBeatValueInt {
        get { return beatValueInt; }
        set {
            if(beatValueInt == value) return;
            beatValueInt = value;
            OnBeatValueChange?.Invoke(beatValueInt);
        }
    }
    public GameObject sampleMarkerNote;
    public float timelineMarkerArraySize;

    public delegate void OnBeatValueChangeDelegate(int newVal);
    public event OnBeatValueChangeDelegate OnBeatValueChange;

    void Start()
    {
            lookAt = Camera.main.GetComponent<LookAt>();
        timelineInstance = GetComponent<Timeline>();
        beatValueInt = 5;
    }

    void Update()
    {
        // For scrubing through GO timeline
        float scrollDirection = Input.mouseScrollDelta.y;
        bool rightArrow = Input.GetKeyDown(KeyCode.RightArrow);
        bool leftArrow = Input.GetKeyDown(KeyCode.LeftArrow);

        // For GO timeline editting, zoom and beat type
        bool altKey = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
        bool ctrlKey = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        // For time stamping what positions there should be a beat -> Place/Remove
        bool leftMouseButton = Input.GetMouseButtonDown(0);
        bool rightMouseButton = Input.GetMouseButtonDown(1);

        // Play, Pause, Resume GO timeline
        bool spacebar = Input.GetKeyDown(KeyCode.Space);
        bool xKey = Input.GetKeyDown(KeyCode.X);

        // Determines what current timestamp the song is at
        GameObject marker = lookAt.InteractRayCast();

        if((!altKey && !ctrlKey && scrollDirection != 0) || rightArrow || leftArrow) {
            // Prevent moving pass the beginning and end of timeline
            if(marker.name == "Marker 0" && scrollDirection > 0) return;
            if(marker.name == "Marker " + (timelineMarkerArraySize - 1) && scrollDirection < 0) return;
            transform.position += new Vector3(2f * scrollDirection, 0f, 0f);
            if(rightArrow) transform.position += new Vector3(-2f, 0f, 0f);
            if(leftArrow) transform.position += new Vector3(2f, 0f, 0f);

            // Update sample set index for timeline movement
            // sampleSetIndex = marker.GetComponent<Marker>().sampleSetIndex;

            //If audio is playing
            timelineInstance.PlayMusic(marker.GetComponent<Marker>().timeStamp, true);
        }
        if(altKey && scrollDirection != 0) {
            transform.position += new Vector3(0f, 0f, 1f * scrollDirection);
        }
        if(ctrlKey && scrollDirection != 0) {
            // Add or subtract by one to ignore dotted notes for now
            if(scrollDirection > 0) {
                NewBeatValueInt = (int)(NewBeatValueInt - scrollDirection - 1) < 1 ? 1 : (int)(NewBeatValueInt - scrollDirection - 1);
            }
            if(scrollDirection < 0) {
                NewBeatValueInt = (int)(NewBeatValueInt + -scrollDirection + 1) > 9 ? 9 : (int)(NewBeatValueInt + -scrollDirection + 1);
            }
        }
        if(leftMouseButton && marker) {
            if(!marker.GetComponent<Marker>().hasTimeStamp) {
                Vector3 spawnPos = new(marker.transform.position.x, marker.transform.position.y + 4.0f, marker.transform.position.z);
                GameObject noteObject = Instantiate(sampleMarkerNote, spawnPos, Quaternion.identity, marker.transform);

                // float markerYScale = marker.GetComponent<Marker>().yScaleMultiplier;
                noteObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                marker.GetComponent<Marker>().hasTimeStamp = true;
            }
            // Destroy(noteObject);
        }
        if(rightMouseButton && marker) {

            if(marker.GetComponent<Marker>().hasTimeStamp) {
                GameObject noteObject = marker.transform.GetChild(0).gameObject;
                marker.GetComponent<Marker>().hasTimeStamp = false;
                Destroy(noteObject);
            }
        }
        if(spacebar) {
            // TODO: Will need to implment a check when scrubing timeline or
            // when GO timeline is moving and does not land on a marker
            timelineInstance.PlayMusic(marker.GetComponent<Marker>().timeStamp, false);
        }
        if(xKey) {
            timelineInstance.RestartMusic();
        }

        // Plays a sound when there is a time stamp
        if(marker.GetComponent<Marker>().hasTimeStamp) {
            PlayHitSound();
        }
        
        // Checks song position to move timeline
        // if(timelineInstance.audioSource.isPlaying) CheckForTime();
        if(timelineInstance.audioSource.isPlaying) CheckForTime(marker.GetComponent<Marker>().sampleSetIndex);
    }

    private void CheckForTime(int index) {
        // if(timelineInstance.audioSource.timeSamples < ((int)timelineInstance.sampleSets[sampleSetIndex] + 1000) && 
        //    timelineInstance.audioSource.timeSamples > ((int)timelineInstance.sampleSets[sampleSetIndex] - 1000)) {
        //     sampleSetIndex++;
        //     transform.position += new Vector3(-2f, 0f, 0f);
        // }
        float sampledTime = (timelineInstance.audioSource.timeSamples + timelineInstance.offset) / (timelineInstance.samplePeriod);
        if(Mathf.FloorToInt(sampledTime) != lastInterval) {
            lastInterval = Mathf.FloorToInt(sampledTime);
            transform.position += new Vector3(-2f, 0f, 0f);
        }
    }

    private void PlayHitSound() {
        if(timelineInstance.audioSource.isPlaying) timelineInstance.hitSound.Play();
    }

    /******* Start of Scrub Timeline Section *******/
    public void SetMarkerIndex(int index) {
        sampleSetIndex = index;
    }
    /******* End of Scrub Timeline Section *******/
}
