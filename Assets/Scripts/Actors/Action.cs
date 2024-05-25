using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    static public void MoveOrHit(Actor actor, Vector2 direction)

    {

        // see if someone is at the target position 

        Actor target = GameManager.Get.GetActorAtLocation(actor.transform.position

                       + (Vector3)direction);



        // if not, we can move 

        if (target == null)

        {

            actor.Move(direction);

            actor.UpdateFieldOfView();

        }
        else
        {
            // If there is a target, hit
            Hit(actor, target);
        }


        // end turn in case this is the player 

        EndTurn(actor);

    }

    static public void Move(Actor actor, Vector2 direction)
    {
        if (MapManager.Get.IsWalkable(actor.transform.position + (Vector3)direction))
        {
            actor.Move(direction);
            actor.UpdateFieldOfView();
        }
    }

    static public void Hit(Actor actor, Actor target)
    {
        int damage = actor.Power - target.Defense;

        if (damage > 0)
        {
            target.DoDamage(damage);
            UIManager.Instance.AddMessage($"{actor.name} hits {target.name} for {damage} damage.", actor.GetComponent<Player>() ? Color.white : Color.red);
        }
        else
        {
            UIManager.Instance.AddMessage($"{actor.name} hits {target.name} but does no damage.", actor.GetComponent<Player>() ? Color.white : Color.red);
        }
    }
    static private void EndTurn(Actor actor)
    {
        // Controleer of de actor een player component heeft
        Player playerComponent = actor.GetComponent<Player>();
        if (playerComponent != null)
        {
            // Voer de functie StartEnemyTurn van de GameManager uit
            GameManager.Get.StartEnemyTurn();
        }
    }
}

