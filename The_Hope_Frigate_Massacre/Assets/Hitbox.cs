using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Whumpus;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Hitbox : MonoBehaviour
{
    public LayerMask HitLayer;

    public bool DeactivateOnImpact = false, MultipleHits = false;
    public bool OnlyHitsLimbs = false;
    public float MaxHits;

    public UnityEvent OnHit;

    [HideInInspector] public GameObject LastHitObject;

    public bool IsActive
    {
        get { return col.enabled; }
    }

    private Collider col;
    private Rigidbody rb;
    private float activeTimer;

    private List<GameObject> hitObjects = new List<GameObject>();


    private void Awake()
    {
        col = GetComponent<Collider>();

        col.isTrigger = true;

        col.enabled = false;


        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void Update()
    {
        if (activeTimer > 0)
            activeTimer -= Time.deltaTime;
        else if (IsActive)
        {
            Deactivate();
        }
    }

    public void Activate(float timer)
    {
        col.enabled = true;

        activeTimer = timer;
    }

    public void Deactivate()
    {
        col.enabled = false;

        hitObjects.Clear();
    }


    private void OnTriggerEnter(Collider other)
    {
        GameObject hitObject = other.gameObject;

        if (hitObject.layer == WhumpusUtilities.ToLayer(HitLayer))
        {
            bool CanHit = true;

            if (OnlyHitsLimbs)
            {
                if (!hitObject.GetComponent<RagdollLimb>())
                {
                    CanHit = false;
                }
            }

            if (!MultipleHits)
            {
                if (hitObjects.Contains(hitObject))
                {
                    CanHit = false;
                }
            }

            if (CanHit)
                Hit(hitObject);
        }
    }

    public void Hit(GameObject hitObject)
    {
        LastHitObject = hitObject;
        OnHit.Invoke();
        

        if (DeactivateOnImpact)
            Deactivate();

        if (!MultipleHits)
        {
            hitObjects.Add(hitObject);
        }
    }
}
