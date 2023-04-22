using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyAnimationEvents : MonoBehaviour
{
    public void ScreenShake()
    {
        CameraEffectsManager.Instance.ScreenShake();
    }
}
