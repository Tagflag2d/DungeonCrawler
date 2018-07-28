using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class LevelGrid : MonoBehaviour
{
    public static LevelGrid instance = null;

    public int gridSizeX, gridSizeZ;
    public GameObject squarePrefab;
    public Transform squaresParent;

    public GameObject[,] squaresField; // stores grid-squares, to easily change their color
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        squaresField = new GameObject[gridSizeX,gridSizeZ];
        squaresParent = GameObject.Find("SquaresParent").transform;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                squaresField[x, z] = GameObject.Instantiate(squarePrefab);
                squaresField[x, z].transform.position = new Vector3(x, transform.position.y, z);
                squaresField[x, z].transform.SetParent(squaresParent);
            }
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
