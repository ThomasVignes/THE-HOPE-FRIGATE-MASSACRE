using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Whumpus;

public class Dismemberer : MonoBehaviour
{
    [SerializeField] private List<LimbType> typePriority = new List<LimbType>();
    [SerializeField] private DiversuitRagdoll ragdollManager;

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
    public void Dismember(out RagdollLimb cutLimb)
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
}
