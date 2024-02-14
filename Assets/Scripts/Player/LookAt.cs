using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    private GameObject hitGameObject;
    private int interactableLayer;
    private int markerLayer;
    private Vector3 originalScale;
    private float scaleSize = 1.15f;
    private float returnSpeed = 3f;
    // private bool alreadyLooking = false;
    // Start is called before the first frame update
    void Start()
    {
        interactableLayer = LayerMask.NameToLayer("Interactable");
        markerLayer = LayerMask.NameToLayer("Marker");
        // originalScale = new Vector3(1f,1f,1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject InteractRayCastMarker() {
        RaycastHit[] allHits;
        Vector3 playerPosition = transform.position;
        Vector3 forwardDirection = transform.forward;

        // Ray interactionRay = new(playerPosition, forwardDirection);
        float interactionRayLength = 50.0f;

        Vector3 interactionRayEndpoint = forwardDirection * interactionRayLength;
        Debug.DrawLine(playerPosition, interactionRayEndpoint);

        allHits = Physics.RaycastAll(transform.position, transform.forward, 50f);
        foreach(var hit in allHits) {
            hitGameObject = hit.transform.gameObject;
            if(hitGameObject.layer == markerLayer)
                return hitGameObject;
        }
        return null;

        // bool hitFound = Physics.Raycast(interactionRay, out RaycastHit interactionRayHit, interactionRayLength, markerLayer);
        // if(hitFound) {
        //     hitGameObject = interactionRayHit.transform.gameObject;
        //     // Debug.Log(hitGameObject.name);
        //     if(hitGameObject.layer == interactableLayer) {
        //         if(originalScale.Equals(new Vector3(0f,0f,0f))) { originalScale = hitGameObject.transform.localScale; }
        //         hitGameObject.transform.localScale = originalScale * scaleSize;
        //     }
        //     return hitGameObject;
        // }
        // else {
        //     if(hitGameObject && hitGameObject.layer == interactableLayer) {
        //         hitGameObject.transform.localScale = Vector3.Lerp(hitGameObject.transform.localScale, originalScale, Time.deltaTime * returnSpeed);
        //     }
        // }
        // return null;
    }
}
