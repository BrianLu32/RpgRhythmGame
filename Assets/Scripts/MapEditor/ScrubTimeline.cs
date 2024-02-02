using System.Collections;
using System.Collections.Generic;
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
            timeline.ScrubMusic((int)currentValue);
            SetMarkerPosition(currentValue);
            SetAudioTimeText((int)currentValue);
        });

        audioTime.text = "00:00";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaxValue(int maxValue) {
        slider.maxValue = maxValue;
    }

    public void SetSliderPosition(int currentValueInSamples) {
        slider.SetValueWithoutNotify(currentValueInSamples);
        SetAudioTimeText(currentValueInSamples);
    }

    private void SetAudioTimeText(int currentValueInSamples) {
        float timeInSeconds = currentValueInSamples / audioManager.song.clip.frequency;
        // Debug.Log(timeInSeconds / 60f);
        int mintues = (int)timeInSeconds / 60;
        float seconds = timeInSeconds % 60f;
        audioTime.text = mintues.ToString("00") + ":" + seconds.ToString("00");
    }

    private void SetMarkerPosition(float currentValueInSamples) {
        float smallestDifference = 10000f;
        int closestIndex = 0;
        for(int i = 0; i < timeline.sampleSets.Count; i++) {
            if(Mathf.Abs(currentValueInSamples - timeline.sampleSets[i]) < smallestDifference) {
                closestIndex = i;
            }
        }
        timelineController.SetMarkerIndex(closestIndex);
        // timelineController.CheckForTime();
    }
}
