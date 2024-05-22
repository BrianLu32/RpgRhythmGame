using System.Collections;
using System.Collections.Generic;
using SynchronizerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrubTimeline : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Slider slider;
    [SerializeField] private Timeline timeline;
    [SerializeField] private TimelineController timelineController;
    [SerializeField] private  TextMeshProUGUI audioTime;

    // Start is called before the first frame update
    void Start()
    {
        slider.onValueChanged.AddListener((currentValue) => {
            audioManager.ScrubMusic((int)currentValue);
            SetMarkerPosition(currentValue);
            SetAudioTimeText((int)currentValue);
        });

        audioTime.text = "00:00:000";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaxValue(float maxValue) {
        slider.maxValue = maxValue;
    }

    /// <summary>
    ///  Sets the slider value to the current song position in seconds without invoking slider listener
    /// </summary>
    /// <param name="currentSongPos">Measured in seconds</param>
    public void SetSliderPosition(float currentSongPos) {
        slider.SetValueWithoutNotify(currentSongPos);
        SetAudioTimeText(currentSongPos);
    }

    private void SetAudioTimeText(float currentSongPos) {
        int milliseconds = (int)currentSongPos % 1000;
        int mintues = (int)(currentSongPos / 1000) / 60;
        int seconds = (int)(currentSongPos / 1000) % 60;
        audioTime.text = mintues.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("000");
    }



    /******* Start of Timeline Controller Section *******/
    /// <summary>
    ///     Updates the index position of the timeline to adjusts timeline position 
    ///     basone on song position. 'newSongPos' is converted to seconds
    /// </summary>
    /// <param name="newSongPos"></param>
    private void SetMarkerPosition(float newSongPos) {
        float newSongPosInSamples = audioManager.convertSongPosToSamplePos(newSongPos / 1000);
        float closest = timeline.sampleSets[0];
        int closestIndex = 0;
        for(int i = 0; i < timeline.sampleSets.Count; i++) {
            if(Mathf.Abs(timeline.sampleSets[i] - newSongPosInSamples) < Mathf.Abs(closest - newSongPosInSamples)) {
                closestIndex = i;
            }
        }
        timelineController.SetMarkerIndex(closestIndex);
    }
    /******* End of Timeline Controller Section *******/
}
