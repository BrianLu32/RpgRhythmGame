using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public float timeStamp;
    public SynchronizerData.BeatValue beatValue;
    public bool hasTimeStamp;
    public int sampleSetIndex;

    // Start is called before the first frame update
    void Start()
    {
        // timeStamp = 0f;
        // beatValue = SynchronizerData.BeatValue.None;
    }
}
