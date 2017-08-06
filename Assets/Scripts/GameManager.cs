using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Maze mazePrefab;

    public PlayerController playerPrefab;
    
    private PlayerController playerInstance;

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
        yield return StartCoroutine(mazeInstance.Generate());
        
        playerInstance = Instantiate(playerPrefab);
        playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
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