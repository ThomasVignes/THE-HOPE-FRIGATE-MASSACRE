using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Whumpus;

public class TargetLimb : MonoBehaviour
{
    public bool Resistant;
    public int HP = 4;
    public RagdollLimb limb;
    public bool HasBeenCut;
    public UnityEvent OnCut;

    private void Awake()
    {
        limb = GetComponent<RagdollLimb>();    
    }

    public void Hit(int damage, float force, Vector3 dir)
    {
        if (!HasBeenCut && ! Resistant)
        {
            HP -= damage;

            if (HP <= 0)
            {
                HasBeenCut = true;
                OnCut.Invoke();
                SlowMoEffector.Instance.Hit(limb.rb, force/4, dir);

                BloodManager.Instance.SpawnBlood(limb.transform.position, limb.transform.parent.position + Vector3.Normalize(limb.transform.position - limb.transform.parent.position) * 0.2f, limb.transform.parent);

                limb.CutLimb();

                Blood[] bloods = GetComponentsInChildren<Blood>();

                foreach (var item in bloods)
                {
                    Destroy(item.gameObject);
                }
            }
            else
            {
                SlowMoEffector.Instance.Hit(limb.rb, force, dir);
            }
        }
        else
        {
            SlowMoEffector.Instance.Hit(limb.rb, force, dir);
        }
    }
}
