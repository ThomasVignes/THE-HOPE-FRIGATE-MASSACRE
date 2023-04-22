using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffectsManager : MonoBehaviour
{
    public static CameraEffectsManager Instance;

    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void ScreenShake()
    {
        impulseSource.GenerateImpulse();
    }

    public void ScreenShake(float force)
    {
        impulseSource.GenerateImpulseWithForce(force);
    }
}
