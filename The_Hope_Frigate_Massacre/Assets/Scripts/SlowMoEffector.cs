
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ViewportState
{
    Perspective,
    ApeOut,
    SideScroller
}
public class SlowMoEffector  : MonoBehaviour
{
    public static SlowMoEffector Instance;
    private List<Rigidbody> hitRbs = new List<Rigidbody>();

    [Header("General Settings")]
    public LayerMask PlayerMask;

    [Header("SlowMo")]
    public float slowMotionTimescale, slowMotionDuration;

    private float startTimescale;
    private float startFixedDeltaTime;
    private bool slowingDown;
    private Vector3 currentDir;

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        startTimescale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;
    }


    public void SlowMo()
    {
        if (!slowingDown)
            StartCoroutine(SlowMoEffect());
    }

    IEnumerator SlowMoEffect()
    {
        StartSlowMotion();
        slowingDown = true;
        yield return new WaitForSecondsRealtime(slowMotionDuration);
        StopSlowMotion();

        slowingDown = false;
    }


    public void Hit(Rigidbody rb, float force, Vector3 dir)
    {
        if (!hitRbs.Contains(rb))
            hitRbs.Add(rb);

        if (!slowingDown)
        {
            StartCoroutine(SlowMoHit(force, dir));
        }
    }

    IEnumerator SlowMoHit(float force, Vector3 dir)
    {
        StartSlowMotion();
        slowingDown = true;
        yield return new WaitForSecondsRealtime(slowMotionDuration);
        StopSlowMotion();

        foreach (var rb in hitRbs)
        {
            rb.AddForce(force * dir);
        }
        slowingDown = false;

        hitRbs.Clear();
    }

    public void StartSlowMotion()
    {
        Time.timeScale = slowMotionTimescale;
        //Time.fixedDeltaTime = startFixedDeltaTime * slowMotionTimescale;
    }

    public void StopSlowMotion()
    {
        Time.timeScale = startTimescale;
        //Time.fixedDeltaTime = startFixedDeltaTime;
    }
}
