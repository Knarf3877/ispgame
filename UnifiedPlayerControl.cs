﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnifiedPlayerControl : MonoBehaviour
{
    public float speed = 20;
    public float sideSpeed = 10;
    public float verticalSpeed = 6;

    private float activeSpeed;
    private float activeSideSpeed;
    private float activeVertSpeed;
    static public float totalSpeed;

    private float forwardAccerleration = 2;
    private float otherAccerleration = 1.25f;

    public float mouseLookSpeed = 100;
    private Vector2 mouseDistance;
    private Vector2 mouseLocation;
    private Vector2 screenCenter;

    public float rollSpeed = 100;
    public float rollAccerleration = 4;
    private float rollInput;

    // Start is called before the first frame update
    void Start()
    {
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        mouseLocation = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //mouseLocation.x = Input.mousePosition.x;
        //mouseLocation.y = Input.mousePosition.y;
        //sets mouseDistance to proportion to center of screen
        mouseDistance = new Vector2((mouseLocation.x - screenCenter.x) / screenCenter.x, (mouseLocation.y - screenCenter.y) / screenCenter.y);
        //mouseDistance.x = (mouseLocation.x - screenCenter.x) / screenCenter.x;
        //mouseDistance.y = (mouseLocation.y - screenCenter.y) / screenCenter.y;

        //Debug.Log(mouseDistance);

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1);

        transform.Rotate(-mouseDistance.y * Time.deltaTime * mouseLookSpeed, mouseDistance.x * Time.deltaTime * mouseLookSpeed, rollSpeed * Time.deltaTime * -rollInput, Space.Self);

        activeSpeed = Mathf.Lerp(activeSpeed, Input.GetAxisRaw("Vertical") * speed, forwardAccerleration * Time.deltaTime);
        activeSideSpeed = Mathf.Lerp(activeSideSpeed, Input.GetAxisRaw("Horizontal") * sideSpeed, otherAccerleration * Time.deltaTime);
        activeVertSpeed = Mathf.Lerp(activeVertSpeed, Input.GetAxisRaw("Height") * verticalSpeed, otherAccerleration * Time.deltaTime);

        transform.position += transform.forward * activeSpeed * Time.deltaTime;
        transform.position += transform.right * activeSideSpeed * Time.deltaTime;
        transform.position += transform.up * activeVertSpeed * Time.deltaTime;

        //add speed dial

        totalSpeed = Mathf.Sqrt((activeSpeed * activeSpeed) + (activeSideSpeed * activeSideSpeed) + (activeVertSpeed * activeVertSpeed));
        totalSpeed = Mathf.Round(totalSpeed * 10);
        //Debug.Log(totalSpeed);

        rollInput = Mathf.Lerp(rollInput, Input.GetAxis("Roll"), rollAccerleration * Time.deltaTime);

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

}