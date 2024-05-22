using System.Collections;
using System.Collections.Generic;
using SynchronizerData;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource song;
    public float bpm;
    public float offset;

    [SerializeField] private AudioClip hitSound;
    private readonly string hitSoundBaseName = "Hit Source ";

    [SerializeField] private ScrubTimeline scrubTimeline;
    [SerializeField] private Timeline timeline;

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
        float sampleSetPosInSec = convertSamplePosToSongPos(marker.GetComponent<Marker>().timeStamp);
        hitSoundSource.GetComponent<AudioSource>().PlayScheduled(sampleSetPosInSec);
    }

    /******************* Helper Fuctions *******************/
        // Multiply by 1000 to convert to milliseconds and multiply by beat divisor for timings based on beatValue and finally plus offset
    public float convertSongPosToSamplePos(float songPos) {
        return (songPos * 1000f * (1 / BeatDecimalValues.values[(int)timeline.currentBeatValue])) + offset;
    }
    
    public float convertSamplePosToSongPos(float samplePos) {
        return (samplePos - offset) / (1 / BeatDecimalValues.values[(int)timeline.currentBeatValue]) / 1000f;
    }

    /******* Start of Timeline Controller Section *******/
    public void PlayMusic(float currentSampleSetPos, bool isControllerScrolling) {
        if(song.isPlaying && !isControllerScrolling) {
            song.Pause();
        }
        else if(song.isPlaying && isControllerScrolling) {
            float songPosInSec = convertSamplePosToSongPos(currentSampleSetPos);
            song.time = songPosInSec;
            song.Pause();
            song.Play();
        }
        else if(!song.isPlaying && isControllerScrolling) {
            float songPosInSec = convertSamplePosToSongPos(currentSampleSetPos);
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

    /// <summary>
    ///     Sets the slider max value.
    ///     Value is converted to miliseconds
    /// </summary>
    private void UpdateScrubTimelineMaxValue()
    {
        scrubTimeline.SetMaxValue(song.clip.length * 1000);
    }

    /// <summary>
    ///     Updates the slider value based on song position.
    ///     Value is converted to miliseconds
    /// </summary>
    private void UpdateScrubTimelinePosition() {
        scrubTimeline.SetSliderPosition(song.time * 1000);
    }
    /******* End of Scrub Timeline Section *******/
}
