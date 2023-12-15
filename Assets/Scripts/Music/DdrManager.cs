using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DdrManager : MonoBehaviour
{
    public static DdrManager ddrManagerInstance;
    public int inputDelayInMilliseconds;
    public double marginOfErrorInSeconds;
    public float noteTime;
    public Vector3 noteSpawnPos;
    public Vector3 noteTapPos;
    public Lane[] lanes;

    public Vector3 noteDespawnPos {
        get { return noteTapPos - (noteSpawnPos - noteTapPos); }
    }

    // Start is called before the first frame update
    void Start()
    {
        ddrManagerInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
