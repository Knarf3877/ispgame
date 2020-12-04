using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : MonoBehaviour
{
    Vector3 startPosition;
    public Vector3 itemPool = new Vector3 (1000,1000,1000);
    private float waitTime = 3f;
    public static int coinCount;
    public static int fuelCount;
    public static int ammoCount;
    private void Start()
    {
        startPosition = this.transform.position;
    }

    private void OnTriggerEnter(Collider triggerCollider)
    {

        if (this.tag == "Coin" && triggerCollider.tag == "Player")
        {
            this.transform.position = itemPool;
            coinCount++;
            Debug.Log("You have: " + coinCount + " " + this.tag);
            Invoke("Initialize", waitTime);
        } 
        else if (this.tag == "Fuel" && triggerCollider.tag == "Player")
        {
            this.transform.position = itemPool;
            fuelCount++;
            Debug.Log("You have: " + fuelCount + " " + this.tag);
            Invoke("Initialize", waitTime);
        } else if ((this.tag == "Ammo" && triggerCollider.tag == "Player"))
        {
            this.transform.position = itemPool;
            ammoCount++;
            switch (LaserFire.currentWeapon)
            {
                case 1:
                    LaserFire.laser1Ammo += 20;
                    LaserFire.currentWeaponAmmo += 20;
                    break;
                case 2:
                    LaserFire.laser2Ammo += 20;
                    LaserFire.currentWeaponAmmo += 20;
                    break;
            }
            Debug.Log("You have gained: " + ammoCount * 20 + " " + this.tag);
            Invoke("Initialize", waitTime);
        }
        
    }
    void Initialize()
    {
        this.transform.position = startPosition;
        Debug.Log("Respawn completed at : " + Time.time);
        Debug.Log("Item Respawned");
    }
}
