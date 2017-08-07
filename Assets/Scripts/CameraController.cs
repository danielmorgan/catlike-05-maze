using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;

    public Vector3 offset;
    public float zoomSpeed = 5f;
    public float minZoom = 1f;
    public float maxZoom = 6f;
    public float pitch = 0.75f;

    private float currentZoom;

    private void Awake() {
        currentZoom = maxZoom - minZoom;
    }

    private void Update() {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    private void LateUpdate() {
        transform.position = target.position - (offset * currentZoom);
        transform.LookAt(target.position + (Vector3.up * pitch));
    }

    public void SetTarget(Transform newTarget, float zoom) {
        target = newTarget;
        currentZoom = zoom;
    }
}
