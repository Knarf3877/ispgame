using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFire : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject leftBarrel;
    public GameObject rightBarrel;
    public GameObject mainBarrel;
    public Vector3 shootFromBarrel;
    bool shootOrder = false;

    public GameObject laser1;
    public float laser1Force;
    public float laser1FireRate = .2f;

    public GameObject laser2;
    public float laser2Force;
    public float laser2FireRate = .4f;

    private float timeLastFired;

    static public int currentWeapon;
    static public int currentWeaponAmmo;

    static public int laser1Ammo = 100;
    static public int laser2Ammo = 50;

    void Start()
    {
        currentWeapon = 1;
        currentWeaponAmmo = laser1Ammo;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            currentWeapon = 1;
            currentWeaponAmmo = laser1Ammo;


        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = 2;
            currentWeaponAmmo = laser2Ammo;
        }

        switch (currentWeapon)
        {
            case 1:
                if (Input.GetMouseButton(0) && laser1Ammo > 0)
                {
                    if (Time.time > laser1FireRate + timeLastFired)
                    {
                        FireLaser1();
                        timeLastFired = Time.time;
                    }
                }
                break;
            case 2:
                if (Input.GetMouseButton(0) && laser2Ammo > 0)
                {
                    if (Time.time > laser2FireRate + timeLastFired)
                    {
                        FireLaser2();
                        timeLastFired = Time.time;
                    }
                }
                break;
        }
    }
    void FireLaser1()
    {
        if(shootOrder == true)
        {
            shootFromBarrel = leftBarrel.transform.position + transform.forward * 4f;
            shootOrder = !shootOrder;

        }else
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
        currentWeaponAmmo--;
    }

    void FireLaser2()
    {
        GameObject tempLaser2 = Instantiate(laser2, mainBarrel.transform.position + transform.forward * 2f, mainBarrel.transform.rotation);
        tempLaser2.transform.Rotate(Vector3.left * 90f);
        Rigidbody tempLaser2RigidBody = tempLaser2.GetComponent<Rigidbody>();
        tempLaser2RigidBody.AddForce(transform.forward * laser1Force * Time.deltaTime * 1000f);
        Destroy(tempLaser2, 4f);
        laser2Ammo--;
        currentWeaponAmmo--;
    }
}
