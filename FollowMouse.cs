using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Vector2 mouseDirection;

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseChange = new Vector2(Input.GetAxisRaw("Mouse X") * 2f , Input.GetAxisRaw("Mouse Y") * 2f );

        mouseDirection += mouseChange;

        //Quaternion xaxis = 
        //transform.localRotation =  Quaternion.AngleAxis(mouseDirection.x, Vector3.up);
        //transform.localRotation = xaxis * Quaternion.AngleAxis(-mouseDirection.y, Vector3.right);
        //transform.Rotate(-mouseChange.y, 0, mouseChange.x);

        transform.Rotate(-Input.GetAxis("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0);
        //toggle mouse cursor

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
