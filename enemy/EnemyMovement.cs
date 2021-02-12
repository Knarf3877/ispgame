using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Range(0, 1)] public float Throttle = 0f;
    [Range(-1, 1)] public float Pitch = 0f;
    [Range(-1, 1)] public float Yaw = 0f;
    [Range(-1, 1)] public float Roll = 0f;
    [SerializeField] private float Multiplier = .015f;

    private float activeSpeed;
    private float forwardAccerleration = 2;
    [SerializeField] private float speedConstant = 400f;
    private float distance;

    private float lastTargetCheck;
    private float thinkDelay;
    private Vector3 forward;
    //public LayerMask obstacleMask;

    [Header("Combat")]
    public Transform FollowTarget;
    public GameObject AttackTarget;
    public bool IsFiring = false;
    public float MaxTurnPower = .6f;
    [Range(0f, 1f)]
    public float FireChance = .8f;
    public GameObject barrel;
    [SerializeField] [Range(0f, 180f)] private float gimbalRange = 30f;
    public float GimbalRange { get { return gimbalRange; } }

    //1- Standard Laser
    //2- Accurate Laser
    //3- Fast Laser
    //4- Shotgun Laser
    //5- AOE Laser

    public EnemyHitDetection weaponShot;
    public int weaponType;
    private float weaponForce;
    public float weaponSpread;
    private float weaponFireRate;
    private float timeLastFired;

    private float laser1Force = 1200f;
    private float laser1FireRate = .15f;
    [SerializeField] private float l1Spread = 0.15f;

    private float laser2Force = 1500f;
    private float laser2FireRate = .3f;
    [SerializeField] private float l2Spread = 0f;

    private float laser3Force = 1200f;
    private float laser3FireRate = .15f;
    [SerializeField] private float l3Spread = 0.5f;

    private float laser4Force = 1200f;
    private float laser4FireRate = .4f;
    [SerializeField] private float l4Spread = 1f;

    //public Target ownTarget = null;
    //public Ship ownShip = null;
    private GameObject target;
    private Vector3 targetPoint;


   // private float lastTargetCheck = 0f;
   // private float thinkDelay = 0f;

    //private List<Target> potentialTargets = new List<Target>();

    private Vector3 preferredAvoidOffset = Vector3.right * 350f;
    private float seed = 0f;

    public bool IsFireAllowed => Mathf.PerlinNoise(seed, Time.time / 10f) < FireChance;

    public bool UseGimballedAiming { get; set; }


    public void Init()
    {
        seed = Random.Range(0f, 1000f);
        preferredAvoidOffset = Random.onUnitSphere * 350f;

    }
    private void Start()
    {
        Init();
        FindTarget();
        RunAI();
        switch (weaponType)
        {
            case 1:
                weaponForce = laser1Force;
                weaponSpread = l1Spread;
                weaponFireRate = laser1FireRate;
                break;
            case 2:
                weaponForce = laser2Force;
                weaponSpread = l2Spread;
                weaponFireRate = laser2FireRate;
                break;
            case 3:
                weaponForce = laser3Force;
                weaponSpread = l3Spread;
                weaponFireRate = laser3FireRate;
                break;
            case 4:
                weaponForce = laser4Force;
                weaponSpread = l4Spread;
                weaponFireRate = laser4FireRate;
                break;

        }
        UseGimballedAiming = true;
    }
    private void Update()
    {
        if (IsFiring && (Time.time > weaponFireRate + timeLastFired))
        {
            Fire();
            timeLastFired = Time.time;
        }

    }
    public void RunAI()
    {
        RunTargeting();

        if (AttackTarget == null)
            FindTarget();
        else
        {
            DogfightTarget();
        }

        if (AttackTarget == null)
        {
            if (FollowTarget != null)
            {
                TurnTowards(FollowTarget.position);
            }
            else
            {
                Throttle = 0f;
                IsFiring = false;
                Pitch = 0f;
                Yaw = 0f;
                Roll = 0f;
            }
        }
    }
    private void FindTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        FollowTarget = target.transform;
    }
    private void RunTargeting()
    {


           
            if (AttackTarget == null)
            {
                AttackTarget = target;

            }
    }
    /* private void StrafeTarget()
     {
         if (AttackTarget == null)
             return;

         if (strafeState == StrafeState.Strafing)
         {
             Throttle = .8f;

             bool shouldExtend = AttackTarget.DistanceFromAllColliders(ownTarget.Position) < 200f;
             if (shouldExtend)
             {
                 extendPosition = ownTarget.Up * 1000f + AttackTarget.Position;
                 strafeState = StrafeState.Extending;
                 IsFiring = false;
             }
             else
             {
                 var attackPoint = AttackTarget.GetLocalToWorldPoint(strafeTargetOffset);
                 attackPoint = GunMaths.ComputeGunLead(
                     attackPoint,
                     AttackTarget.Velocity,
                     ownTarget.Position,
                     Vector3.zero,
                     ownShip.Specs.Weapons.MuzzleVelocity);

                 TurnTowards(AttackTarget.Transform.TransformPoint(strafeTargetOffset));

                 // Fire when within parameters.
                 var angleToAttack = Vector3.Angle(ownTarget.Forward, attackPoint - ownTarget.Position);
                 var distanceToAttack = Vector3.Distance(attackPoint, ownTarget.Position);
                 IsFiring = angleToAttack < 5f && distanceToAttack < 500f;
             }
         }
         else
         {
             Throttle = 1f;
             IsFiring = false;

             var distanceToExtend = Vector3.Distance(extendPosition, ownTarget.Position);
             if (distanceToExtend < 100f)
             {
                 strafeState = StrafeState.Strafing;
                 strafeTargetOffset = AttackTarget.GetRandomPointOnTarget();
             }
             else
             {
                 TurnTowards(extendPosition);
             }
         }
     }*/
     private void DogfightTarget()
     {
         if (AttackTarget == null)
             return;

         // Evasive maneuvers!
         distance = Vector3.Distance(AttackTarget.transform.position, this.transform.position);
         if (distance < 5f)
         {
             //IsFiring = false;
             TurnTowards(AttackTarget.transform.position + preferredAvoidOffset);
             Throttle = 0.4f;
         }
         else if (distance < 400f)
         {
             // Get lead on target.
             var targetPoint = GunMaths.ComputeGunLead(
                 AttackTarget.transform.position ,
                 AttackTarget.GetComponent<Rigidbody>().velocity,
                 transform.position,
                 transform.GetComponent<Rigidbody>().velocity,
                 weaponForce);

             TurnTowards(targetPoint, Mathf.PerlinNoise(seed, Time.time / 10f) + .2f * MaxTurnPower);
             var angleToTarget = Vector3.Angle(transform.forward, AttackTarget.transform.position - transform.position);

             IsFiring = angleToTarget < 20f && IsFireAllowed && distance < 400f;
         }
         else
         {
            TurnTowards(AttackTarget.transform.position);
         }
     }
   /* void Movement()
    {
        transform.GetComponent<Rigidbody>().AddRelativeTorque(Pitch *  Multiplier, Yaw *  Multiplier, -Roll * Multiplier);
        activeSpeed = Mathf.Lerp(activeSpeed, Throttle * 35, forwardAccerleration * Time.deltaTime);
        transform.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * activeSpeed * 400f * Time.deltaTime);
    }*/
    private void TurnTowards(Vector3 point, float power = 80f)
    {
        Vector3 localPosition = this.transform.InverseTransformPoint(point);

        Pitch = -localPosition.y;
        Yaw = localPosition.x;

        Pitch = Mathf.Clamp(Pitch, -12f, 12f) * power;
        Yaw = Mathf.Clamp(Yaw, -12f, 12f) * power;


        if (localPosition.z > 0f)
            Throttle = localPosition.z / 100f;
        else
            Throttle = 0.2f;
        Throttle = Mathf.Clamp(Throttle, 0f, 1f);
    }

    void FixedUpdate()
    {   
        RunAI();
        Avoid();
        Turn();
        Move();
         
    }

     void Move() 
     {
        activeSpeed = Mathf.Lerp(activeSpeed, Throttle * 35, forwardAccerleration * Time.deltaTime);
        transform.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * activeSpeed * speedConstant * Time.deltaTime);
     }

     void Turn()
     {
        transform.GetComponent<Rigidbody>().AddRelativeTorque(Pitch * Multiplier, Yaw * Multiplier, -Roll * Multiplier);
     }

    void Avoid() {
        RaycastHit hit;
        Vector3 avoidTarget =  (transform.forward).normalized;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 80f))
        {
            if (hit.transform != transform || hit.transform.tag != "Player")
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);

                avoidTarget += hit.normal * 60f;
                Debug.DrawLine(transform.position, avoidTarget, Color.blue);
                //transform.GetComponent<Rigidbody>().AddRelativeTorque(avoidTarget * Time.deltaTime * 50f);
            }
        }
        else if (distance > 90) {
            if (Time.time > lastTargetCheck + thinkDelay)
            {
                thinkDelay = Random.Range(1f, 2f);
                lastTargetCheck = Time.time;
                transform.GetComponent<Rigidbody>().AddRelativeTorque(Pitch * 3 * Multiplier, Yaw * 3 * Multiplier, 0);
            }
            
        }
        
        Vector3 leftR = transform.position;
        Vector3 rightR = transform.position;

        leftR.x -= 3;
        rightR.x += 3;

        if (Physics.Raycast(leftR, transform.forward, out hit, 100f))
        {
            if (hit.transform != transform || hit.transform.tag != "Player")
            {
                Debug.DrawLine(leftR, hit.point, Color.red);
                avoidTarget += hit.normal * 60f;
                Debug.DrawLine(transform.position, avoidTarget, Color.blue);
            }

        }
        if (Physics.Raycast(rightR, transform.forward, out hit, 100f))
        {
            if (hit.transform != transform || hit.transform.tag != "Player")
            {
                Debug.DrawLine(rightR, hit.point, Color.red);

                avoidTarget += hit.normal * 60f;
                Debug.DrawLine(transform.position, avoidTarget, Color.blue);
            }

        }
        Quaternion torotation = Quaternion.LookRotation(avoidTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, torotation, Time.deltaTime * 2f);

    }

    void Fire()
    {

        Vector3 spawnPos = barrel.transform.position;
        Quaternion aimRotation = barrel.transform.rotation;

        // Gimbal the bullet towards the target only if needed.
        if (UseGimballedAiming == true)
        {
            Vector3 gimballedVec = transform.forward;
            gimballedVec = Vector3.RotateTowards(gimballedVec,
                                                 AttackTarget.transform.position - barrel.transform.position,
                                                 Mathf.Deg2Rad * gimbalRange,
                                                 1f);
            gimballedVec.Normalize();
            aimRotation = Quaternion.LookRotation(gimballedVec);
        }


        // Instantiate and fire bullet.

        if(weaponType == 4)
        {
            for (int i = 0; i <= 7; i++)
            {
                var bullet = Instantiate(weaponShot, spawnPos, aimRotation);
                bullet.FireGun(spawnPos, aimRotation, transform.GetComponent<Rigidbody>().velocity, weaponForce, weaponSpread);
            }
        }
        else
        {
        var bullet = Instantiate(weaponShot, spawnPos, aimRotation);
        bullet.FireGun(spawnPos, aimRotation, transform.GetComponent<Rigidbody>().velocity, weaponForce, weaponSpread);
        }
    }

    public static float Remap(float fromMin, float fromMax, float toMin, float toMax, float value)
    {
        float lerp = Mathf.InverseLerp(fromMin, fromMax, value);
        return Mathf.Lerp(toMin, toMax, lerp);
    }
}

