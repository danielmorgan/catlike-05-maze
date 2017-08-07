using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Maze mazePrefab;

    public PlayerController playerPrefab;
    
    private PlayerController playerInstance;

    public CameraController cameraController;

    private Maze mazeInstance;

    private void Start () {
        StartCoroutine(BeginGame());
    }

    private void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            RestartGame();
        }
    }

    private IEnumerator BeginGame () {
        mazeInstance = Instantiate(mazePrefab);
        
        cameraController.SetTarget(mazeInstance.transform, cameraController.maxZoom);
        
        yield return StartCoroutine(mazeInstance.Generate());
        
        playerInstance = Instantiate(playerPrefab);
        playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

        cameraController.SetTarget(playerInstance.transform, cameraController.minZoom);
    }

    private void RestartGame () {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        if (playerInstance != null) {
            Destroy(playerInstance.gameObject);
        }
        StartCoroutine(BeginGame());
    }
}