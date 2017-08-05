using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Maze : MonoBehaviour {
    
    public IntVector2 size;

    public MazeCell cellPrefab;
    
    public float generationStepDelay;
    
    public MazePassage passagePrefab;

    public MazeDoor doorPrefab;

    [Range(0f, 1f)]
    public float doorProbability;
    
    public MazeWall[] wallPrefabs;

    private MazeCell[,] cells;

    public IEnumerator Generate() {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new MazeCell[size.x, size.z];
        List<MazeCell> activeCells = new List<MazeCell>();
        DoFirstGenerationStep(activeCells);
        while (activeCells.Count > 0) {
            yield return delay;
            DoNextGenerationStep(activeCells);
        }
    }

    private void DoFirstGenerationStep(List<MazeCell> activeCells) {
        activeCells.Add(CreateCell(RandomCoordinates));
    }

    private void DoNextGenerationStep(List<MazeCell> activeCells) {
        int currentIndex = activeCells.Count - 1;
        MazeCell currentCell = activeCells[currentIndex];
        if (currentCell.IsFullyInitialized) {
            activeCells.RemoveAt(currentIndex);
            return;
        }
        MazeDirection direction = currentCell.RandomUninitializedDirection;
        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
        if (ContainsCoordinates(coordinates)) {
            MazeCell neighbour = GetCell(coordinates);
            if (neighbour == null) {
                neighbour = CreateCell(coordinates);
                CreatePassage(currentCell, neighbour, direction);
                activeCells.Add(neighbour);
            }
            else {
                CreateWall(currentCell, neighbour, direction);
            }
        }
        else {
            CreateWall(currentCell, null, direction);
        }
    }

    private MazeCell CreateCell(IntVector2 coordinates) {
        MazeCell newCell = Instantiate(cellPrefab);
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(
            coordinates.x - size.x * 0.5f + 0.5f,
            0f,
            coordinates.z - size.z * 0.5f + 0.5f
        );
        return newCell;
    }

    private void CreatePassage(MazeCell parentCell, MazeCell neighbourCell, MazeDirection direction) {
        MazePassage prefab = Random.value < doorProbability ? doorPrefab : passagePrefab;
        MazePassage passage = Instantiate(prefab);
        passage.Initialize(parentCell, neighbourCell, direction);
        passage = Instantiate(prefab);
        passage.Initialize(neighbourCell, parentCell, direction.GetOpposite());
    }

    private void CreateWall(MazeCell parentCell, MazeCell neighbourCell, MazeDirection direction) {
        MazeWall wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]);
        wall.Initialize(parentCell, neighbourCell, direction);
        if (neighbourCell != null) {
            wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]);
            wall.Initialize(neighbourCell, parentCell, direction.GetOpposite());
        }
    }

    public bool ContainsCoordinates (IntVector2 coordinate) {
        return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
    }

    public MazeCell GetCell(IntVector2 coordinates) {
        return cells[coordinates.x, coordinates.z];
    }

    public IntVector2 RandomCoordinates {
        get { return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z)); }
    }
}