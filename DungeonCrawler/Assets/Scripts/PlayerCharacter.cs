using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MoveableObject
{
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
                cameraFollow = false;
                StartCoroutine("SmoothRotationPlayer", -45);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                movingFinished = false;
                cameraFollow = false;
                StartCoroutine("SmoothRotationPlayer", 45);
            }

            else if (Input.GetKeyDown(KeyCode.S))
            {
                Camera.main.transform.RotateAround(transform.position, Vector3.up, 45);
            }
        }

    }

    private void LateUpdate()
    {
        // Camera follow player
        if (cameraFollow)
            Camera.main.transform.position = transform.position+cameraOffset;
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
}
