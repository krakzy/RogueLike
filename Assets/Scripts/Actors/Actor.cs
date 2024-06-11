using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private AdamMilVisibility algorithm;
    public List<Vector3Int> FieldOfView = new List<Vector3Int>();
    public int FieldOfViewRange = 8;

    [Header("Powers")]
    [SerializeField] private int maxHitPoints = 30;
    [SerializeField] private int hitPoints = 30;
    [SerializeField] private int defense;
    [SerializeField] private int power;
    [SerializeField] private int level = 1;
    [SerializeField] private int xp = 0;
    [SerializeField] private int xpToNextLevel = 100;

    public int MaxHitPoints => maxHitPoints;
    public int HitPoints => hitPoints;
    public int Defense => defense;
    public int Power => power;
    public int Level => level;
    public int XP => xp;
    public int XPToNextLevel => xpToNextLevel;

    private void Start()
    {
        algorithm = new AdamMilVisibility();
        UpdateFieldOfView();

        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
            UIManager.Instance.UpdateLevel(level);
            UIManager.Instance.UpdateXP(xp);
        }
    }

    private void Die()
    {
        if (GetComponent<Player>())
        {
            UIManager.Instance.AddMessage("You died!", Color.red);
        }
        else
        {
            UIManager.Instance.AddMessage($"{name} is dead!", Color.green);
            GameManager.Get.RemoveEnemy(this);
        }

        GameObject gravestone = GameManager.Get.CreateActor("Dead", transform.position);
        gravestone.name = $"Remains of {name}";

        Destroy(gameObject);
    }

    public void DoDamage(int hp, Actor attacker)
    {
        hitPoints -= hp;
        if (hitPoints < 0)
        {
            hitPoints = 0;
        }

        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
        }

        if (hitPoints == 0)
        {
            Die();
            if (attacker != null && attacker.GetComponent<Player>())
            {
                attacker.AddXP(xp);
            }
        }
    }

    public void Move(Vector3 direction)
    {
        if (MapManager.Get.IsWalkable(transform.position + direction))
        {
            transform.position += direction;
        }
    }

    public void UpdateFieldOfView()
    {
        var pos = MapManager.Get.FloorMap.WorldToCell(transform.position);

        FieldOfView.Clear();
        algorithm.Compute(pos, FieldOfViewRange, FieldOfView);

        if (GetComponent<Player>())
        {
            MapManager.Get.UpdateFogMap(FieldOfView);
        }
    }

    public void Heal(int hp)
    {
        int oldHitPoints = hitPoints;
        hitPoints += hp;
        if (hitPoints > maxHitPoints)
        {
            hitPoints = maxHitPoints;
        }

        int actualHealing = hitPoints - oldHitPoints;

        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
            UIManager.Instance.AddMessage($"You healed for {actualHealing} hit points!", Color.green);
        }
    }

    public void AddXP(int xp)
    {
        this.xp += xp;
        if (this.xp >= xpToNextLevel)
        {
            LevelUp();
        }

        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateXP(this.xp);
        }
    }

    private void LevelUp()
    {
        level++;
        xp -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f); // Exponentially increase XP required for next level
        maxHitPoints += 10;
        defense += 2;
        power += 2;
        hitPoints = maxHitPoints; // Fully heal on level up

        if (GetComponent<Player>())
        {
            UIManager.Instance.AddMessage("You leveled up!", Color.yellow);
            UIManager.Instance.UpdateLevel(level);
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
        }
    }
}
