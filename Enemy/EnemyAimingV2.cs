using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAimingV2 : MonoBehaviour
{
	// === variables you need ===
	//how fast our shots move
	float shotSpeed;
	//objects
	public GameObject shooter;
	public GameObject target;

	// === derived variables ===
	//positions
	Vector3 shooterPosition;
	Vector3 targetPosition;
	//velocities
	Vector3 shooterVelocity;
	Vector3 targetVelocity;

    private void Start()
    {
        shooterPosition = shooter.transform.position;
        targetPosition = target.transform.position;
        shotSpeed = 1200f;
        shooterVelocity = shooter.GetComponent<Rigidbody>() ? shooter.GetComponent<Rigidbody>().velocity : Vector3.zero;
        targetVelocity = target.GetComponent<Rigidbody>() ? target.GetComponent<Rigidbody>().velocity : Vector3.zero;
    }

    private void FixedUpdate()
    {
        shooterPosition = shooter.transform.position;
        targetPosition = target.transform.position;

        shooterVelocity = shooter.GetComponent<Rigidbody>() ? shooter.GetComponent<Rigidbody>().velocity : Vector3.zero;
        targetVelocity = target.GetComponent<Rigidbody>() ? target.GetComponent<Rigidbody>().velocity : Vector3.zero;
        Vector3 interceptPoint = GunMathsV2.FirstOrderIntercept
        (
            shooterPosition,
            shooterVelocity,
            shotSpeed,
            targetPosition,
            targetVelocity
        );
        Debug.DrawLine(shooterPosition, interceptPoint, Color.red);
    }
   
}
