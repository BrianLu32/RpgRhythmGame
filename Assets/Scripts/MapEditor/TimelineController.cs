using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;

public class TimelineController : MonoBehaviour
{
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
        beatValueInt = 5;
    }

    void Update()
    {
        float scrollDirection = Input.mouseScrollDelta.y;
        bool altKey = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
        bool ctrlKey = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        bool leftMouseButton = Input.GetMouseButton(0);
        bool rightMouseButton = Input.GetMouseButton(1);

        GameObject marker = lookAt.InteractRayCast();

        if(!altKey && !ctrlKey && scrollDirection != 0) {
            // Prevent moving pass the beginning and end of timeline
            if(marker.name == "Marker 0" && scrollDirection > 0) return;
            if(marker.name == "Marker " + (timelineMarkerArraySize - 1) && scrollDirection < 0) return;
            transform.position += new Vector3(2f * scrollDirection, 0f, 0f);
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
    }
}
