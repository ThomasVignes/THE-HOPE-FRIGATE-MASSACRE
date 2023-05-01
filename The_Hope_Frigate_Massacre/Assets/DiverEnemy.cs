using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiverEnemy : EnemyBehaviour
{
    [SerializeField] private float throwSpeed, throwRange, throwCD;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Animator animator;
    [SerializeField] private HarpoonThrower harpoonThrower;
    private float attackTimer;
    [SerializeField] private bool showGizmos;

    public float ThrowSpeed { get { return throwSpeed; } }
    public LayerMask PlayerLayer { get { return playerLayer; } }
    public HarpoonThrower HarpoonThrower { get { return harpoonThrower; } }
    public Vector3 PlayerSpot { get { return targetPlayer.transform.position; } }

    void Start()
    {
        base.Init();
    }

    void Update()
    {
        base.RotateTowardsTarget();

        if (attackTimer == 0)
        {
            bool inRange = Physics.CheckSphere(pelvis.transform.position, throwRange, playerLayer);

            if (inRange)
            {
                harpoonThrower.Restart();

                FollowTarget();

                animator.SetTrigger("Throw");

                attackTimer = throwCD;
            }
        }
        else
        {
            if (attackTimer > 0)
                attackTimer -= Time.deltaTime;
            else
                attackTimer = 0;
        }
    }

    void OnDrawGizmos()
    {
        if (showGizmos)
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(pelvis.transform.position, throwRange);
        }
    }

}
