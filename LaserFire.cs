using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFire : MonoBehaviour
{
    public static bool canFire;

    public GameObject leftBarrel;
   // Light leftBarrelFlash;
    public GameObject rightBarrel;
   // Light rightBarrelFlash;
    public GameObject mainBarrel;
    public Vector3 shootFromBarrel;
    private int shootOrder;

    public HitDetection laser1;
    private ParticleSystem laser1FXLeft;
    private ParticleSystem laser1FXRight;
    public float laser1Force;
    public float laser1FireRate = .2f;
    [SerializeField] private float l1Spread = 0.4f;

    public HitDetection laser2;
    private ParticleSystem laser2FX;
    public float laser2Force;
    public float laser2FireRate = .4f;
    [SerializeField] private float l2Spread = 0f;

    public HitDetection laser3;
    private ParticleSystem laser3FX;
    public float laser3Force;
    public float laser3FireRate = .4f;
    [SerializeField] private float l3Spread = 0f;

    private float timeLastFired;

    static public int currentWeapon;
    static public int currentWeaponAmmo;

    static public int laser1Ammo = 100;
    static public int laser2Ammo = 50;
    static public int laser3Ammo = 200;
    static public int defaultLaser1Ammo;
    static public int defaultLaser2Ammo;
    static public int defaultLaser3Ammo;



    public bool useAmmo = false;

     void Start()
       {
        currentWeapon = 1;
        currentWeaponAmmo = laser1Ammo;
        canFire = true;
        laser1FXLeft = leftBarrel.GetComponent<ParticleSystem>();
        laser1FXRight = rightBarrel.GetComponent<ParticleSystem>();
        laser2FX = mainBarrel.GetComponent<ParticleSystem>();
        laser3FX = mainBarrel.GetComponent<ParticleSystem>();

        defaultLaser1Ammo = laser1Ammo;
        defaultLaser2Ammo = laser2Ammo;
        defaultLaser3Ammo = laser3Ammo;

        //rightBarrelFlash = rightBarrel.GetComponent<Light>();
        //leftBarrelFlash = leftBarrel.GetComponent<Light>();
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
           else if (Input.GetKeyDown(KeyCode.Alpha3))
           {
               currentWeapon = 3;
               currentWeaponAmmo = laser3Ammo;
           }
           if (UnifiedPlayerControl.warping == false) {
               switch (currentWeapon)
               {
                   case 1:
                       if (Input.GetMouseButton(0) && laser1Ammo > 0)
                       {
                           if (Time.time > laser1FireRate + timeLastFired)
                           {
                               FireLaser1();
                               timeLastFired = Time.time;
                               //Invoke("TurnOffFlash", .05f);
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
                  case 3:
                       if (Input.GetMouseButton(0) && laser3Ammo > 0)
                       {
                          if (Time.time > laser3FireRate + timeLastFired)
                          {
                              FireLaser3();
                              timeLastFired = Time.time;
                          }
                       }
                       break;
            }
           }
       }
       void FireLaser1()
       {
        switch (shootOrder)
        {
            case 0:
                shootFromBarrel = leftBarrel.transform.position + transform.forward * 4f;
                shootOrder ++;
                laser1FXLeft.Play();
                break;
           
            case 1:
                shootFromBarrel = rightBarrel.transform.position + transform.forward * 4f;
                shootOrder-- ;
                laser1FXRight.Play();
                break;
        }
        var hitDetection = Instantiate(laser1, shootFromBarrel, leftBarrel.transform.rotation);

        hitDetection.Fire(shootFromBarrel, leftBarrel.transform.rotation, laser1Force, l1Spread);
           laser1Ammo--;
           currentWeaponAmmo--;


    }

       void FireLaser2()
       {
           laser2FX.Play();
/*           GameObject tempLaser2 = Instantiate(laser2, mainBarrel.transform.position + transform.forward * 2f, mainBarrel.transform.rotation);
           tempLaser2.transform.Rotate(Vector3.left * 90f);
           Rigidbody tempLaser2RigidBody = tempLaser2.GetComponent<Rigidbody>();
           tempLaser2RigidBody.AddForce(transform.forward * laser2Force * Time.deltaTime * 1000f);
           Destroy(tempLaser2, 4f);*/
           var hitDetection = Instantiate(laser2, mainBarrel.transform.position + transform.forward * 6f, mainBarrel.transform.rotation);
           hitDetection.Fire(mainBarrel.transform.position + transform.forward * 6f, mainBarrel.transform.rotation, laser2Force, l2Spread);
           laser2Ammo--;
           currentWeaponAmmo--;
       }
           void FireLaser3()
       {
           laser3FX.Play();

           var hitDetection = Instantiate(laser3, mainBarrel.transform.position + transform.forward * 2f, mainBarrel.transform.rotation);
           hitDetection.Fire(mainBarrel.transform.position + transform.forward * 2f, mainBarrel.transform.rotation, laser3Force, l3Spread);
           laser3Ammo--;
           currentWeaponAmmo--;
       }


}
