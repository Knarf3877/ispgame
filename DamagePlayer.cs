using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public static float playerHealth = 100;
    public static float defaultPlayerHealth;
    public static float playerShield = 100;
    public static float defaultPlayerShield;

    public bool isImmune;
    //public static bool warpDeath;

    void Start()
    {
        isImmune = false;
        //warpDeath = false;
        defaultPlayerHealth = playerHealth;
        defaultPlayerShield = playerShield;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
       
        if (UnifiedPlayerControl.warping)
        {
            playerHealth -= 100;
        }

        if (isImmune == false)
        {
            if (playerShield >= 15) {
                playerShield -= 30;
            }
            else
            {
                playerShield -= 30;
                playerHealth -= Mathf.Abs(playerShield);
                playerShield = 0;
            }           
        }
        isImmune = true;
        Invoke("NoMoreImmunity", 2f);
    }
    void NoMoreImmunity()
    {
        isImmune = false;
    }

}
