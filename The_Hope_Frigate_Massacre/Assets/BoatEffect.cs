using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatEffect : MonoBehaviour
{
    [SerializeField] private float tiltSpeed;
    [SerializeField] private float maxTiltAngle;

    private void Start()
    {
        transform.Rotate(0, 0, -maxTiltAngle);
    }

    void Update()
    {
        transform.Rotate(0, 0, Mathf.Sin(tiltSpeed * Time.time) * maxTiltAngle * Time.deltaTime, Space.World);
    }
}
