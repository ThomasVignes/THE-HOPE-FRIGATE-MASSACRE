using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiverAnimationEvents : MonoBehaviour
{
    [SerializeField] private DiverEnemy diver;

    public void Throw()
    {
        diver.HarpoonThrower.Throw(diver.ThrowSpeed, diver.HarpoonThrower.transform.position, diver.PlayerSpot, diver.PlayerLayer);
    }
}
