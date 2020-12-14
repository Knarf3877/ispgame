using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnifiedPlayerControl : MonoBehaviour
{
    public float speed = 20;
    public float sideSpeed = 10;
    public float verticalSpeed = 6;
    public float brakeSpeed = 2;

    private float activeSpeed;
    private float vertAxis;
    private float thrustIncrement;
    static public float throttle;
    private float activeSideSpeed;
    private float activeVertSpeed;
    static public float totalSpeed;

    private float forwardAccerleration = 2;
    private float otherAccerleration = 1.25f;

    public float mouseLookSpeed = 300;
    private Vector2 mouseDistance;
    private Vector2 mouseLocation;
    private Vector2 screenCenter;

    public float rollSpeed = 100;
    public float rollAccerleration = 4;
    private float rollInput;


    public Texture2D crossHair, defaultCursor;
    Rigidbody playerRB;

    // Start is called before the first frame update
    void Start()
    {
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        //Cursor.SetCursor(crossHair, new Vector2(crossHair.width / 2, crossHair.height / 2 + 1), CursorMode.Auto);


    }

    void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            Debug.Log("this is working");
            gameObject.GetComponent<SU_TravelWarp>().Warp = true;

        }
        else
        {
            gameObject.GetComponent<SU_TravelWarp>().Warp = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vertAxis = Input.GetAxis("Vertical");
        if (vertAxis > 0)
        {
            thrustIncrement = 0.02f;
        }
        else if (vertAxis < 0)
        {
            thrustIncrement = -0.02f;
        }

        if (Input.GetButton("Vertical"))
        {
            throttle += thrustIncrement;
        }

        throttle = Mathf.Clamp(throttle, -0.5f, 1.25f);

        mouseLocation = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mouseDistance = new Vector2((mouseLocation.x - screenCenter.x) / screenCenter.x, (mouseLocation.y - screenCenter.y) / screenCenter.y);
        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1);

        transform.Rotate(-mouseDistance.y * Time.deltaTime * mouseLookSpeed, mouseDistance.x * Time.deltaTime * mouseLookSpeed, rollSpeed * Time.deltaTime * -rollInput, Space.Self);

        activeSpeed = Mathf.Lerp(activeSpeed, throttle * speed, forwardAccerleration * Time.deltaTime);
        activeSideSpeed = Mathf.Lerp(activeSideSpeed, Input.GetAxisRaw("Horizontal") * sideSpeed, otherAccerleration * Time.deltaTime);
        activeVertSpeed = Mathf.Lerp(activeVertSpeed, Input.GetAxisRaw("Height") * verticalSpeed, otherAccerleration * Time.deltaTime);

        /*transform.position += transform.forward * activeSpeed * Time.deltaTime;
        transform.position += transform.right * activeSideSpeed * Time.deltaTime;
        transform.position += transform.up * activeVertSpeed * Time.deltaTime;*/

        if (Input.GetKey(KeyCode.B))
        {
            Brake();
        }

        Rigidbody playerRB = GetComponent<Rigidbody>();
        Vector3 movementForce = new Vector3(activeSpeed, activeSideSpeed, activeVertSpeed);
        playerRB.AddRelativeForce(activeSideSpeed * 400f * Time.deltaTime, activeVertSpeed * 400f * Time.deltaTime, activeSpeed * 400f * Time.deltaTime);

        if (Input.GetKey(KeyCode.G))
        {
            mouseLookSpeed = 10f;
            playerRB.AddRelativeForce(0, 0, 500000f * Time.deltaTime);
        }
        else
        {
            mouseLookSpeed = 300f;
        }

        //add speed dial

        totalSpeed = Mathf.Sqrt((activeSpeed * activeSpeed) + (activeSideSpeed * activeSideSpeed) + (activeVertSpeed * activeVertSpeed));
        totalSpeed = Mathf.Round(totalSpeed * 10);
        //Debug.Log(totalSpeed);

        rollInput = Mathf.Lerp(rollInput, Input.GetAxis("Roll"), rollAccerleration * Time.deltaTime);

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = CursorLockMode.None;
        }

    }


    void OnCollisionEnter()
    {
        //Rigidbody playerRB = GetComponent<Rigidbody>();
        //playerRB.AddRelativeForce(-activeSideSpeed * 400f * Time.deltaTime, -activeVertSpeed * 400f * Time.deltaTime, -activeSpeed * 400f * Time.deltaTime);
        //transform.position = Vector3.Lerp(transform.position, transform.position - Vector3.one * 13f, 10f * Time.deltaTime);
        throttle = -1f;
        Invoke("setZero", 1.4f);
        Debug.Log("hit object");
    }

    void setZero()
    {
        throttle = 0;
    }

    void Brake()
    {
        throttle = Mathf.Lerp(throttle, 0f, brakeSpeed * Time.deltaTime);
        activeSideSpeed = Mathf.Lerp(activeSideSpeed, 0f, brakeSpeed * Time.deltaTime); ;
        activeVertSpeed = Mathf.Lerp(activeVertSpeed, 0f, brakeSpeed * Time.deltaTime);
        transform.Rotate(Vector3.zero);
        Debug.Log("braked");
    }

}
