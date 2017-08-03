using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

    public int sizeX, sizeZ;
    public float generationStepDelay;

    public MazeCell cellPrefab;
    private MazeCell[,] cells;

    public IEnumerator Generate() {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);

        cells = new MazeCell[sizeX, sizeZ];

        for (int x = 0; x < sizeX; x++) {
            for (int z = 0; z < sizeZ; z++) {
                yield return delay;
                CreateCell(x, z);
            }
        }
    }

    private void CreateCell (int x, int z) {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        newCell.name = "Maze Cell " + x + ", " + z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(
            x - sizeX * 0.5f + 0.5f,
            0f,
            z - sizeZ * 0.5f + 0.5f
        );

        cells[x, z] = newCell;
    }
}
