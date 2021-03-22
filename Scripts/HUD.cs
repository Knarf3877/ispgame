using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HUD : MonoBehaviour
{
    public GameObject scoretext;
    public GameObject weaponText;
    public GameObject lifeText;
    public GameObject missionText;
    public Image healthBar;
    public Image shieldBar;
    static public int lives;
    public int defaultLives = 3;
    /// <summary>
    /// 1 - kill enemies
    /// 2 - destroy targets
    /// 3 - finish tutorial
    /// </summary>
    public int missionType;
    int requiredKills;
    public GameObject levelWin;
    public bool isTutorial;

    private void Start()
    {
        requiredKills = KillsToDifficulty((int)PlayerPrefs.GetFloat("difficulty"));
        // avgFrameRate = 0;
        if (isTutorial)
        {
            requiredKills = 1;
        }
        lives = (int)PlayerPrefs.GetFloat("lives");
    }
    private void Update()
    {
        // avgFrameRate = Mathf.Round(Time.frameCount / Time.time);
        scoretext.GetComponent<Text>().text = UnifiedPlayerControl.totalSpeed.ToString("F0") + " mph" +
            //   "\n" + avgFrameRate + " fps" +
            "\n" + UnifiedPlayerControl.warpFuel.ToString("F0") + " : fuel left" +
            "\n" + UnifiedPlayerControl.throttle.ToString("F3") +
            "\n" + EnemyStats.totalDeaths.ToString("F0") + " : enemies killed";
        weaponText.GetComponent<TextMeshProUGUI>().text = WeaponName(LaserFire.currentWeapon) +
           "\n" + LaserFire.currentWeaponAmmo.ToString("F0");
        healthBar.fillAmount = DamagePlayer.playerHealth / 100;
        shieldBar.fillAmount = DamagePlayer.playerShield / 100;

        lifeText.GetComponent<TMP_Text>().text = "Lives: " + lives;
        missionText.GetComponent<TMP_Text>().text = MissionObjective(missionType);
        if (missionType == 3 && EnemyStats.totalDeaths == requiredKills)
        {
            levelWin.GetComponent<PauseMenu>().LevelComplete();

        }
        if ((missionType == 1 && EnemyStats.totalDeaths == requiredKills) || (missionType == 2 && EnemyStats.turretDeaths == 32))
        {
            levelWin.GetComponent<PauseMenu>().LevelComplete();
        }
    }

    string WeaponName(int weaponNum)
    {
        switch (weaponNum)
        {
            case 1:
                return "Type 1";

            case 2:
                return "Type 2";

            case 3:
                return "Type 3";

            case 4:
                return "Type 4";

            default:
                return "weapon not found";

        }
    }

    private int KillsToDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 1:
                return 8;
            case 2:
                return 15;
            case 3:
                return 30;
            default:
                return 100;
        }

    }
    private string MissionObjective(int type)
    {
        switch (type)
        {

            case 1:
                return "Mission Objective:\n" + "Defeat Enemy Fighters " + PlayerPrefs.GetInt("eDeaths").ToString("F0") + "/" + requiredKills;
            case 2:
                return "Mission Objective:\n" + "Destroy Turrets " + PlayerPrefs.GetInt("tDeaths").ToString("F0") + "/32";
            case 3:
                return "Mission Objective:\n" + "Complete Tutorial " + PlayerPrefs.GetInt("eDeaths").ToString("F0") + "/" + 1;
            default:
                return "mission not found";

        }

    }

}
