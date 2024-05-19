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

    [SerializeField] private ScrubTimeline scrubTimeline;

    void Start() {
        song = GetComponent<AudioSource>();
        UpdateScrubTimelineMaxValue();

        // Removes the initial delay when playing the song for the first time
        song.Play();
        song.Stop();
        song.time = 0;
    }

    void Update() {
        if(song.isPlaying) {
            UpdateScrubTimelinePosition();
        }
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

    public void PlayHitSoundSource(GameObject marker) {
        GameObject hitSoundSource = GameObject.Find(hitSoundBaseName + marker.GetComponent<Marker>().sampleSetIndex);
        float sampleSetPosInSec = (marker.GetComponent<Marker>().timeStamp - offset) / 1000f;
        hitSoundSource.GetComponent<AudioSource>().PlayScheduled(sampleSetPosInSec);
    }

    /******* Start of Timeline Controller Section *******/
    public void PlayMusic(float currentSampleSetPos, bool isControllerScrolling) {
        if(song.isPlaying && !isControllerScrolling) {
            song.Pause();
        }
        else if(song.isPlaying && isControllerScrolling) {
            float songPosInSec = (currentSampleSetPos / 1000) - offset;
            song.time = songPosInSec;
            song.Pause();
            song.Play();
        }
        else if(!song.isPlaying && isControllerScrolling) {
            float songPosInSec = (currentSampleSetPos - offset) / 1000;
            song.time = songPosInSec;
        }
        else {
            song.Play();
        }
    }

    public void RestartMusic() {
        song.timeSamples = 0;
    }
    /******* End of Timeline Controller Section *******/



    /******* Start of Scrub Timeline Section *******/
    /// <summary>
    ///     Sets song position in seconds
    /// </summary>
    /// <param name="newSongPos"></param>
    public void ScrubMusic(float newSongPos) {
        song.time = newSongPos;
    }

    private void UpdateScrubTimelineMaxValue()
    {
        scrubTimeline.SetMaxValue(song.clip.length);
    }

    private void UpdateScrubTimelinePosition() {
        scrubTimeline.SetSliderPosition(song.time);
    }
    /******* End of Scrub Timeline Section *******/
}
