using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineCollider : MonoBehaviour
{
    private BoxCollider boxCollider;

    public bool isMouseOver = false;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        float scaler = Camera.main.aspect;
        boxCollider.size = new Vector3(4f * scaler, 1f, 1f);
    }

    void OnMouseOver() {
        isMouseOver = true;
    }

    void OnMouseExit() {
        isMouseOver = false;
    }
}
