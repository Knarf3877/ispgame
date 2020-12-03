using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFire : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject leftBarrel;
    public GameObject rightBarrel;
    public Vector3 shootFromBarrel;
    bool shootOrder = false;

    public GameObject laser1;
    public float laser1Force;

    public float laser1FireRate = .2f;
    private float timeLastFired;

    static public int laser1Ammo = 100;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.GetMouseButton(0) && laser1Ammo > 0)
        {
            if(Time.time> laser1FireRate + timeLastFired)
            {
                FireLaser1();
                timeLastFired = Time.time;
            } 

        }
    }
    void FireLaser1()
    {
        if(shootOrder == true)
        {
            shootFromBarrel = leftBarrel.transform.position + transform.forward * 4f;
            shootOrder = !shootOrder;

        }
        else
        {
            shootFromBarrel = rightBarrel.transform.position + transform.forward * 4f;
            shootOrder = !shootOrder;
        }
        GameObject tempLaser1 = Instantiate(laser1, shootFromBarrel, leftBarrel.transform.rotation);
        tempLaser1.transform.Rotate(Vector3.left * 90f);
        Rigidbody tempLaser1RigidBody = tempLaser1.GetComponent<Rigidbody>();
        tempLaser1RigidBody.AddForce(transform.forward * laser1Force * Time.deltaTime * 1000f);
        Destroy(tempLaser1, 4f);
        laser1Ammo--;
    }
}
