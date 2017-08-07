using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    public LayerMask movementMask;
    
    private Camera cam;

    private PlayerMotor motor;

    private void Start() {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
    }

    public void SetLocation(MazeCell cell) {
        transform.localPosition = cell.transform.localPosition;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, movementMask)) {
                motor.MoveToPoint(hit.point);
            }
        }
    }
}
