using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Whumpus;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private int HP;
    [SerializeField] private float AttackRange, RunRange, AttackCD;
    [SerializeField] private List<Hitbox> hitboxes = new List<Hitbox>();
    [SerializeField] private GameObject pelvis, player;
    [SerializeField] private DiversuitRagdoll ragdoll;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Animator animator;
    Collider[] cols;
    private float attackTimer;
    private RagdollLimb grabbedLimb;

    [HideInInspector] public bool Dead;

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

        /*
        bool inRange = Physics.CheckSphere(pelvis.transform.position, AttackRange, playerLayer);
        if (inRange)
        {
            animator.SetTrigger("Grab");

            foreach (var item in hitboxes)
            {
                item.Activate(1.3f);
            }

            attackTimer = AttackCD/3;
        }
        */

        if (attackTimer == 0)
        {
            bool inRange = Physics.CheckSphere(pelvis.transform.position, RunRange, playerLayer);

            if (inRange)
            {
                DropLimb();

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

    private void DropLimb()
    {
        if (grabbedLimb != null)
        {
            Collider col = grabbedLimb.GetComponent<Collider>();
            if (col != null)
                col.enabled = true;

            grabbedLimb.transform.parent = null;

            grabbedLimb.rb.constraints = RigidbodyConstraints.None;

            grabbedLimb = null;
        }
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
                RagdollLimb hitLimb = item.LastHitObject.GetComponent<RagdollLimb>();
                if (hitLimb != null)
                {
                    if (!hitLimb.IsCut)
                    {
                        if (hitLimb.transform.IsChildOf(hitLimb.ragdollManager.transform))
                        {
                            CameraEffectsManager.Instance.ScreenShake();
                            animator.SetTrigger("Dismember");

                            hitLimb.ragdollManager.transform.parent.GetComponent<Dismemberer>().DismemberSpecific(out cutLimb);

                            grabbedLimb = cutLimb;
                            Collider col = grabbedLimb.GetComponent<Collider>();
                            if (col != null)
                                col.enabled = false;
                            parent = item.transform;
                            caught = true;
                        }
                    }
                }
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

        if (HP <= 0 && !Dead)
        {
            Dead = true;
            animator.SetBool("Dead", true);
            StartCoroutine(DieCoroutine());
        }
    }

    IEnumerator DieCoroutine()
    {
        ragdoll.ChangeWeight(0.07f);

        float duration = Random.Range(1.3f, 2.7f);

        yield return new WaitForSeconds(duration);

        Die();
    }
    public void Die()
    {
        DropLimb();

        animator.SetBool("Walking", false);
        var rb = pelvis.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.AddRelativeForce(-Vector3.forward * 500f);

        ragdoll.Explode();
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(pelvis.transform.position, RunRange);
    }
}
