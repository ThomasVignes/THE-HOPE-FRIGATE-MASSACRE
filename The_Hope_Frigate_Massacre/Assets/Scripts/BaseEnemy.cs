using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Whumpus;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private int HP;
    [SerializeField] private float AttackRange, AttackCD;
    [SerializeField] private List<Hitbox> hitboxes = new List<Hitbox>();
    [SerializeField] private GameObject pelvis, player;
    [SerializeField] private DiversuitRagdoll ragdoll;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Animator animator;
    Collider[] cols;
    private float attackTimer;

    private void Awake()
    {
        Hitbox[] hbs = GetComponentsInChildren<Hitbox>();

        foreach (var item in hbs)
        {
            hitboxes.Add(item);
        }
    }

    private void Update()
    {
        if (attackTimer == 0)
        {
            bool inRange = Physics.CheckSphere(pelvis.transform.position, AttackRange, playerLayer);

            if (inRange)
            {
                animator.SetTrigger("Run");

                foreach (var item in hitboxes)
                {
                    item.Activate(4f);
                }

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

    public void DismemberLimb()
    {
        bool caught = false;

        RagdollLimb cutLimb = null;

        Transform parent = null;

        foreach (var item in hitboxes)
        {
            if (item.LastHitObject != null && !caught)
            {
                //item.LastHitObject.GetComponent<RagdollLimb>().ragdollManager.transform.parent.GetComponent<Dismemberer>().Dismember(out cutLimb);
                cutLimb = item.LastHitObject.GetComponent<RagdollLimb>();
                cutLimb.CutLimb();
                parent = item.transform;
                caught = true;
            }

            item.Deactivate();
        }

        
        if (cutLimb != null && parent != null)
        {
            cutLimb.transform.parent = parent;
            cutLimb.rb.constraints = RigidbodyConstraints.FreezeAll;
            cutLimb.transform.localPosition = new Vector3(-0.16f, 0, 0);
        }
        
    }

    public void Hurt()
    {
        HP--;

        if (HP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        animator.SetBool("Walking", false);
        animator.SetBool("Dead", true);
        var rb = pelvis.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.AddRelativeForce(-Vector3.forward * 500f);

        ragdoll.Explode();
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(pelvis.transform.position, AttackRange);
    }
}
