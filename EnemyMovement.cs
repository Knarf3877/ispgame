using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Range(0, 1)] public float Throttle = 0f;
    [Range(-1, 1)] public float Pitch = 0f;
    [Range(-1, 1)] public float Yaw = 0f;
    [Range(-1, 1)] public float Roll = 0f;
    private const float Multiplier = .015f;

    private float activeSpeed;
    private float forwardAccerleration = 2;

    [Header("Combat")]
    public Transform FollowTarget = null;
    public GameObject AttackTarget = null;
    public bool IsFiring = false;
    public float MaxTurnPower = 100f;
    [Range(0f, 1f)]
    public float FireChance = 1f;
    public GameObject barrel;

    public EnemyHitDetection weaponShot;
    public int weaponType;
    private float weaponForce;
    public float weaponSpread;
    private float weaponFireRate;
    private float timeLastFired;

    private float laser1Force = 1500f;
    private float laser1FireRate = .2f;
    [SerializeField] private float l1Spread = 0.15f;

    private float laser2Force = 2500f;
    private float laser2FireRate = .4f;
    [SerializeField] private float l2Spread = 0f;

    private float laser3Force = 1500f;
    private float laser3FireRate = .1f;
    [SerializeField] private float l3Spread = 0.5f;

    //public Target ownTarget = null;
    //public Ship ownShip = null;
    private GameObject target;

    private StrafeState strafeState = StrafeState.Strafing;
    private Vector3 strafeTargetOffset = Vector3.zero;
    private Vector3 extendPosition = Vector3.zero;

    private float lastTargetCheck = 0f;
    private float thinkDelay = 0f;

    //private List<Target> potentialTargets = new List<Target>();

    private Vector3 preferredAvoidOffset = Vector3.right * 350f;
    private Vector3 currentTurnTowards;
    private float seed = 0f;

    public bool IsFireAllowed => Mathf.PerlinNoise(seed, Time.time / 10f) < FireChance;

    private enum StrafeState
    {
        Extending,
        Strafing
    }

    public void Init()
    {
        seed = Random.Range(0f, 1000f);
        preferredAvoidOffset = Random.onUnitSphere * 350f;
    }
    private void Start()
    {
        Init();
        FindTarget();
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
        }
    }
    private void Update()
    {
        if (IsFiring && (Time.time > weaponFireRate + timeLastFired))
        {
            Fire();
            timeLastFired = Time.time;
        }
    }
    private void FixedUpdate()
    {
        RunAI();
        Movement();
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
    private void RunTargeting()
    {
        if (Time.time > lastTargetCheck + thinkDelay)
        {
            thinkDelay = Random.Range(1f, 2f);
            lastTargetCheck = Time.time;

            if (AttackTarget == null)
            {
                AttackTarget = target;

                //FindTarget();
                /*if (AttackTarget != null && AttackTarget.IsCapital)
                    strafeTargetOffset = AttackTarget.GetRandomPointOnTarget();*/
            }
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
         var distance = Vector3.Distance(AttackTarget.transform.position, this.transform.position);
         if (distance < 5f)
         {
             IsFiring = false;
             //TurnTowards(AttackTarget.transform.position + preferredAvoidOffset);
             TurnTowards(AttackTarget.transform.position + preferredAvoidOffset);
             Throttle = 0.4f;
         }
         else if (distance < 300f)
         {
             // Get lead on target.
             var targetPoint = GunMaths.ComputeGunLead(
                 AttackTarget.transform.position ,
                 AttackTarget.GetComponent<Rigidbody>().velocity,
                 transform.position,
                 transform.GetComponent<Rigidbody>().velocity,
                 weaponForce);

             TurnTowards(targetPoint, Mathf.PerlinNoise(seed, Time.time / 10f) * MaxTurnPower);
             var angleToTarget = Vector3.Angle(transform.forward, AttackTarget.transform.position - transform.position);

             IsFiring = angleToTarget < 20f && IsFireAllowed && distance < 300f;

/*             if (Vector3.Angle(AttackTarget.transform.forward, transform.forward) < 90)
                        {
                            // Adjust throttle based on distance so faster ships don't overtake slower ones.
                            Throttle = Remap(50f, 250f, .33f, .8f, distance);
                        }
                        else
                            Throttle = 0.85f;*/
         }
         else
         {
            TurnTowards(AttackTarget.transform.position);
         }
     }
    void Movement()
    {
        transform.GetComponent<Rigidbody>().AddRelativeTorque(Pitch *  Multiplier, Yaw *  Multiplier, -Roll * Multiplier);
        activeSpeed = Mathf.Lerp(activeSpeed, Throttle * 35, forwardAccerleration * Time.deltaTime);
        transform.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * activeSpeed * 400f * Time.deltaTime);
        //Vector3 predicted = transform.position + transform.forward * 300f * Throttle * Time.deltaTime;
        //transform.position = Vector3.Slerp(transform.position, predicted, 10f);
    }
    private void TurnTowards(Vector3 point, float power = 75f)
    {
        Vector3 localPosition = this.transform.InverseTransformPoint(point);

        Pitch = -localPosition.y;
        Yaw = localPosition.x;

        Pitch = Mathf.Clamp(Pitch, -1f, 1f) * power;
        Yaw = Mathf.Clamp(Yaw, -1f, 1f) * power;

        // Capitals auto-level.
        /* if (ownShip.Specs.IsCapital)
             Roll = Mathf.Clamp(ownShip.transform.right.y * 5f, -1f, 1f);*/

        if (localPosition.z > 0f)
            Throttle = localPosition.z / 100f;
        else
            Throttle = 0.2f;
        Throttle = Mathf.Clamp(Throttle, 0f, 1f);
    }

    private void FindTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        FollowTarget = target.transform;

        /* potentialTargets.Clear();
         foreach (var target in Target.AllTargets)
         {
             if (target == ownTarget)
                 continue;
             if (target.Faction != ownTarget.Faction)
                 potentialTargets.Add(target);
         }

         // Pick random one.
         if (potentialTargets.Count > 0)
             return potentialTargets[Random.Range(0, potentialTargets.Count)];
         else
             return null;*/
    }


    /* [SerializeField]Transform target;
     [SerializeField] float rotationDamp = .3f;
     [SerializeField] float movementSpeed = 20;



     // Update is called once per frame
     void Update()
     {
         Turn();
         Move();
     }

     void Move() 
     {
         transform.position += transform.forward * Time.deltaTime * movementSpeed;
     }

     void Turn()
     {
         Vector3 pos = target.position - transform.position;
         Quaternion rotation = Quaternion.LookRotation(pos);
         transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamp);
     }
 */
    void Fire()
    {
        var hitDetection = Instantiate(weaponShot, this.transform.position, this.transform.rotation);
        hitDetection.FireGun(this.transform.position, this.transform.rotation, transform.GetComponent<Rigidbody>().velocity, weaponForce, weaponSpread);
    }

    public static float Remap(float fromMin, float fromMax, float toMin, float toMax, float value)
    {
        float lerp = Mathf.InverseLerp(fromMin, fromMax, value);
        return Mathf.Lerp(toMin, toMax, lerp);
    }
}

