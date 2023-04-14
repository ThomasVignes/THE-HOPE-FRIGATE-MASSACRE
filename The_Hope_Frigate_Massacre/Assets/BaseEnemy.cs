using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private float AttackRange, AttackCD;
    [SerializeField] private GameObject pelvis, player;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Animator animator;
    Collider[] cols;
    private float attackTimer;

    private void Update()
    {
        if (attackTimer == 0)
        {
            bool inRange = Physics.CheckSphere(pelvis.transform.position, AttackRange, playerLayer);

            if (inRange)
            {
                animator.SetTrigger("Run");
                attackTimer = AttackCD;
            }
        }
        else
        {
            if (attackTimer > 0)
                attackTimer -= Time.deltaTime;
            else
                attackTimer = 0;
        }

        Vector3 target = new Vector3(player.transform.position.x, pelvis.transform.position.y, player.transform.position.z);
        pelvis.transform.LookAt(target);
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(pelvis.transform.position, AttackRange);
    }
}
