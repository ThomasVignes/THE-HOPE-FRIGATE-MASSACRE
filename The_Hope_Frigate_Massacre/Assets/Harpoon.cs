using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Whumpus;

public class Harpoon : MonoBehaviour
{
    [HideInInspector] public HarpoonThrower Thrower;

    [HideInInspector] public LayerMask TargetMask;

    [HideInInspector] public RagdollLimb hitLimb;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == WhumpusUtilities.ToLayer(TargetMask))
        {
            if (hitLimb == null)
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

                            hitLimb.ragdollManager.transform.parent.GetComponent<Dismemberer>().DismemberSpecific(out cutLimb);

                            Collider col = cutLimb.GetComponent<Collider>();

                            if (col != null)
                                col.enabled = false;

                            Rigidbody rb = cutLimb.GetComponent<Rigidbody>();

                            if (rb != null)
                                rb.isKinematic = true;

                            cutLimb.transform.parent = transform;
                            cutLimb.transform.localPosition = Vector3.zero;
                        }

                    }
                }
            }
        }
        else
        {
            Thrower.Stop();
        }
    }
}
