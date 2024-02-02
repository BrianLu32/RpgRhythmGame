using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource song;
    public float bpm;
    public float offset;

    [SerializeField] private AudioClip hitSound;
    private readonly string hitSoundBaseName = "Hit Source ";

    void Start() {
        song = GetComponent<AudioSource>();
    }

    public void AddHitSoundSource(int hitSoundNumber) {
        GameObject newHitSoundSource = new(hitSoundBaseName + hitSoundNumber);
        newHitSoundSource.AddComponent<AudioSource>().clip = hitSound;
        newHitSoundSource.GetComponent<AudioSource>().volume = 0.2f;
        newHitSoundSource.transform.parent = transform;
    }

    public void DestroyHitSoundSource(int hitSoundNumber) {
        GameObject hitSoundSource = GameObject.Find(hitSoundBaseName + hitSoundNumber);
        Destroy(hitSoundSource);
    }

    public void PlayHitSoundSource(int hitSoundNumber) {
        GameObject hitSoundSource = GameObject.Find(hitSoundBaseName + hitSoundNumber);
        hitSoundSource.GetComponent<AudioSource>().Play();
    }

    /******* Start of Timeline Controller Section *******/
    public void PlayMusic(float currentTimeStamp, bool isControllerScrolling) {
        if(song.isPlaying && !isControllerScrolling) {
            song.Pause();
        }
        else if(song.isPlaying && isControllerScrolling) {
            song.timeSamples = (int)currentTimeStamp;
            song.Pause();
            song.Play();
        }
        else if(!song.isPlaying && isControllerScrolling) {
            song.timeSamples = (int)currentTimeStamp;
        }
        else {
            song.Play();
        }
    }

    public void RestartMusic() {
        song.timeSamples = 0;
    }
    /******* End of Timeline Controller Section *******/
}
