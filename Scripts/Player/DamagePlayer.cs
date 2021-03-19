using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public static float playerHealth = 100;
    public static float defaultPlayerHealth = 100;
    public static float playerShield = 100;
    public static float defaultPlayerShield = 100;

    public bool isImmune;
    //public static bool warpDeath;

    void Start()
    {
        playerHealth = 100;
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
        if (collision.gameObject.tag == "Enemy")
        {
            playerHealth -= 100;
            collision.gameObject.GetComponent<EnemyStats>().DestroySelf();
        }

        if (isImmune == false)
        {
            int collisionDamage = 30;


            if (playerShield >= 15)
            {
                playerShield -= collisionDamage;
            }
            else
            {
                playerShield -= collisionDamage;
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
    public void ApplyDamage(float damage)
    {
        if (isImmune == false)
        {
            playerShield -= damage;
            if (playerShield <= 0)
            {
                playerHealth -= Mathf.Abs(playerShield);
                playerShield = 0;
            }

        }
    }
}
