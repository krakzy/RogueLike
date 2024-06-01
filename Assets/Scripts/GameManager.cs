using Items;
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

    // Lijst van items (Consumables)
    private List<Consumable> items = new List<Consumable>();

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
        GameObject actorPrefab = Resources.Load<GameObject>($"Prefabs/{name}");
        if (actorPrefab == null)
        {
            Debug.LogError($"Prefab with name {name} not found in Prefabs folder.");
            return null;
        }

        GameObject actor = Instantiate(actorPrefab, position, Quaternion.identity);
        actor.name = name;

        // Check if the actor is the Player and assign it to the Player property
        if (name == "Player")
        {
            Player = actor.GetComponent<Actor>();
            if (Player == null)
            {
                Debug.LogError("The Player prefab does not have an Actor component.");
            }
            else
            {
                Debug.Log("Player has been initialized.");
            }
        }

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

    // Methode om een item toe te voegen aan de lijst
    public void AddItem(Consumable item)
    {
        items.Add(item);
        Debug.Log($"{item.name} has been added.");
    }

    // Methode om een item uit de lijst te verwijderen
    public void RemoveItem(Consumable item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Destroy(item.gameObject);
            Debug.Log($"{item.name} has been removed.");
        }
        else
        {
            Debug.Log("Item not found in the list.");
        }
    }

    // Methode om een item op een bepaalde locatie op te halen
    public Consumable GetItemAtLocation(Vector3 location)
    {
        foreach (Consumable item in items)
        {
            if (item.transform.position == location)
            {
                return item;
            }
        }

        // Geen item gevonden op de gegeven locatie
        return null;
    }

    // Nieuwe methode om vijanden binnen een straal van 5 eenheden te krijgen
    public List<Actor> GetNearbyEnemies(Vector3 location)
    {
        List<Actor> nearbyEnemies = new List<Actor>();
        foreach (Actor enemy in enemies)
        {
            if (Vector3.Distance(enemy.transform.position, location) < 5f)
            {
                nearbyEnemies.Add(enemy);
            }
        }
        return nearbyEnemies;
    }
}
