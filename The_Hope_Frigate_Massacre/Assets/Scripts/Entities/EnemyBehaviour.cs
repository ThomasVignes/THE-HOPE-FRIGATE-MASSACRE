using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public int HP;
    public List<Hitbox> hitboxes = new List<Hitbox>();
    public GameObject pelvis;
    public EnemyPathing pathingObject;
    public Transform pathfinderLookAt;

    [HideInInspector] GameObject followTarget;

    //Debug
    public GameObject targetPlayer;

    public void Init()
    {
        pathingObject.Init(targetPlayer.transform);

        FollowPathfinder();
    }

    public void RotateTowardsTarget()
    {
        Vector3 target = new Vector3(followTarget.transform.position.x, pelvis.transform.position.y, followTarget.transform.position.z);

        if (followTarget == pathingObject.gameObject)
        {
            pathfinderLookAt.LookAt(target);
            pelvis.transform.rotation = Quaternion.Lerp(pelvis.transform.rotation, pathfinderLookAt.rotation, 0.17f);
        }
        else
        {
            pelvis.transform.LookAt(target);
        }
    }

    public void FollowPathfinder()
    {
        followTarget = pathingObject.gameObject;
    }

    public void FollowTarget()
    {
        followTarget = targetPlayer;
    }
}
