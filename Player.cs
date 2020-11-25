using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10f;
    float forwardMovement;
    float horizontalMovement;
    int coinCount;
    int fuelCount;
    Rigidbody myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
         forwardMovement = Input.GetAxis("Vertical");
           forwardMovement = forwardMovement * Time.deltaTime * speed;

           horizontalMovement = Input.GetAxis("Horizontal");
           horizontalMovement = horizontalMovement * Time.deltaTime * speed;
       
    }

     void FixedUpdate()
    {
        //need to make this physics based
        myRigidBody.transform.Translate(Vector3.forward * forwardMovement);
        myRigidBody.transform.Translate(Vector3.right * horizontalMovement);
        

        
    }

    void OnTriggerEnter(Collider triggerCollider)
    {
        if (triggerCollider.tag == "Coin")
        {
            Destroy(triggerCollider.gameObject);
            coinCount++;
            print("You have: " + coinCount + " " + triggerCollider.gameObject.tag );
        }
        else if (triggerCollider.gameObject.tag == "Fuel")
        {
            Destroy(triggerCollider.gameObject);
            fuelCount++;
            print("You have: " + fuelCount + " " + triggerCollider.gameObject.tag);
        }
        
     
    }
}
