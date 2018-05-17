using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    public LevelGrid grid; // reference to grid, to see if space is free or inside map
    public bool movingFinished = true; // prevents object from interfering with it's smooth movement

    public float moveSpeed = 0.1f; // 0.1 units per second, how fast it moves

    // Use this for initialization
    void Awake()
    {
        grid = GameObject.FindObjectOfType<LevelGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        // Test
        if (movingFinished)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveForwardByOneCamera();
            }
        }
    }

    // Moves Forward by 1 square, to where the camera is pointing at
    public void MoveForwardByOneCamera()
    {
        Vector3 newPos = Camera.main.transform.forward; // get direction from camera
        newPos = new Vector3(Mathf.Round(newPos.x), 0, Mathf.Round(newPos.z)); // get squares direction to move to (vector * vector-squares for more than 1)
        MoveToPosition((int)(Mathf.Round(transform.position.x)+newPos.x), (int)(Mathf.Round(transform.position.z) + newPos.z)); // calculate new position and move there
    }

    // Moves Object to this coordinate
    public void MoveToPosition(int x, int z)
    {
        Debug.Log("Start Coroutine");
        Vector3 newPos = new Vector3(x, transform.position.y, z);
        movingFinished = false;

        StartCoroutine("SmoothMove", newPos);
    }

    IEnumerator SmoothMove(Vector3 newPos)
    {
        while (Vector3.Distance(newPos, transform.position) > Vector3.kEpsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, moveSpeed);
            
            yield return null;
        }

        movingFinished = true;
    }
}
