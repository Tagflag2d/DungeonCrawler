using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    public Color normalColor, moveColor, attackColor; // NormalColor is default color of squares, MoveColor is where player lands after using move
    public Color enemyAttackColor;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
}
