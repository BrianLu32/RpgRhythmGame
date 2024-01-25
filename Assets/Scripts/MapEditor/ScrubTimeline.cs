using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrubTimeline : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Timeline timeline;
    [SerializeField] private  TextMeshProUGUI audioTime;

    // Start is called before the first frame update
    void Start()
    {
        slider.onValueChanged.AddListener((currentValue) => {
            timeline.ScrubMusic((int)currentValue);
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
        slider.value = currentValueInSamples;
        SetAudioTimeText(currentValueInSamples);
    }

    private void SetAudioTimeText(int currentValueInSamples) {
        float timeInSeconds = currentValueInSamples / timeline.audioSource.clip.frequency;
        float mintues = timeInSeconds / 60f;
        float seconds = timeInSeconds % 60f;
        audioTime.text = mintues.ToString("00") + ":" + seconds.ToString("00");
    }
}
