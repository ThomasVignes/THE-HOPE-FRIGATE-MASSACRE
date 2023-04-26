using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyAnimationEvents : MonoBehaviour
{
    [SerializeField] BaseEnemy enemy;
    public void ScreenShake()
    {
        CameraEffectsManager.Instance.ScreenShake();
    }

    public void DashEnd()
    {
        enemy.EndDash();
    }
}
