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

    public int MaxHitPoints => maxHitPoints;
    public int HitPoints => hitPoints;
    public int Defense => defense;
    public int Power => power;

    private void Start()
    {
        algorithm = new AdamMilVisibility();
        UpdateFieldOfView();

        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
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

    public void DoDamage(int hp)
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
}
