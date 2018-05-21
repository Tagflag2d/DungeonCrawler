using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MoveableObject
{
    public float rotationTime; // rotation speed
    private Vector3 ownPos; // the coord to the point where the camera looks at

    float oldAngle;

    // Use this for initialization
    void Start()
    {
        ownPos = transform.position; // get target's coords
        Camera.main.transform.LookAt(ownPos); // makes the camera look to it 
    }

    // Update is called once per frame
    void Update()
    {
        if (movingFinished)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveForwardByOneCamera();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveLeftByOneCamera();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveRightByOneCamera();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveBackwardByOneCamera();
            }

            // Camera Rotation Control
            if (Input.GetKeyDown(KeyCode.Q))
            {
                movingFinished = false;
                StartCoroutine("SmoothRotationPlayer", -45);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                movingFinished = false;
                StartCoroutine("SmoothRotationPlayer", 45);
            }
        }
    }

    IEnumerator SmoothRotationPlayer(float angle)
    {
        Quaternion fromAngle = transform.rotation;
        Quaternion toAngle = Quaternion.Euler(transform.eulerAngles + Vector3.up * angle);

        for (float t = 0f; t < 1; t += Time.deltaTime / rotationTime)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }

        transform.rotation = toAngle;
        movingFinished = true;
    }
}
