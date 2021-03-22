using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public Image healthBar;
    public float health = 100;
    public float defaultHealth = 100;

    public GameObject deathFX;
    public float respawnTime = 10;
    public bool canRespawn;

    public Vector3 enemyPool;
    public Vector3 respawnLocation;
    public Quaternion respawnRotation;

    public static int totalDeaths = 0;
    public static int turretDeaths = 0;
    void Start()
    {
        totalDeaths = PlayerPrefs.GetInt("eDeaths");
        turretDeaths = PlayerPrefs.GetInt("tDeaths");
        health = defaultHealth;
        respawnLocation = transform.position;
        respawnRotation = transform.rotation;
        PlayerPrefs.DeleteKey("eDeaths");
        PlayerPrefs.DeleteKey("tDeaths");
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health / defaultHealth;
        if (health <= 0f)
        {
            DestroySelf();
            totalDeaths++;
            PlayerPrefs.SetInt("eDeaths", totalDeaths);
            PlayerPrefs.Save();
            if (this.tag == "Turret") {
                turretDeaths++;
                PlayerPrefs.SetInt("tDeaths", turretDeaths);
                PlayerPrefs.Save();
            }
            if(canRespawn)
              Invoke("RespawnSelf", respawnTime);
        }
    }
    void OnTriggerEnter(Collider triggerCollider)
    {
        //Debug.Log(this.name + " got hit by " + triggerCollider.name);

        if (this.tag == "Enemy" && triggerCollider.tag == "Player")
        {
            DamagePlayer.playerHealth -= 100;
            DestroySelf();

            if (canRespawn)
                Invoke("RespawnSelf", respawnTime);
        }
    }
    public void ApplyDamage(float damage)
    {
        health -= damage;
    }
    //include enable/disable enemy ai
    public void DestroySelf()
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
