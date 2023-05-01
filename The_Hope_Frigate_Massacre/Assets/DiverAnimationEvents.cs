using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiverAnimationEvents : MonoBehaviour
{
    [SerializeField] private DiverEnemy diver;

    public void Throw()
    {
        diver.Throw();
    }

    public void RemoveBlood()
    {
        Blood[] blood = diver.GetComponentsInChildren<Blood>();

        foreach (var b in blood)
        {
            Destroy(b.gameObject);
        }
    }

    public void StartRecall()
    {
        diver.Recall();
    }
}
