using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExportBeatmap : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameObject timelineObject;
    private Timeline timelineInstance;

    void Start () {
        timelineInstance = timelineObject.GetComponent<Timeline>();
    }

    public void Export() {
        // Path for file
        string path = Application.dataPath + "/SongExports/" + audioManager.song.clip.name + ".txt";
        StreamWriter writer = File.AppendText(path);

        foreach(var marker in timelineInstance.markerSets) {
            if(marker.GetComponent<Marker>().hasTimeStamp) {
                writer.WriteLine(marker.GetComponent<Marker>().timeStamp + "\n");
            }
        }
        writer.Close();
    }
}
