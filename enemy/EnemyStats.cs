using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public Image healthBar;
    public float health = 100;
    public float defaultHealth = 100;

    Respawn death;
    public GameObject deathFX;


    public Vector3 enemyPool;
    public Vector3 respawnLocation;
    public Quaternion respawnRotation;
    public static int totalDeaths = 0;
    void Start()
    {
        health = defaultHealth;
        respawnLocation = transform.position;
        respawnRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health / 100;
        if(health <= 0f)
        {
            DestroySelf();
            totalDeaths++;
            Invoke("RespawnSelf", 10f);
        }
    }
    void OnTriggerEnter(Collider triggerCollider)
    {
        //Debug.Log(this.name + " got hit by " + triggerCollider.name);

        if (this.tag == "Enemy" && triggerCollider.tag == "Player")
        {
            DamagePlayer.playerHealth -= 100;
            DestroySelf();
            Invoke("RespawnSelf", 10f);
        }
    }
    public void ApplyDamage(float damage)
    {
        health -= damage;
    }
    //include enable/disable enemy ai
    void DestroySelf()
    {
        
        health = defaultHealth;
        GameObject tempEnemyFX = Instantiate(deathFX, transform.position, transform.rotation);
        Destroy(tempEnemyFX, 6f);
        transform.position = enemyPool;
        gameObject.SetActive(false);

    }

    void RespawnSelf()
    {
        gameObject.SetActive(true);
        transform.position = respawnLocation;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.rotation = respawnRotation;
    }
}
