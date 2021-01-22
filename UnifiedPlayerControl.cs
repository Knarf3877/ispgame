using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnifiedPlayerControl : MonoBehaviour
{
    public float speed = 40;
    public float sideSpeed = 10;
    public float verticalSpeed = 6;
    public float brakeSpeed = 4;

    private float activeSpeed;
    private float vertAxis;
    private float thrustIncrement;
    static public float throttle;
    private float activeSideSpeed;
    private float activeVertSpeed;
    static public float totalSpeed;
    static public float warpMulti = 1;
    static public bool warping;
    static public float warpFuel = 100f;
    static public float defaultWarpFuel;

    private float forwardAccerleration = 2;
    private float otherAccerleration = 1.25f;

    static public float mouseLookSpeed = 300;
    private Vector2 mouseDistance;
    private Vector2 mouseLocation;
    private Vector2 screenCenter;

    public float rollSpeed = 100;
    public float rollAccerleration = 4;
    private float rollInput;

    public GameObject mainCamera;
    public GameObject fpsCam;

    public GameObject mainEngine;
    public GameObject mainEngineInput;
    public ParticleSystem mainThrust;
    public GameObject reverseEngine;
    public GameObject reverseEngineInput;
    public ParticleSystem reverseThrust;
    public GameObject warpEngine;

    public Rigidbody playerRB;

    // Start is called before the first frame update
    void Start()
    {
        warping = false;
        fpsCam.SetActive(false);
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        mainEngine.SetActive(false); 
        reverseEngine.SetActive(false);
        warpEngine.SetActive(false);

        Rigidbody playerRB = GetComponent<Rigidbody>();

        defaultWarpFuel = warpFuel;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            fpsCam.SetActive(!fpsCam.activeSelf);
        }

        if (throttle > 0.001)
        {
           mainEngine.SetActive(true);
           reverseEngine.SetActive(false);
        }
        else if (throttle < -0.001)
        {
           mainEngine.SetActive(false);
           reverseEngine.SetActive(true);
        }
        else 
        {
           mainEngine.SetActive(false);
           reverseEngine.SetActive(false);
        }

        if (Input.GetKey(KeyCode.W))
        {
            mainEngineInput.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            reverseEngineInput.SetActive(true);
        }
        else
        {
            mainEngineInput.SetActive(false);
            reverseEngineInput.SetActive(false);
        }
        if (Input.GetKey(KeyCode.G) && warpFuel > 0 && throttle > 0)
        {
            gameObject.GetComponent<SU_TravelWarp>().Warp = true;

        }
        else if (Input.GetKeyUp(KeyCode.G) || warpFuel <= 0)
        {
            gameObject.GetComponent<SU_TravelWarp>().Warp = false;
        }
    }

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

        throttle = Mathf.Clamp(throttle, -0.5f, 1f);

        mouseLocation = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mouseDistance = new Vector2((mouseLocation.x - screenCenter.x) / screenCenter.x, (mouseLocation.y - screenCenter.y) / screenCenter.y);
        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1);

        rollInput = Mathf.Lerp(rollInput, Input.GetAxis("Roll"), rollAccerleration * Time.deltaTime);

        transform.Rotate(-mouseDistance.y * Time.deltaTime * mouseLookSpeed / 2, mouseDistance.x * Time.deltaTime * mouseLookSpeed / 2, rollSpeed * Time.deltaTime * -rollInput, Space.Self);

        activeSpeed = Mathf.Lerp(activeSpeed, throttle * speed, forwardAccerleration * Time.deltaTime);
        activeSideSpeed = Mathf.Lerp(activeSideSpeed, Input.GetAxisRaw("Horizontal") * sideSpeed, otherAccerleration * Time.deltaTime);
        activeVertSpeed = Mathf.Lerp(activeVertSpeed, Input.GetAxisRaw("Height") * verticalSpeed, otherAccerleration * Time.deltaTime);

        Vector3 movementForce = new Vector3(activeSpeed, activeSideSpeed, activeVertSpeed);
        playerRB.AddRelativeForce(activeSideSpeed * 300f * Time.deltaTime, activeVertSpeed * 300f * Time.deltaTime, activeSpeed * 300f * Time.deltaTime * warpMulti);

        totalSpeed = Mathf.Sqrt((activeSpeed * activeSpeed) + (activeSideSpeed * activeSideSpeed) + (activeVertSpeed * activeVertSpeed));
        totalSpeed = Mathf.Round(totalSpeed * 10);

        if (Input.GetKey(KeyCode.G) && warpFuel > 0 && throttle > 0)
        {
            mainCamera.SetActive(false);
            mouseLookSpeed = 20f;
            warpMulti += .1f;
            warpMulti = Mathf.Clamp(warpMulti, 1, 3);
            warping = true;
            warpFuel -= .5f;
            totalSpeed += 100;
            warpEngine.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.G) || warpFuel <= 0)
        {
            mainCamera.SetActive(true);
            mouseLookSpeed = 300f;
            warping = false;
            warpEngine.SetActive(false);
            if (warpMulti != 1f) {
                warpMulti = 1f;
            }
        }

        

        if (Input.GetKey(KeyCode.B))
        {
            Brake();
        }
    }

    void OnCollisionEnter()
    {
        throttle = -.5f;
        Invoke("setZero", 1.4f);
        Debug.Log("hit object");
    }


    void setZero()
    {
        throttle = 0;
        playerRB.velocity = Vector3.zero;
        playerRB.angularVelocity = Vector3.zero;

    }

    void Brake()
    {
        throttle = Mathf.Lerp(throttle, 0f, brakeSpeed * Time.deltaTime);
        activeSideSpeed = Mathf.Lerp(activeSideSpeed, 0f, brakeSpeed * Time.deltaTime);
        activeVertSpeed = Mathf.Lerp(activeVertSpeed, 0f, brakeSpeed * Time.deltaTime);
        transform.Rotate(Vector3.zero);

    }

}
