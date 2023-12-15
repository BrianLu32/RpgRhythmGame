using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager scoreManagerInstance;
    public AudioSource hitSfx;
    public AudioSource missSfx;
    // public TMPro.TextMeshProUGUI scoreText;
    static int comboScore;

    // Start is called before the first frame update
    void Start()
    {
        scoreManagerInstance = this;
        comboScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // scoreText.text = comboScore.ToString();
    }

    public static void Hit() {
        comboScore++;
        scoreManagerInstance.hitSfx.Play();
    }

    public static void Miss() {
        comboScore = 0;
        scoreManagerInstance.missSfx.Play();
    }
}
