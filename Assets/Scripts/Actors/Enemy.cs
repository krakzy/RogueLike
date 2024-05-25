using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
[RequireComponent(typeof(AStar))]
public class Enemy : MonoBehaviour
{
    public Actor Target { get; set; }
    public bool IsFighting { get; set; } = false;
    public AStar Algorithm { get; private set; }

    private void Start()
    {
        GameManager.Get.AddEnemy(GetComponent<Actor>());

        // Initialize Algorithm
        Algorithm = GetComponent<AStar>();
        if (Algorithm == null)
        {
            Debug.LogError("AStar component is missing on the Enemy gameObject.");
            enabled = false; // Disable the script if AStar is not found to avoid errors
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RunAI();
    }

    public void MoveAlongPath(Vector3Int targetPosition)
    {
        if (Algorithm == null)
        {
            Debug.LogError("AStar algorithm is not initialized.");
            return;
        }

        Vector3Int gridPosition = MapManager.Get?.FloorMap?.WorldToCell(transform.position) ?? Vector3Int.zero;
        if (gridPosition == Vector3Int.zero)
        {
            Debug.LogError("MapManager or FloorMap is not initialized.");
            return;
        }

        Vector2 direction = Algorithm.Compute((Vector2Int)gridPosition, (Vector2Int)targetPosition);
        Action.Move(GetComponent<Actor>(), direction);
    }

    public void RunAI()
    {
        // Ensure GameManager is initialized and has a player
        if (GameManager.Get == null || GameManager.Get.Player == null)
        {
            Debug.LogError("GameManager or Player is not initialized.");
            return;
        }

        // Set target to player if null
        if (Target == null)
        {
            Target = GameManager.Get.Player;
        }

        // Convert the position of the target to a gridPosition
        Vector3Int gridPosition = MapManager.Get?.FloorMap?.WorldToCell(Target.transform.position) ?? Vector3Int.zero;
        if (gridPosition == Vector3Int.zero)
        {
            Debug.LogError("MapManager or FloorMap is not initialized.");
            return;
        }

        // Calculate distance between enemy and target
        float distance = Vector3.Distance(transform.position, Target.transform.position);

        // Check if target is within attacking range
        if (distance < 1.5f)
        {
            // Attack the target
            Action.Hit(GetComponent<Actor>(), Target);
        }
        else
        {
            // If the enemy was not fighting, it should be fighting now
            IsFighting = true;

            // Call MoveAlongPath with the gridPosition
            MoveAlongPath(gridPosition);
        }
    }
}


