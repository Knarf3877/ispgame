using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Respawn : MonoBehaviour
{
    public GameObject explosionFX;
    private GameObject player;
    public GameObject playerStats;
    public GameObject playerHud;
    public GameObject respawnWindow;
    public static bool isDead;
    public GameObject mainCamera;
    public GameObject warpEngine;
    public Vector3 respawnLocation;
    GameObject[] listOfEnemies;

    private UnifiedPlayerControl fuel;
    void Start()
    {
        isDead = false;
        player = GameObject.FindGameObjectWithTag("Player");
        respawnLocation = player.transform.position;
        listOfEnemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void LateUpdate()
    {
        if (DamagePlayer.playerHealth <= 0 && isDead == false)
        {
            DestroyPlayer();
        }
    }

    public void DestroyPlayer()
    {
        isDead = true;
        mainCamera.SetActive(true);
        mainCamera.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = 118;
        GameObject tempExplode = Instantiate(explosionFX, player.transform.position, player.transform.rotation);
        UnifiedPlayerControl.warpMulti = 1f;
        Destroy(tempExplode, 10f);
        Cursor.visible = true;
        respawnWindow.SetActive(true);
        playerHud.SetActive(false);
        player.gameObject.SetActive(false);
        EnemyStats.totalDeaths = 0;
    }
    public void RespawnPlayer()
    {
        UnifiedPlayerControl.throttle = 0f;
        DamagePlayer.playerHealth = DamagePlayer.defaultPlayerHealth;
        DamagePlayer.playerShield = DamagePlayer.defaultPlayerShield;
        UnifiedPlayerControl.warpFuel = UnifiedPlayerControl.defaultWarpFuel;
        LaserFire.laser1Ammo = LaserFire.defaultLaser1Ammo;
        LaserFire.laser2Ammo = LaserFire.defaultLaser2Ammo;
        LaserFire.laser3Ammo = LaserFire.defaultLaser3Ammo;
        LaserFire.laser4Ammo = LaserFire.defaultLaser4Ammo;
        LaserFire.currentWeaponAmmo = LaserFire.laser1Ammo;
        LaserFire.currentWeapon = 1;
        mainCamera.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = 80;
        player.transform.position = respawnLocation;
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
    public void ResetEnemies()
    {
        foreach (var enemy in listOfEnemies)
        {
            print(enemy.name);
            enemy.transform.position = enemy.GetComponent<EnemyStats>().respawnLocation;
            enemy.transform.rotation = enemy.GetComponent<EnemyStats>().respawnRotation;
        }
    }
}
