using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Whumpus;

public class DiverEnemy : EnemyBehaviour
{
    [SerializeField] private int maxStunCounter;
    [SerializeField] private float throwSpeed, recallSpeed, throwRange, throwCD;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Animator animator;
    [SerializeField] private HarpoonThrower harpoonThrower;
    [SerializeField] private DiversuitRagdoll ragdoll;

    private float attackTimer;

    [SerializeField] private bool showGizmos;

    private int stunCounter;

    private bool recalled;

    [HideInInspector] public bool Dead;

    private void Awake()
    {
        ragdoll = GetComponentInChildren<DiversuitRagdoll>();
    }

    void Start()
    {
        stunCounter = maxStunCounter;

        base.Init();
    }

    void Update()
    {
        if (!Dead)
        {
            base.RotateTowardsTarget();

            if (attackTimer == 0)
            {
                bool inRange = Physics.CheckSphere(pelvis.transform.position, throwRange, playerLayer);

                if (inRange)
                {
                    FollowTarget();

                    animator.SetTrigger("Throw");
                }
            }
            else
            {
                if (attackTimer < 2 * throwCD / 3 && !recalled)
                {
                    recalled = true;
                    animator.SetTrigger("StartRecall");
                    FollowPathfinder();
                }

                if (attackTimer > 0)
                    attackTimer -= Time.deltaTime;
                else
                {
                    recalled = false;
                    attackTimer = 0;
                }
            }
        }
    }

    public void Hurt()
    {
        stunCounter--;

        if (stunCounter <= 0)
        {
            if (!Invincible)
                HP--;

            if (HP > 0)
            {
                animator.SetTrigger("Stun");
                stunCounter = maxStunCounter;
            }
            else
            {
                Death();
            }
        }
    }

    public void Throw()
    {
        harpoonThrower.Throw(throwSpeed, harpoonThrower.transform.position, targetPlayer.transform.position, playerLayer);
        attackTimer = throwCD;
        animator.ResetTrigger("Throw");
    }

    public void Recall()
    {
        harpoonThrower.Recall(recallSpeed);
    }

    public void RecallRecovery()
    {
        animator.SetTrigger("EndRecall");
    }

    [ContextMenu("Death")]
    public void Death()
    {
        Dead = true;

        ragdoll.mainRb.constraints = RigidbodyConstraints.None;
        ragdoll.EnableForces(false);
        ragdoll.ChangeWeight(0.07f);
        //pelvis.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        animator.SetTrigger("Death");
    }

    void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(pelvis.transform.position, throwRange);
        }
    }

}
