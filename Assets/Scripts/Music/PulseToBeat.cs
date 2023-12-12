using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseToBeat : MonoBehaviour
{
    public AudioSource hitSound;

    [SerializeField] float pulseSize = 1.15f;
    [SerializeField] float returnSpeed = 5f;
    private Vector3 startSize;
    private Quaternion startRotate;
    private List<float> zRotations;    
    private int randomRotation;

    void Start() {
        startSize = transform.localScale;
        startRotate = transform.localRotation;
        zRotations = new List<float>
        {
            0f,
            -90f,
            90f,
            180f
        };
    }

    // Update is called once per frame
    void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale, startSize, Time.deltaTime * returnSpeed);
        randomRotation = Random.Range(0, zRotations.Count);
    }

    public void Pulse() {
        transform.localScale = startSize * pulseSize;
        if(hitSound) { hitSound.Play(); }
    }

    public void rotateImg() {
        transform.localRotation = Quaternion.Euler(0f, 0f, zRotations[randomRotation]);
    }
}
