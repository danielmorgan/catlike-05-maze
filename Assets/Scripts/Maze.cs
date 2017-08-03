using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

    public IntVector2 size;
    public float generationStepDelay;

    public MazeCell cellPrefab;
    private MazeCell[,] cells;

    public IEnumerator Generate() {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);

        cells = new MazeCell[size.x, size.z];

        IntVector2 coords = RandomCoords;
        while (ContainsCoords(coords) && GetCell(coords) == null) {
            yield return delay;
            CreateCell(coords);
            coords += MazeDirections.RandomValue.ToIntVector2();
        }
    }

    private void CreateCell (IntVector2 coords) {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        newCell.name = "Maze Cell " + coords.x + ", " + coords.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(
            coords.x - size.x * 0.5f + 0.5f,
            0f,
            coords.z - size.z * 0.5f + 0.5f
        );

        cells[coords.z, coords.z] = newCell;
    }

    public bool ContainsCoords (IntVector2 coords) {
        return coords.x >= 0 &&
               coords.z >= 0 &&
               coords.x < size.x &&
               coords.z < size.z;
    }

    public MazeCell GetCell (IntVector2 coords) {
        return cells[coords.x, coords.z];
    }

    public IntVector2 RandomCoords {
        get {
            return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
        }
    }
}
