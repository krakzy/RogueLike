using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // Lijst van vijanden (Enemies)
    private List<Actor> enemies = new List<Actor>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static GameManager Get { get => instance; }

    public Actor GetActorAtLocation(Vector3 location)
    {
        return null;
    }

    // Methode om een vijand toe te voegen aan de lijst
    public void AddEnemy(Actor enemy)
    {
        enemies.Add(enemy);
    }

    // Methode om een vijand te verwijderen uit de lijst
    public void RemoveEnemy(Actor enemy)
    {
        enemies.Remove(enemy);
    }
    public GameObject CreateActor(string name, Vector3 position)
    {
        GameObject actor = Instantiate(Resources.Load<GameObject>($"Prefabs/{name}"), position, Quaternion.identity);
        actor.name = name;
        return actor;
    }


    // Methode om de lijst van vijanden op te halen
    public List<Actor> GetEnemies()
    {
        return enemies;
    }

}
