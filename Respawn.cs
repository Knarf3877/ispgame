using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject explosionFX;
    public GameObject player;
    public GameObject playerStats;
    public GameObject playerHud;
    public GameObject respawnWindow;
    public static bool isDead;
    public GameObject mainCamera;
    public GameObject warpEngine;

    void Start()
    {
        isDead = false;
    }

    void LateUpdate()
    {
        if (DamagePlayer.playerHealth <= 0)
        {
            DestroyPlayer();
        }
      /*  if (DamagePlayer.warpDeath)
        {
            DamagePlayer.warpDeath = false;
            DestroyPlayer();
        }*/
    }

    public void DestroyPlayer()
    {
        isDead = true;
        GameObject tempExplode = Instantiate(explosionFX, player.transform.position, player.transform.rotation);
        Destroy(tempExplode, 10f);
        Cursor.visible = true;               
        DamagePlayer.playerHealth = DamagePlayer.defaultPlayerHealth;
        DamagePlayer.playerShield = DamagePlayer.defaultPlayerShield;
        UnifiedPlayerControl.warpFuel = UnifiedPlayerControl.defaultWarpFuel;
        LaserFire.laser1Ammo = LaserFire.defaultLaser1Ammo;
        LaserFire.laser2Ammo = LaserFire.defaultLaser2Ammo;
        LaserFire.currentWeaponAmmo = LaserFire.laser1Ammo;
        LaserFire.currentWeapon = 1;
        respawnWindow.SetActive(true);
        playerHud.SetActive(false);
        player.gameObject.SetActive(false);
    }
    public void RespawnPlayer()
    {        
        player.transform.position = new Vector3(0, 0, -10);
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        player.gameObject.SetActive(true);
        warpEngine.SetActive(false);
        UnifiedPlayerControl.mouseLookSpeed = 300f;
        UnifiedPlayerControl.warping = false;
        player.GetComponent<SU_TravelWarp>().Warp = false;
        respawnWindow.SetActive(false);        
        playerHud.SetActive(true);
        Cursor.visible = false;
        isDead = false;
        mainCamera.SetActive(true);


    }
}
