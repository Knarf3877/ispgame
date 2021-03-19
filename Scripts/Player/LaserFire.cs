using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFire : MonoBehaviour
{
    public static bool canFire;

    public GameObject leftBarrel;
    public GameObject rightBarrel;
    public GameObject mainBarrel;
    public GameObject mainBarrel2;
    private Vector3 shootFromBarrel;
    private int shootOrder;

    public HitDetection laser1;
    private ParticleSystem laser1FXLeft;
    private ParticleSystem laser1FXRight;
    public float laser1Force;
    public float laser1FireRate = .2f;
    [SerializeField] private float l1Spread = 0.15f;

    public HitDetection laser2;
    private ParticleSystem laser2FX;
    public float laser2Force;
    public float laser2FireRate = .4f;
    [SerializeField] private float l2Spread = 0f;

    public HitDetection laser3;
    private ParticleSystem laser3FX;
    public float laser3Force;
    public float laser3FireRate = .12f;
    [SerializeField] private float l3Spread = 0.5f;

    public HitDetection laser4;
    private ParticleSystem laser4FX;
    public float laser4Force;
    public float laser4FireRate = .5f;
    [SerializeField] private float l4Spread = 1.5f;

    private float timeLastFired;

    static public int currentWeapon;
    static public int currentWeaponAmmo;

    static public int laser1Ammo = 100;
    static public int laser2Ammo = 50;
    static public int laser3Ammo = 200;
    static public int laser4Ammo = 50;
    static public int defaultLaser1Ammo = 100;
    static public int defaultLaser2Ammo = 50;
    static public int defaultLaser3Ammo = 200;
    static public int defaultLaser4Ammo = 50;

    public bool infiniteAmmo = false;

    void Start()
    {
        currentWeapon = 1;
        currentWeaponAmmo = laser1Ammo;
        canFire = true;
        laser1FXLeft = leftBarrel.GetComponent<ParticleSystem>();
        laser1FXRight = rightBarrel.GetComponent<ParticleSystem>();
        laser2FX = mainBarrel.GetComponent<ParticleSystem>();
        laser3FX = mainBarrel2.GetComponent<ParticleSystem>();
        laser4FX = mainBarrel2.GetComponent<ParticleSystem>();

        defaultLaser1Ammo = laser1Ammo;
        defaultLaser2Ammo = laser2Ammo;
        defaultLaser3Ammo = laser3Ammo;
        defaultLaser4Ammo = laser4Ammo;
    }



    // Update is called once per frame
    void Update()
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
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentWeapon = 4;
            currentWeaponAmmo = laser4Ammo;
        }
        if (UnifiedPlayerControl.warping == false)
        {
            switch (currentWeapon)
            {
                case 1:
                    if (Input.GetMouseButton(0) && laser1Ammo > 0)
                    {
                        if (Time.time > laser1FireRate + timeLastFired)
                        {
                            FireLaser1();
                            leftBarrel.GetComponent<AudioSource>().Play();
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
                            rightBarrel.GetComponent<AudioSource>().Play();
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
                            mainBarrel.GetComponent<AudioSource>().Play();
                            timeLastFired = Time.time;
                        }
                    }
                    break;
                case 4:
                    if (Input.GetMouseButton(0) && laser4Ammo > 0)
                    {
                        if (Time.time > laser4FireRate + timeLastFired)
                        {
                            FireLaser4();
                            mainBarrel2.GetComponent<AudioSource>().Play();
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
                shootFromBarrel = leftBarrel.transform.position + transform.forward * 5f;
                shootOrder++;
                laser1FXLeft.Play();
                break;

            case 1:
                shootFromBarrel = rightBarrel.transform.position + transform.forward * 5f;
                shootOrder--;
                laser1FXRight.Play();
                break;
        }
        var hitDetection = Instantiate(laser1, shootFromBarrel, leftBarrel.transform.rotation);
        hitDetection.Fire(shootFromBarrel, leftBarrel.transform.rotation, laser1Force, l1Spread);
        UseAmmo(1);
    }

    void FireLaser2()
    {
        laser2FX.Play();
        var hitDetection = Instantiate(laser2, mainBarrel.transform.position + transform.forward * 6f, mainBarrel.transform.rotation);
        hitDetection.Fire(mainBarrel.transform.position + transform.forward * 6f, mainBarrel.transform.rotation, laser2Force, l2Spread);
        UseAmmo(2);
    }
    void FireLaser3()
    {
        laser3FX.Play();
        var hitDetection = Instantiate(laser3, mainBarrel2.transform.position + transform.forward * 2f, mainBarrel2.transform.rotation);
        hitDetection.Fire(mainBarrel2.transform.position + transform.forward * 2f, mainBarrel2.transform.rotation, laser3Force, l3Spread);
        UseAmmo(3);
    }

    void FireLaser4()
    {
        laser4FX.Play();
        for (int i = 0; i <= 10; i++)
        {
            var hitDetection = Instantiate(laser4, mainBarrel2.transform.position + transform.forward * 6f, mainBarrel2.transform.rotation);
            hitDetection.Fire(mainBarrel2.transform.position + transform.forward * 6f, mainBarrel2.transform.rotation, laser4Force, l4Spread);            
        }
        UseAmmo(4);
    }

    void UseAmmo(int currentWeapon)
    {
        if(infiniteAmmo == false)
        {
            switch (currentWeapon)
            {
                case 1:
                    laser1Ammo--;
                    break;
                case 2:
                    laser2Ammo--;
                    break;
                case 3:
                    laser3Ammo--;
                    break;
                case 4:
                    laser4Ammo--;
                    break;

            }
            currentWeaponAmmo--;
        }
    }

}
