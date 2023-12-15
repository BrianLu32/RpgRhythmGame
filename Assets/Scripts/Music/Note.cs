using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public double timeInstantiated;
    public float assignedTime; 
    // Start is called before the first frame update
    void Start()
    {
        timeInstantiated = MusicController.GetAudioSourceTime();
    }

    // Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = MusicController.GetAudioSourceTime() - timeInstantiated;

        // Note time accounts for the time between the spawnY and the tapY
        // Want the time between spawnY and tapY
        float time = (float)(timeSinceInstantiated / (DdrManager.ddrManagerInstance.noteTime * 2));

        if(time > 1) { Destroy(gameObject); }
        else { 
            transform.localPosition = Vector3.Lerp(
                DdrManager.ddrManagerInstance.noteSpawnPos, 
                DdrManager.ddrManagerInstance.noteDespawnPos, 
                time
            ); 
        }
    }
}
