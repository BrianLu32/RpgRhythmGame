using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager scoreManagerInstance;
    private AudioClip hitSfx;
    private AudioClip missSfx;
    private GameObject hitAudioSource;
    private GameObject missAudioSource;
    // public TMPro.TextMeshProUGUI scoreText;
    static int comboScore;

    // Start is called before the first frame update
    void Start()
    {
        scoreManagerInstance = this;
        comboScore = 0;

        // Load in SFX
        hitSfx = Resources.Load<AudioClip>("Sfx/hit");
        missSfx = Resources.Load<AudioClip>("Sfx/miss");

        hitAudioSource = new("Hit");
        hitAudioSource.transform.parent = transform;
        hitAudioSource.AddComponent<AudioSource>();
        hitAudioSource.GetComponent<AudioSource>().clip = hitSfx;
        hitAudioSource.GetComponent<AudioSource>().volume = 0.1f;

        missAudioSource = new("Miss");
        missAudioSource.transform.parent = transform;
        missAudioSource.AddComponent<AudioSource>();
        missAudioSource.GetComponent<AudioSource>().clip = missSfx;
        missAudioSource.GetComponent<AudioSource>().volume = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        // scoreText.text = comboScore.ToString();
    }

    public static void Hit() {
        comboScore++;
        scoreManagerInstance.hitAudioSource.GetComponent<AudioSource>().Play();
    }

    public static void Miss() {
        comboScore = 0;
        scoreManagerInstance.missAudioSource.GetComponent<AudioSource>().Play();
    }
}
