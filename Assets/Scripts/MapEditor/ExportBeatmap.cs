using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExportBeatmap : MonoBehaviour
{
    [SerializeField] private GameObject timelineObject;
    private Timeline timelineInstance;

    void Start () {
        timelineInstance = timelineObject.GetComponent<Timeline>();
    }

    public void Export() {
        foreach(var marker in timelineInstance.markerSets) {
            if(marker.GetComponent<Marker>().hasTimeStamp) {
                
            }
        }
    }
}
