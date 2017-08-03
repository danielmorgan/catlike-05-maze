using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeCellEdge : MonoBehaviour {
    public MazeCell parentCell, neighbourCell;
    public MazeDirection direction;

    public void Initialize(MazeCell parentCell, MazeCell neighbourCell, MazeDirection direction) {
        this.parentCell = parentCell;
        this.neighbourCell = neighbourCell;
        this.direction = direction;

        parentCell.SetEdge(direction, this);
        transform.parent = parentCell.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = direction.ToRotation();
    }
}