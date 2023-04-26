using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Whumpus;

public class Dismemberer : MonoBehaviour
{
    [SerializeField] private List<LimbType> typePriority = new List<LimbType>();
    [SerializeField] private List<RagdollLimb> limbsPriority = new List<RagdollLimb>();
    [SerializeField] private DiversuitRagdoll ragdollManager;

    public UnityEvent OnLastLimbLost;

    public void Dismember()
    {
        for (int i = 0; i < typePriority.Count; i++)
        {
            bool hasCut = false;
            foreach (var limb in ragdollManager.Limbs)
            {
                if (limb.Type == typePriority[i])
                {
                    limb.CutLimb();
                    hasCut = true;
                    break;
                }
            }

            if (hasCut)
                break;
        }
    }
    public void DismemberType(out RagdollLimb cutLimb)
    {
        cutLimb = null;

        for (int i = 0; i < typePriority.Count; i++)
        {
            bool hasCut = false;
            foreach (var limb in ragdollManager.Limbs)
            {
                if (limb.Type == typePriority[i])
                {
                    limb.CutLimb();
                    cutLimb = limb;
                    hasCut = true;
                    break;
                }
            }

            if (hasCut)
                break;
        }
    }

    public void DismemberSpecific(out RagdollLimb cutLimb)
    {
        cutLimb = null;

        foreach (var limb in limbsPriority)
        {
            if (!limb.IsCut)
            {
                limb.CutLimb();
                cutLimb = limb;

                if (limb == limbsPriority[limbsPriority.Count - 1])
                    OnLastLimbLost.Invoke();

                break;
            }


        }
    }
}
