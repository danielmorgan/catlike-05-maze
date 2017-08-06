using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeWall : MazeCellEdge {

    public Transform wall;

    public override void Initialize(MazeCell parentCell, MazeCell neighbourCell, MazeDirection direction) {
        base.Initialize(parentCell, neighbourCell, direction);
        wall.GetComponent<Renderer>().material = parentCell.room.settings.wallMaterial;
    }
}