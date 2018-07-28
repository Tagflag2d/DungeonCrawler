using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MoveableObject
{
    [System.Serializable]
    public struct PlayerStats
    {
        public int hp;
        public int hpCurrent;
        public int mana;
        public int manaCurrent;
        public int atk;
        public int atkBonus; // full atk = atk + bonus
        public int def;
        public int defBonus;
        public int ma; // magic
        public int maBonus;
        public int res;
        public int resBonus; // resitance
        //public int speed; // increase avoid // enemys have hit to counter it
    }

    public PlayerStats stats = new PlayerStats();

    public float rotationTime; // rotation speed
    private Vector3 ownPos; // the coord to the point where the camera looks at

    float oldAngle;
    public Vector3 cameraOffset;
    public bool cameraFollow = true;

    // Use this for initialization
    void Start()
    {
        ownPos = transform.position; // get target's coords
        Camera.main.transform.LookAt(ownPos); // makes the camera look to it 
        cameraOffset = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingFinished)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) // Player Move
            {
                MoveForwardByOneCamera();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) // Player Move
            {
                MoveLeftByOneCamera();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) // Player Move
            {
                MoveRightByOneCamera();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) // Player Move
            {
                MoveBackwardByOneCamera();
            }
            else if (Input.GetKeyDown(KeyCode.Q)) // Camera Rotation Control
            {
                movingFinished = false;
                cameraFollow = false;
                StartCoroutine("SmoothRotationPlayer", -45);
            }
            else if (Input.GetKeyDown(KeyCode.W)) // Camera Rotation Control
            {
                movingFinished = false;
                cameraFollow = false;
                StartCoroutine("SmoothRotationPlayer", 45);
            }
            //else if (Input.GetKeyDown(KeyCode.S)) // Free camera Move mode
            //{
            //
            //}
            else if (Input.GetKeyDown(KeyCode.Space)) // Basic Attack
            {
                Vector3 newPos = gameObject.transform.forward; // get direction from camera
                newPos = new Vector3(Mathf.Round(newPos.x), 0, Mathf.Round(newPos.z)); // target pos
                int x = (int)(Mathf.Round(transform.position.x) + newPos.x);
                int z = (int)(Mathf.Round(transform.position.z) + newPos.z);
                Square square = grid.squaresField[x, z].GetComponent<Square>();
                GameObject target = square.onSquare;
                Debug.Log("Player Target: " + newPos.ToString());
                Debug.Log("Player Target: " + target.name);
            }
        }
    }

    private void LateUpdate()
    {
        // Camera follow player
        if (cameraFollow)
            Camera.main.transform.position = transform.position + cameraOffset;
    }

    IEnumerator SmoothRotationPlayer(float angle)
    {
        Vector3 startPosition = Camera.main.transform.position;
        Quaternion startRotation = Camera.main.transform.rotation;

        Camera.main.transform.RotateAround(transform.position, Vector3.up, angle);
        Vector3 targetPosition = Camera.main.transform.position;
        Quaternion targetRotation = Camera.main.transform.rotation;

        Camera.main.transform.position = startPosition;
        Camera.main.transform.rotation = startRotation;

        for (float t = 0f; t < 0.9; t += Time.deltaTime / rotationTime)
        {
            Camera.main.transform.RotateAround(transform.position, Vector3.up, angle * Time.deltaTime / rotationTime);
            yield return null;
        }

        Camera.main.transform.position = targetPosition;
        Camera.main.transform.rotation = targetRotation;

        cameraOffset = targetPosition - transform.position;
        cameraFollow = true;

        movingFinished = true;
    }

    public override void FinishedMoving()
    {
        Square sq;
        // Reset old Attack-Range indicator
        for (int x1 = 0; x1 < grid.gridSizeX; x1++)
        {
            for (int z1 = 0; z1 < grid.gridSizeZ; z1++)
            {
                sq = grid.squaresField[x1, z1].GetComponent<Square>();
                if (sq.meshRenderer.material.GetColor("_Color") == sq.attackColor)
                {
                    sq.ChangeColor(sq.normalColor);
                }
            }
        }

        // Update Attack-Range indicator
        Vector3 newPos = gameObject.transform.forward; // get direction from camera
        newPos = new Vector3(Mathf.Round(newPos.x), 0, Mathf.Round(newPos.z)); // target pos

        int x = (int)(Mathf.Round(transform.position.x) + newPos.x);
        int z = (int)(Mathf.Round(transform.position.z) + newPos.z);

        // if not outside of Grid
        if (!(x < 0 || x >= grid.gridSizeX || z < 0 || z >= grid.gridSizeZ))
        {
            Square square = grid.squaresField[x, z].GetComponent<Square>();
            square.ChangeColor(square.attackColor);
        }

        base.FinishedMoving();
    }
}
