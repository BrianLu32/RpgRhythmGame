using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;
using System;

public class TimelineController : MonoBehaviour
{
    private Timeline timelineInstance;
    private TimelineCollider timelineCollider;
    public GameObject timelineColliderObject;
    [SerializeField] private AudioManager audioManager;
    private int currentSampleSetIndex = 0;

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

    public delegate void OnBeatValueChangeDelegate(int newVal);
    public event OnBeatValueChangeDelegate OnBeatValueChange;

    void Start()
    {
        lookAt = Camera.main.GetComponent<LookAt>();
        timelineInstance = GetComponent<Timeline>();
        timelineCollider = timelineColliderObject.GetComponent<TimelineCollider>();
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
        GameObject marker = timelineInstance.markerSets[currentSampleSetIndex];

        if((!altKey && !ctrlKey && scrollDirection != 0) || rightArrow || leftArrow) {
            // Prevent moving pass the beginning and end of timeline
            // Scrolling Down = -1, Scrolling Up = 1
            if(currentSampleSetIndex - 1 < 0 && scrollDirection > 0) { return; }
            if(currentSampleSetIndex > timelineInstance.sampleSets.Count && scrollDirection < 0) { return; }

            if(scrollDirection > 0) { currentSampleSetIndex--; }
            if(scrollDirection < 0) { currentSampleSetIndex++; }

            transform.position += new Vector3(2f * scrollDirection, 0f, 0f);

            if(rightArrow) transform.position += new Vector3(-2f, 0f, 0f);
            if(leftArrow) transform.position += new Vector3(2f, 0f, 0f);

            audioManager.PlayMusic(timelineInstance.sampleSets[currentSampleSetIndex], true);
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
        if(timelineCollider.isMouseOver && leftMouseButton && marker) {
            if(!marker.GetComponent<Marker>().hasTimeStamp) {
                Vector3 spawnPos = new(marker.transform.position.x, marker.transform.position.y + 4.0f, marker.transform.position.z);
                GameObject noteObject = Instantiate(sampleMarkerNote, spawnPos, Quaternion.identity, marker.transform);

                // float markerYScale = marker.GetComponent<Marker>().yScaleMultiplier;
                noteObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                marker.GetComponent<Marker>().hasTimeStamp = true;
                audioManager.AddHitSoundSource(marker.GetComponent<Marker>().sampleSetIndex);
            }
        }
        if(timelineCollider.isMouseOver && rightMouseButton && marker) {

            if(marker.GetComponent<Marker>().hasTimeStamp) {
                GameObject noteObject = marker.transform.GetChild(0).gameObject;
                marker.GetComponent<Marker>().hasTimeStamp = false;
                Destroy(noteObject);
                audioManager.DestroyHitSoundSource(marker.GetComponent<Marker>().sampleSetIndex);
            }
        }
        if(spacebar) {
            audioManager.PlayMusic(timelineInstance.sampleSets[currentSampleSetIndex], false);
        }
        if(xKey) {
            audioManager.RestartMusic();
        }

        // Plays a sound when there is a time stamp
        // Needs to look ahead by one marker to schedule the sound to play on time
        if(timelineInstance.markerSets[currentSampleSetIndex + 1].GetComponent<Marker>().hasTimeStamp) {
            PlayHitSound(timelineInstance.markerSets[currentSampleSetIndex + 1]);
        }
        
        // Checks song position to move timeline
        if(audioManager.song.isPlaying) CheckForTime();
    }

    public void CheckForTime() {
        float songPosition = audioManager.convertSongPosToSamplePos(audioManager.song.time);
        if(songPosition >= timelineInstance.sampleSets[currentSampleSetIndex]) {
            transform.position += new Vector3(-2f, 0f, 0f);
            currentSampleSetIndex++;
        }
    }

    private void PlayHitSound(GameObject marker) {
        if(audioManager.song.isPlaying) audioManager.PlayHitSoundSource(marker);
    }

    /******* Start of Scrub Timeline Section *******/
    public void SetMarkerIndex(int index) {
        UpdateTimeLinePosition(index);
        currentSampleSetIndex = index;
    }
    private void UpdateTimeLinePosition(int newIndex) {
        // If this is negative, it will be traversing backwards, otherwise forwards
        int indexDifference = newIndex - currentSampleSetIndex;
        for(int i = 0; i < Math.Abs(indexDifference); i++) {
            if(indexDifference < 0) {
                transform.position += new Vector3(2f, 0f, 0f);
            }
            else {
                transform.position += new Vector3(-2f, 0f, 0f);
            }
        }
    }
    /******* End of Scrub Timeline Section *******/
}
