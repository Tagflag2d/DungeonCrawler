using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    public Color normalColor, moveColor, attackColor; // NormalColor is default color of squares, MoveColor is where player lands after using move
    public Color enemyAttackColor;

    public GameObject onSquare;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        onSquare = null;
    }

    public void ChangeColor(Color color)
    {
        meshRenderer.material.SetColor("_Color", color);
    }
}
