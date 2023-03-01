using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiversuitRagdoll : MonoBehaviour
{
    [Header("Settings")]
    public float SetTo;
    public float Weight = 1f;
    public float AdditionalDamping = 0f;
    public float Mass = 1f;
    public bool LimitVelocity = true;
    public bool LimitAngularVelocity = true;
    public float MaxVelocity = 20000f;
    public float MaxAngularVelocity = 20000f;
    public bool EnableProjection = false;
    [SerializeField] private LayerMask BaseLayer, NoColLayer;

    [Header("References")]
    [SerializeField] private List<RagdollLimb> limbs = new List<RagdollLimb>();
    [SerializeField] private List<ConstantForce> forces = new List<ConstantForce>();
    [SerializeField] private GameObject root;
    public Rigidbody mainRb;

    public GameObject Root
    {
        get { return root; }
        set { root = value; }
    }

    public Action Hit;

    private void Start()
    {
        mainRb = root.GetComponent<Rigidbody>();
        RagdollLimb[] l = transform.GetComponentsInChildren<RagdollLimb>();
        foreach (var e in l)
        {
            //e.gameObject.layer = WhumpusUtilities.ToLayer(BaseLayer);
            e.ragdollManager = this;

            limbs.Add(e);

            e.LimitVelocity = LimitVelocity;
            e.MaxVelocity = MaxVelocity;
            e.LimitAngularVelocity = LimitAngularVelocity;
            e.MaxAngularVelocity = MaxAngularVelocity;
            e.ProjectionEnabled = EnableProjection;
            if (e.AdditionalDamping)
            {
                e.AdditionalDamper = AdditionalDamping;
            }

            e.Initialize();
        }

        ConstantForce[] f = transform.GetComponentsInChildren<ConstantForce>();
        foreach (var i in f)
        {
            forces.Add(i);
        }

        ChangeWeight(Weight);
    }

    public void SetEntityCollisions(bool active)
    {
        foreach (var item in limbs)
        {
            if (active)
                item.gameObject.layer = WhumpusUtilities.ToLayer(BaseLayer);
            else
                item.gameObject.layer = WhumpusUtilities.ToLayer(NoColLayer);
        }
    }

    public void ChangeWeight(float w)
    {
        Weight = w;

        UpdateLimbs();
    }

    public void Simulate(bool simulate)
    {
        foreach (var limb in limbs)
        {
            limb.Simulated = simulate;
        }
    }

    public void ChangeMass(float m)
    {
        Mass = m;

        UpdateLimbMass();
    }

    public void EnableForces(bool state)
    {
        foreach (var f in forces)
        {
            f.enabled = state;
        }
    }

    private void UpdateLimbs()
    {
        foreach (var limb in limbs)
        {
            limb.JointWeight = Weight;
            limb.UpdateJoint();
        }
    }

    private void UpdateLimbMass()
    {
        foreach (var limb in limbs)
        {
            limb.Mass = Mass;
            limb.UpdateRb();
        }
    }

    public void AddForce(Vector3 force, bool resetVelocity)
    {
        foreach (var limb in limbs)
        {
            if (resetVelocity)
                limb.rb.velocity = Vector3.zero;

            limb.rb.AddForce(force);
        }
    }

    public void AddDelayedForce(Vector3 force, bool resetVelocity)
    {
        Hit?.Invoke();
        foreach (var limb in limbs)
        {
            if (resetVelocity)
                limb.rb.velocity = Vector3.zero;

            SlowMoEffector.Instance.Hit(limb.rb, force.magnitude, force.normalized);
        }
    }

    public void IgnoreEntities(bool yes)
    {
        if (yes)
        {
            foreach (var limb in limbs)
            {
                limb.gameObject.layer = WhumpusUtilities.ToLayer(NoColLayer);
            }
        }
        else
        {
            foreach (var limb in limbs)
            {
                limb.gameObject.layer = WhumpusUtilities.ToLayer(BaseLayer);
            }
        }
    }

    [ContextMenu("Die")]
    public void Die()
    {
        EnableForces(false);

        foreach (var limb in limbs)
        {
            limb.gameObject.layer = 0;

            ChangeWeight(0f);

            limb.rb.velocity = Vector3.zero;
            limb.rb.AddForce(root.transform.forward * 1000);
        }
    }

    [ContextMenu("Explode")]
    public void Explode()
    {
        EnableForces(false);

        foreach (var limb in limbs)
        {
            limb.gameObject.layer = 0;

            if (limb.CanBeCut)
                limb.CutLimb();

            limb.rb.velocity = Vector3.zero;
            limb.rb.AddExplosionForce(90f, root.transform.position, 10f);
        }
    }

    [ContextMenu("Rebuild")]
    public void Rebuild()
    {
        EnableForces(true);

        foreach (var limb in limbs)
        {
            limb.gameObject.layer = WhumpusUtilities.ToLayer(BaseLayer);

            if (limb.CanBeCut)
                limb.Reattatch();

            limb.rb.velocity = Vector3.zero;
        }
    }
}
