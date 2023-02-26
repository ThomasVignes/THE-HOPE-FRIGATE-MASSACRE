using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyLimb : MonoBehaviour
{
    public bool Simulated = true;
    public bool AdditionalDamping = true;
    public float JointWeight, AdditionalDamper, Mass;
    public bool LimitVelocity, LimitAngularVelocity;
    public float MaxVelocity, MaxAngularVelocity;
    public bool ProjectionEnabled;
    public bool CanBeCut = true;
    public bool Monitor = false;

    [SerializeField] private Transform targetLimb;
    [HideInInspector]
    public DiversuitRagdoll ragdollManager;
    [HideInInspector]
    public ConfigurableJoint m_ConfigurableJoint;
    [HideInInspector]
    public Rigidbody rb, connectedRb;
    [HideInInspector] Transform parent;
    private float initialMass, initialXDrive, initialXDamp, initialYZDrive, initialYZDamp, maxXForce, maxYZForce;

    public Transform TargetLimb
    {
        get { return targetLimb; }
        set { targetLimb = value; }
    }


    Quaternion targetInitialRotation;

    public void Initialize()
    {
        m_ConfigurableJoint = this.GetComponent<ConfigurableJoint>();
        rb = this.GetComponent<Rigidbody>();

        targetInitialRotation = this.targetLimb.transform.localRotation;

        if (Simulated)
        {
            initialMass = rb.mass;
            initialXDrive = m_ConfigurableJoint.angularXDrive.positionSpring;
            initialXDamp = m_ConfigurableJoint.angularXDrive.positionDamper + AdditionalDamper;
            maxXForce = m_ConfigurableJoint.angularXDrive.maximumForce;
            initialYZDrive = m_ConfigurableJoint.angularYZDrive.positionSpring;
            initialYZDamp = m_ConfigurableJoint.angularYZDrive.positionDamper + AdditionalDamper;
            maxYZForce = m_ConfigurableJoint.angularYZDrive.maximumForce;

            if (ProjectionEnabled)
                m_ConfigurableJoint.projectionMode = JointProjectionMode.PositionAndRotation;
            else
                m_ConfigurableJoint.projectionMode = JointProjectionMode.None;

            connectedRb = m_ConfigurableJoint.connectedBody;
        }
        else
        {
            if (m_ConfigurableJoint != null)
                Destroy(m_ConfigurableJoint);

            if (rb != null)
                Destroy(rb);
        }
        parent = transform.parent;
    }

    private void Update()
    {
        if (!Simulated)
        {
            transform.localRotation = targetLimb.transform.localRotation;
        }
    }

    private void FixedUpdate()
    {
        if (m_ConfigurableJoint != null && Simulated)
        {
            m_ConfigurableJoint.targetRotation = CopyRotation();

            if (LimitVelocity)
            {
                if (m_ConfigurableJoint.targetVelocity.magnitude > MaxVelocity)
                {
                    m_ConfigurableJoint.targetVelocity = m_ConfigurableJoint.targetVelocity.normalized * MaxVelocity;
                }
            }

            if (LimitAngularVelocity)
            {
                if (m_ConfigurableJoint.targetAngularVelocity.magnitude > MaxAngularVelocity)
                {
                    m_ConfigurableJoint.targetAngularVelocity = m_ConfigurableJoint.targetAngularVelocity.normalized * MaxAngularVelocity;
                }
            }
        }
    }


    public void UpdateJoint()
    {
        if (Simulated)
        {
            JointDrive XDrive = new JointDrive();
            JointDrive YZDrive = new JointDrive();
            XDrive.positionSpring = initialXDrive * JointWeight;
            XDrive.positionDamper = initialXDamp * JointWeight;
            XDrive.maximumForce = maxXForce;
            YZDrive.positionSpring = initialYZDrive * JointWeight;
            YZDrive.positionDamper = initialYZDamp * JointWeight;
            YZDrive.maximumForce = maxYZForce;

            m_ConfigurableJoint.angularXDrive = XDrive;
            m_ConfigurableJoint.angularYZDrive = YZDrive;
        }
    }

    public void UpdateRb()
    {
        if (Simulated)
        {
            rb.mass = initialMass * Mass;
        }
    }

    private Quaternion CopyRotation() {
        return Quaternion.Inverse(targetLimb.localRotation) * targetInitialRotation;
    }

    [ContextMenu("Cut")]
    public void CutLimb()
    {
        //m_ConfigurableJoint.connectedBody = null;
        transform.parent = null;
        if (Simulated)
        {
            m_ConfigurableJoint.xMotion = ConfigurableJointMotion.Free;
            m_ConfigurableJoint.yMotion = ConfigurableJointMotion.Free;
            m_ConfigurableJoint.zMotion = ConfigurableJointMotion.Free;
        }
    }

    [ContextMenu("Reattach")]
    public void Reattatch()
    {
        //m_ConfigurableJoint.connectedBody = connectedRb;
        transform.parent = parent;
        if (Simulated)
        {
            m_ConfigurableJoint.xMotion = ConfigurableJointMotion.Locked;
            m_ConfigurableJoint.yMotion = ConfigurableJointMotion.Locked;
            m_ConfigurableJoint.zMotion = ConfigurableJointMotion.Locked;
        }
    }
}
