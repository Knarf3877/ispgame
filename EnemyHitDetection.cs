using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitDetection : MonoBehaviour
{
    private bool useFixedUpdate = true;
    [SerializeField] private LayerMask hitMask = -1;

    public ParticleSystem explodeFX;
    private Vector3 velocity = Vector3.forward;
    private float destructionTime = 0f;
    public float lifeTime = 4f;
    private bool isFired = false;
    private const float kVelocityMult = 1f;
    public float damage = 10f;

    [SerializeField] private bool adjustLaser;

    void Start()
    {
        if (adjustLaser)
        {
            transform.Rotate(Vector3.left * 90);
        }
    }

    void Update()
    {
        if (isFired == false)
            return;
        if (Time.time > destructionTime)
            DestroyBullet(transform.position, false);
    }
    void FixedUpdate()
    {
        if (isFired == false)
            return;

        if (useFixedUpdate == true)
        {
            MoveBullet(damage);
        }
    }
    public void FireGun(Vector3 position, Quaternion rotation, Vector3 inheritedVelocity, float muzzleVelocity, float spread)
    {

        transform.position = position;
        Vector3 spreadAngle = Vector3.zero;
        spreadAngle.x = Random.Range(-spread, spread);
        spreadAngle.y = Random.Range(-spread, spread);
        Quaternion deviationRotation = Quaternion.Euler(spreadAngle);

        transform.rotation = rotation * deviationRotation;

        velocity = (transform.forward * muzzleVelocity) + inheritedVelocity;
        destructionTime = Time.time + lifeTime;
        isFired = true;
    }
    public void MoveBullet(float damage)
    {
        RaycastHit rayHit;
        Ray velocityRay = new Ray(transform.position, velocity.normalized);
        bool rayHasHit = Physics.Raycast(velocityRay, out rayHit, velocity.magnitude * kVelocityMult * Time.deltaTime, hitMask);

        if (rayHasHit == true)
        {

            GameObject rayHitGameObject = rayHit.transform.gameObject;
            DamagePlayer target = rayHitGameObject.GetComponent<DamagePlayer>();
            //  bool hitSelf = false;
            if (target)
            {
                print("Hit " + target.name + " with " + DamagePlayer.playerHealth + " HP left.");
                target.ApplyDamage(damage);
            }

            DestroyBullet(rayHit.point, true);
        }
        else
        {
            // Bullet didn't hit anything, continue moving.
            transform.Translate(velocity * Time.deltaTime, Space.World);


        }

    }
    public void DestroyBullet(Vector3 position, bool fromImpact)
    {
        if (fromImpact == true && explodeFX != null)
        {
            ParticleSystem tempExplodeFX = Instantiate(explodeFX, transform.position, transform.rotation);
            Destroy(tempExplodeFX.gameObject, 1f);

        }
        Destroy(gameObject);
    }
}
