// Ladder.cs
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] public bool up; // Indicates if the ladder goes up
    public bool Up { get { return up; } set { up = value; } }
    public Vector3 Position { get { return transform.position; } } // Property to store ladder position


    public bool IsUp
    {
        get { return up; }
        set { up = value; }
    }

    void Start()
    {
        // Add this ladder to the GameManager
        GameManager.Get.AddLadder(this);
    }

    void Update()
    {
        // Add any necessary update logic here
    }
}
