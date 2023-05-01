using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Whumpus;

public class HarpoonThrower : MonoBehaviour
{
    [SerializeField] private float precision, curveMultiplier = 1;
    [SerializeField] private AnimationCurve throwCurve;
    [SerializeField] private UnityEvent OnThrowEnd, OnRecallEnd, OnCollision;

    private GameObject harpoonObject;

    private float throwSpeed, recallSpeed;

    private Vector3 originalPos, endPos;

    [HideInInspector] public bool Throwing, EndArc, Recalling;

    private float originalDist, originalY;

    private LayerMask targetMask;

    private Vector3 lastPos;

    private Harpoon harpoon;

    private void Start()
    {
        harpoon = GetComponentInChildren<Harpoon>();
        harpoon.Thrower = this;
        harpoonObject = harpoon.gameObject;
    }

    private void Update()
    {
        if (Throwing)
        {
            Vector3 target = new Vector3(endPos.x, harpoonObject.transform.position.y, endPos.z);

            float currentDist = Vector3.Distance(harpoonObject.transform.position, target);

            if (currentDist > precision)
            {
                harpoonObject.transform.position = Vector3.MoveTowards(harpoonObject.transform.position, target, throwSpeed * Time.deltaTime);

                float currentY = throwCurve.Evaluate(1 - currentDist / originalDist);
                harpoonObject.transform.position = new Vector3(harpoonObject.transform.position.x, originalY + currentY * curveMultiplier, harpoonObject.transform.position.z);

                harpoonObject.transform.right = Vector3.Normalize(lastPos - harpoonObject.transform.position);
                lastPos = harpoonObject.transform.position;
            }
            else
            {
                ThrowEnd();
            }
        }

        if (EndArc)
        {
            harpoonObject.transform.position += Vector3.Normalize(-harpoonObject.transform.right) * throwSpeed * Time.deltaTime;
        }

        if (Recalling)
        {
            float currentDist = Vector3.Distance(harpoonObject.transform.position, transform.position);

            if (currentDist > precision)
            {
                harpoonObject.transform.position = Vector3.MoveTowards(harpoonObject.transform.position, transform.position, recallSpeed * Time.deltaTime);
                harpoonObject.transform.rotation = Quaternion.RotateTowards(harpoonObject.transform.rotation, transform.rotation, recallSpeed * Time.deltaTime);
            }
            else
            {
                Restart();
                OnRecallEnd.Invoke();
            }
        }
            
    }

    public void Throw(float speed, Vector3 start, Vector3 end, LayerMask target)
    {
        throwSpeed = speed;

        CreateThrowPath(start, end);

        targetMask = target;

        harpoon.TargetMask = targetMask;

        harpoonObject.transform.parent = FindObjectOfType<BoatEffect>().transform;

        lastPos = transform.position;

        Throwing = true;
    }

    public void Recall(float speed)
    {
        recallSpeed = speed;

        Recalling = true;
    }

    private void CreateThrowPath(Vector3 start, Vector3 end)
    {
        originalPos = start;
        endPos = end;
        originalY = start.y;
        originalDist = Vector3.Distance(start, end);
    }

    private void ThrowEnd()
    {
        Throwing = false;
        OnThrowEnd.Invoke();
        EndArc = true;
    }

    public void Stop()
    {
        if (Throwing || EndArc)
        {
            OnCollision.Invoke();
        }

        Throwing = false;
        EndArc = false;
    }

    public void Restart()
    {
        Throwing = false;
        EndArc = false;
        Recalling = false;

        harpoonObject.transform.parent = transform;
        harpoonObject.transform.localPosition = Vector3.zero;
        harpoonObject.transform.localRotation = Quaternion.identity;
        harpoon.hitLimb = null;
    }
}
