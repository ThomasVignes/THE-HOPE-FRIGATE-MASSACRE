using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    public void ThrowRArm()
    {
        player.DetachArm();
    }
}
