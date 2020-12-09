using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public static float playerHealth = 100;
    public static float playerShield = 100;
    public bool isImmune;
    public GameObject explosionFX;

    // Start is called before the first frame update
    void Start()
    {
        isImmune = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth <= 0)
        {
            DestroyPlayer();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isImmune == false)
        {
            if (playerShield >= 15) {
                playerShield -= 15;
            }
            else
            {
                playerShield -= 15;
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

    void DestroyPlayer()
    {
        Destroy(gameObject);
        Instantiate(explosionFX, transform.position, transform.rotation);
    }
}
