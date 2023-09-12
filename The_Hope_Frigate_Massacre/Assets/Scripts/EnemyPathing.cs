using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPathing : MonoBehaviour
{
    public bool IsStopped;
    public Transform target;
    [SerializeField] private NavMeshAgent navMesh;

    public float resetOffset = 0.3f, maxDistFromEnemy;
    public Transform enemy;

    public void Init(Transform targ)
    {
        IsStopped = false;

        target = targ;
    }

    private void Update()
    {
        if (target != null)
        {
            if (!IsStopped)
            {
                float dist = Vector3.Distance(transform.position, enemy.position);

                if (dist < maxDistFromEnemy)
                {
                    if (navMesh.isStopped)
                        navMesh.isStopped = false;

                    navMesh.SetDestination(target.position);
                }
                else
                {
                    ResetPath();
                }
            }
            else
            {
                ResetPath();
            }
        }  
    }

    public void ResetPath()
    {
        transform.position = enemy.position + enemy.forward.normalized * resetOffset;
    }

}
