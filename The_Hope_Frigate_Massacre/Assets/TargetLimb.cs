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
                SlowMoEffector.Instance.Hit(limb.rb, force, dir);

                limb.CutLimb();
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
