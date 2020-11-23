using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 6;
    Vector3 velocity;
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
        Vector3 input = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
        Vector3 direction = input.normalized;
        velocity = direction * speed;
    }

     void FixedUpdate()
    {
        myRigidBody.position += velocity * Time.fixedDeltaTime;
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
