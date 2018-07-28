using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MoveableObject
{
    public int hp, attack, defense; // HP = Max HP; maybe no defense
    public int currentHp;

    private void Start()
    {
        currentHp = hp;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            MoveOneToPlayer();
        }
    }

    // Returns if action was succesfull
    private bool MoveOneToPlayer()
    {
        Vector3 playerPos = GameObject.FindObjectOfType<PlayerCharacter>().transform.position;
        Vector3 direction = GetDirection(playerPos);

        direction.Normalize();
        direction = new Vector3(Mathf.Round(direction.x), 0, Mathf.Round(direction.z));
        Vector3 newPos = direction + transform.position;

        int x = (int)Mathf.Round(newPos.x);
        int z = (int)Mathf.Round(newPos.z);

        // Can move?
        if (!MoveToPosition(x, z))
        {
            // if not then get what is blocking the way
            GameObject other = WhatsInTheWay(x, z);
            return HandleBlockingObject(other);
        }

        // Nothing blocked
        return true;
    }

    private bool HandleBlockingObject(GameObject other)
    {
        return true;
    }

    private Vector3 GetDirection(Vector3 targetPos)
    {
        // Get Direction as -1, 0, 1 only
        Vector3 direction = targetPos - transform.position;
        direction = new Vector3(Mathf.Round(direction.x), direction.y, Mathf.Round(direction.z));

        return direction;
    }
}
