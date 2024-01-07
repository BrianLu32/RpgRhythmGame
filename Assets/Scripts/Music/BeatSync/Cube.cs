using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;

public class Cube : MonoBehaviour
{
    private BeatObserver beatObserver;

    Vector3 initSpawnLoc;

    // Start is called before the first frame update
    void Start()
    {
        beatObserver = GetComponent<BeatObserver>();
        initSpawnLoc = new Vector3(0f, 0f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(beatObserver.beatMask & BeatType.DownBeat);
        if((beatObserver.beatMask & BeatType.DownBeat) == BeatType.DownBeat) {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = initSpawnLoc;
            initSpawnLoc += new Vector3(5f, 0f, 0f);
        }
    }
}
