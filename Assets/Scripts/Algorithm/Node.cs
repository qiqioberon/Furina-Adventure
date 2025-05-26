// Node.cs
using UnityEngine;

/// <summary> Simple node for BFS </summary>
public class Node {
    public Vector2Int position { get; set; }
    public Node parent { get; set; }

    public Node(Vector2Int position) {
        this.position = position;
        this.parent = null;
    }
}
