using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScript : MonoBehaviour
{
    [SerializeField] private BillboardType billboardType;
    public enum BillboardType { LookAtCamera, CameraForward };

    Vector3 originalRotation;

    float maxCameraDistance = 15f;

    void Awake() {
        originalRotation = transform.rotation.eulerAngles;
    }

    void LateUpdate() {
        switch(billboardType) {
            case BillboardType.LookAtCamera:
                transform.LookAt(Camera.main.transform.position, Vector3.up);
                break;
            case BillboardType.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            default:
                break;
        }
        // Check distance from Main Camera to either face player or reset orientation
        float distanceFromCamera = Vector3.Distance(Camera.main.transform.position, transform.position);
        if(distanceFromCamera > maxCameraDistance) {
            transform.rotation = Quaternion.Euler(new Vector3(originalRotation.x, originalRotation.y, originalRotation.z));
        }
        else {
            // Lock rotations 
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.x = originalRotation.x;
            transform.rotation = Quaternion.Euler(rotation);
        }        
    }
}
