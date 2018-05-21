using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveableObject : MonoBehaviour
{
    public LevelGrid grid; // reference to grid, to see if space is free or inside map
    public bool movingFinished = true; // prevents object from interfering with it's smooth movement

    public float moveSpeed = 0.1f; // 0.1 units per second, how fast it moves

    // Use this for initialization
    void Awake()
    {
        grid = GameObject.FindObjectOfType<LevelGrid>();
    }

    // Moves Forward by 1 square, to where the camera is pointing at
    public void MoveForwardByOneCamera()
    {
        Vector3 newPos = Camera.main.transform.forward; // get direction from camera
        newPos = new Vector3(Mathf.Round(newPos.x), 0, Mathf.Round(newPos.z)); // get squares direction to move to (vector * vector-squares for more than 1)
        MoveToPosition((int)(Mathf.Round(transform.position.x) + newPos.x), (int)(Mathf.Round(transform.position.z) + newPos.z)); // calculate new position and move there
    }

    public void MoveBackwardByOneCamera()
    {
        Vector3 newPos = Camera.main.transform.forward * -1; // get direction from camera
        newPos = new Vector3(Mathf.Round(newPos.x), 0, Mathf.Round(newPos.z)); // get squares direction to move to (vector * vector-squares for more than 1)
        MoveToPosition((int)(Mathf.Round(transform.position.x) + newPos.x), (int)(Mathf.Round(transform.position.z) + newPos.z)); // calculate new position and move there
    }

    public void MoveLeftByOneCamera()
    {
        Vector3 newPos = Camera.main.transform.right * -1; // get direction from camera
        newPos = new Vector3(Mathf.Round(newPos.x), 0, Mathf.Round(newPos.z)); // get squares direction to move to (vector * vector-squares for more than 1)
        MoveToPosition((int)(Mathf.Round(transform.position.x) + newPos.x), (int)(Mathf.Round(transform.position.z) + newPos.z)); // calculate new position and move there
    }

    public void MoveRightByOneCamera()
    {
        Vector3 newPos = Camera.main.transform.right; // get direction from camera
        newPos = new Vector3(Mathf.Round(newPos.x), 0, Mathf.Round(newPos.z)); // get squares direction to move to (vector * vector-squares for more than 1)
        MoveToPosition((int)(Mathf.Round(transform.position.x) + newPos.x), (int)(Mathf.Round(transform.position.z) + newPos.z)); // calculate new position and move there
    }

    // Moves Object to this coordinate
    public void MoveToPosition(int x, int z)
    {
        if (CanMoveToPosition(x, z))
        {
            // Get new position and prevent further movement until moving is done
            Vector3 newPos = new Vector3(x, transform.position.y, z);
            movingFinished = false;

            // Get old position to reset square.onSquare, set new square.onSquare
            Vector2 oldPosition = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.z));
            grid.squaresField[(int)oldPosition.x, (int)oldPosition.y].GetComponent<Square>().onSquare = null;
            grid.squaresField[x, z].GetComponent<Square>().onSquare = this.gameObject;

            StartCoroutine("SmoothMove", newPos);
        }
        // else do something that tells you can't move there?
    }

    // Makes smooth movement between positions possible
    IEnumerator SmoothMove(Vector3 newPos)
    {
        while (Vector3.Distance(newPos, transform.position) > Vector3.kEpsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, moveSpeed);

            yield return null;
        }

        movingFinished = true;
    }

    private bool CanMoveToPosition(int x, int z)
    {
        // False if outside of Grid
        if (x < 0 || x >= grid.gridSizeX || z < 0 || z >= grid.gridSizeZ)
            return false;

        // Is square empty?
        if (grid.squaresField[x, z].GetComponent<Square>().onSquare != null)
            return false;

        // Can move
        return true;
    }
}
