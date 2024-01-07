using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;

public class TimelineController : MonoBehaviour
{
    private Timeline timeline;
    private int beatValueInt;
    public int NewBeatValueInt {
        get { return beatValueInt; }
        set {
            if(beatValueInt == value) return;
            beatValueInt = value;
            OnVariableChange?.Invoke(beatValueInt);
        }
    }
    public delegate void OnVariableChangeDelegate(int newVal);
    public event OnVariableChangeDelegate OnVariableChange;

    void Start()
    {
        beatValueInt = 5;
        timeline = GetComponent<Timeline>();
    }

    void Update()
    {
        float scrollDirection = Input.mouseScrollDelta.y;
        bool altKey = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
        bool ctrlKey = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        if(!altKey && !ctrlKey && scrollDirection != 0) {
            // Negate y direction for scrolling sideways; scroll right (+), scroll left (-)
            transform.position += new Vector3(2f * -scrollDirection, 0f, 0f);
        }
        if(altKey && scrollDirection != 0) {
            transform.position += new Vector3(0f, 0f, 1f * scrollDirection);
        }
        if(ctrlKey && scrollDirection != 0) {

            // Add or subtract by one to ignore dotted notes for now
            if(scrollDirection > 0) {
                NewBeatValueInt = (int)(NewBeatValueInt - scrollDirection - 1) < 1 ? 1 : (int)(NewBeatValueInt - scrollDirection - 1);
                // beatValueInt = (int)(beatValueInt - scrollDirection - 1);
                // if(beatValueInt < 1) beatValueInt = 1;
                // timeline.timelineInstance.currentBeatValue = (BeatValue)beatValueInt; 
            }
            if(scrollDirection < 0) {
                NewBeatValueInt = (int)(NewBeatValueInt + -scrollDirection + 1) > 9 ? 9 : (int)(NewBeatValueInt + -scrollDirection + 1);
                // beatValueInt = (int)(beatValueInt + -scrollDirection + 1);
                // if(beatValueInt > 10) beatValueInt = 9;
                // timeline.timelineInstance.currentBeatValue = (BeatValue)beatValueInt; 
            }
        }
    }
}
