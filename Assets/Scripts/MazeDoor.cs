using UnityEngine;

[RequireComponent(typeof(Transform))]
public class MazeDoor : MazePassage {

    public Transform hinge;

    private MazeDoor OtherSideOfDoor {
        get { return neighbourCell.GetEdge(direction.GetOpposite()) as MazeDoor; }
    }

    public override void Initialize(MazeCell parentCell, MazeCell neighbourCell, MazeDirection direction) {
        base.Initialize(parentCell, neighbourCell, direction);
        if (OtherSideOfDoor != null) {
            hinge.localScale = new Vector3(-1f, 1f, 1f);
            Vector3 p = hinge.localPosition;
            p.x = -p.x;
            hinge.localPosition = p;
        }
    }
}