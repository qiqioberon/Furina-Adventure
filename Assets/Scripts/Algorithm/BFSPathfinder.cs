using System.Collections.Generic;
using UnityEngine;

/// <summary> Breadth-First Search pathfinding algorithm </summary>
[RequireComponent(typeof(Actor))]
public class BFSPathfinder : MonoBehaviour {
    // Semua 8 arah (termasuk diagonal)
    private static readonly Vector2Int[] Directions = {
        new Vector2Int( 1,  0), new Vector2Int(-1,  0),
        new Vector2Int( 0,  1), new Vector2Int( 0, -1),
        new Vector2Int( 1,  1), new Vector2Int( 1, -1),
        new Vector2Int(-1,  1), new Vector2Int(-1, -1)
    };

    /// <summary> Finds the first step direction toward goal using BFS </summary>
    /// <param name="start">Start cell</param>
    /// <param name="goal">Goal cell</param>
    /// <returns>Unit step direction from start toward goal, or Vector2.zero jika tidak ada jalur</returns>
    public Vector2 Compute(Vector2Int start, Vector2Int goal) {
        var queue = new Queue<Node>();
        var visited = new HashSet<Vector2Int>();
        Node goalNode = null;
        Debug.Log($"BFSPathfinder.Compute: Start={start}, Goal={goal}");
        // Inisialisasi
        var startNode = new Node(start);
        queue.Enqueue(startNode);
        visited.Add(start);

        // BFS
        while (queue.Count > 0) {
            var current = queue.Dequeue();
            if (current.position == goal) {
                goalNode = current;
                break;
            }

            foreach (var dir in Directions) {
                var nextPos = current.position + dir;
                if (visited.Contains(nextPos)) continue;
                if (!MapManager.Instance.FloorMap.GetTile((Vector3Int)nextPos)) continue;
                
                var neighbor = new Node(nextPos) { parent = current };
                queue.Enqueue(neighbor);
                visited.Add(nextPos);
            }
        }

        if (goalNode == null) {
            Debug.Log("No path found (BFS)");
            return Vector2.zero;
        }

        // Bangun kembali jalur ke start
        var path = new List<Vector2Int>();
        var node = goalNode;
        while (node.position != start) {
            path.Add(node.position);
            node = node.parent;
        }
        path.Reverse();

        // Ambil langkah pertama
        var nextStep = path.Count > 0 ? path[0] : start;
        var stepDir = new Vector2(nextStep.x - start.x, nextStep.y - start.y);

        // Cek apakah ada actor blocking
        if (GameManager.Instance.GetBlockingActorAtLocation(transform.position + (Vector3)stepDir))
            return Vector2.zero;

        return stepDir;
    }
}
