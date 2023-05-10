using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Whumpus;

public class Harpoon : MonoBehaviour
{
    [HideInInspector] public HarpoonThrower Thrower;

    [HideInInspector] public LayerMask TargetMask;

    [HideInInspector] public RagdollLimb hitLimb;

    [HideInInspector] public bool Ready;

    public void Cut()
    {
        if (hitLimb != null)
        {
            RagdollLimb cutLimb = null;
            hitLimb.ragdollManager.transform.parent.GetComponent<Dismemberer>().DismemberSpecific(out cutLimb);

            cutLimb.transform.parent = transform;
            cutLimb.transform.localPosition = Vector3.zero;

            hitLimb = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == WhumpusUtilities.ToLayer(TargetMask))
        {
            if (hitLimb == null && Ready)
            {
                hitLimb = other.GetComponent<RagdollLimb>();

                if (hitLimb != null)
                {
                    if (!hitLimb.IsCut)
                    {
                        if (hitLimb.transform.IsChildOf(hitLimb.ragdollManager.transform))
                        {
                            RagdollLimb cutLimb;

                            CameraEffectsManager.Instance.ScreenShake();

                            //hitLimb.ragdollManager.transform.parent.GetComponent<Dismemberer>().DismemberSpecific(out cutLimb);
                            hitLimb.ragdollManager.transform.parent.GetComponent<Dismemberer>().GetSpecific(out cutLimb);

                            Collider col = cutLimb.GetComponent<Collider>();

                            if (col != null)
                                col.enabled = false;

                            Rigidbody rb = cutLimb.GetComponent<Rigidbody>();

                            if (rb != null)
                                rb.isKinematic = true;

                            cutLimb.transform.parent = transform;
                            cutLimb.transform.localPosition = Vector3.zero;

                            hitLimb = cutLimb;

                            Ready = false;
                        }

                    }
                }
            }
        }
        else
        {
            Ready = false;

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(-transform.right.normalized * 100f);

            Thrower.Stop();
        }
    }
}
