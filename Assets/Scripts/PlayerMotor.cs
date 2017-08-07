using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour {

    private NavMeshAgent agent;

    public MeshRenderer movementMarkerPrefab;
    private MeshRenderer movementMarker;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveToPoint(Vector3 point) {
        if (movementMarker != null) {
            Destroy(movementMarker.gameObject);
        }
        
        movementMarker = Instantiate<MeshRenderer>(movementMarkerPrefab, point, Quaternion.identity) as MeshRenderer;
        agent.SetDestination(point);
    }

    private void Update() {
        if (movementMarker != null) {
            if (agent.remainingDistance < 0.2f) {
                Destroy(movementMarker.gameObject);
            }
        }
    }
}