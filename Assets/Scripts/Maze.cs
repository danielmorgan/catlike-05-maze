using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {
    public IntVector2 size;
    public float generationStepDelay;

    public MazeCell cellPrefab;
    public MazePassage passagePrefab;
    public MazeWall wallPrefab;

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
        activeCells.Add(CreateCell(RandomCoords));
    }

    private void DoNextGenerationStep(List<MazeCell> activeCells) {
        int currentIndex = activeCells.Count - 1;
        MazeCell currentCell = activeCells[currentIndex];
        MazeDirection direction = MazeDirections.RandomValue;
        IntVector2 coords = currentCell.coords + direction.ToIntVector2();

        // Inside maze
        if (ContainsCoords(coords)) {
            MazeCell neighbour = GetCell(coords);

            // Not hit anything
            if (neighbour == null) {
                neighbour = CreateCell(coords);
                CreatePassage(currentCell, neighbour, direction);
                activeCells.Add(neighbour);
            }
            // Hit existing cell
            else {
                CreateWall(currentCell, neighbour, direction);
                activeCells.RemoveAt(currentIndex);
            }
        }
        // Outside
        else {
            CreateWall(currentCell, null, direction);
            activeCells.RemoveAt(currentIndex);
        }
    }

    private MazeCell CreateCell(IntVector2 coords) {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;

        newCell.coords = coords;
        newCell.name = "Maze Cell " + coords.x + ", " + coords.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(
            coords.x - size.x * 0.5f + 0.5f,
            0f,
            coords.z - size.z * 0.5f + 0.5f
        );

        cells[coords.z, coords.z] = newCell;

        return newCell;
    }

    private void CreatePassage(MazeCell parentCell, MazeCell neighbourCell, MazeDirection direction) {
        MazePassage passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(parentCell, neighbourCell, direction);
        passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(neighbourCell, parentCell, direction.GetOpposite());
    }

    private void CreateWall(MazeCell parentCell, MazeCell neighbourCell, MazeDirection direction) {
        MazeWall wall = Instantiate(wallPrefab) as MazeWall;
        wall.Initialize(parentCell, neighbourCell, direction);

        if (neighbourCell != null) {
            wall = Instantiate(wallPrefab) as MazeWall;
            wall.Initialize(neighbourCell, parentCell, direction.GetOpposite());
        }
    }

    public bool ContainsCoords(IntVector2 coords) {
        return coords.x >= 0 &&
               coords.z >= 0 &&
               coords.x < size.x &&
               coords.z < size.z;
    }

    public MazeCell GetCell(IntVector2 coords) {
        return cells[coords.x, coords.z];
    }

    public IntVector2 RandomCoords {
        get { return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z)); }
    }
}