using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor), typeof(BFSPathfinder))]
public class Ai : MonoBehaviour
{
    [SerializeField] private BFSPathfinder pathfinder;
    public BFSPathfinder Pathfinder { get => pathfinder; set => pathfinder = value; }

    private void OnValidate() => pathfinder = GetComponent<BFSPathfinder>();

    public void MoveAlongPath(Vector3Int targetPos, float movementSpeed) {
        Vector3Int gridPos = MapManager.Instance.FloorMap.WorldToCell(transform.position);
        Vector2 dir = pathfinder.Compute((Vector2Int)gridPos, (Vector2Int)targetPos)
                      * movementSpeed * Time.deltaTime;
        Action.MovementAction(GetComponent<Actor>(), dir);
    }

    public virtual AiState SaveState() => new AiState();
}



[System.Serializable]
public class AiState {
    [SerializeField] private string type;
    public string Type { get => type; set => type = value; }
    public AiState(string type = "") {
        this.type = type;
    }
}