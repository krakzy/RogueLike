using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // Lijst van vijanden (Enemies)
    private List<Actor> enemies = new List<Actor>();

    // Speler (Player)
    public Actor Player { get; set; }

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
    public void StartEnemyTurn()
    {
        foreach (Actor enemy in enemies)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.RunAI(); // Roep de RunAI-functie van het Enemy-component aan
            }
        }
    }

    public static GameManager Get { get => instance; }

    // Methode om een vijand toe te voegen aan de lijst
    public void AddEnemy(Actor enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(Actor enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Destroy(enemy.gameObject);
            Debug.Log($"{enemy.name} has been removed.");
        }
        else
        {
            Debug.Log("Enemy not found in the list.");
        }
    }

    // Methode om een acteur te maken
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

    // Methode om de acteur op een bepaalde locatie op te halen
    public Actor GetActorAtLocation(Vector3 location)
    {
        // Check of de locatie overeenkomt met de positie van de speler
        if (Player != null && Player.transform.position == location)
        {
            return Player;
        }

        // Check of de locatie overeenkomt met de positie van een vijand
        foreach (Actor enemy in enemies)
        {
            if (enemy.transform.position == location)
            {
                return enemy;
            }
        }

        // Geen acteur gevonden op de gegeven locatie
        return null;
    }

}

