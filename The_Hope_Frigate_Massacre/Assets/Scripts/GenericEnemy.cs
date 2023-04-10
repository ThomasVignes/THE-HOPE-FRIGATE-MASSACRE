using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum State
{
    Idle,
    Roaming,
    Running
}
public class GenericEnemy : MonoBehaviour
{
    [SerializeField] State state;
    [SerializeField] private Animator leftFootAnimator, rightFootAnimator;
    [SerializeField] private List<Animator> hands = new List<Animator>();
    [SerializeField] private List<Animator> heads = new List<Animator>();

    private void Update()
    {
        if (state == State.Idle)
        {
            leftFootAnimator.SetInteger("MoveType", 0);
            rightFootAnimator.SetInteger("MoveType", 0);
        }
        else if (state == State.Roaming)
        {
            leftFootAnimator.SetInteger("MoveType", 1);
            rightFootAnimator.SetInteger("MoveType", 2);
        }

        if (Input.GetKeyDown(KeyCode.F12))
            Scream();
    }

    [ContextMenu("Scream")]
    public void Scream()
    {
        foreach (var item in hands)
        {
            item.SetTrigger("Scream");
        }

        foreach (var item in heads)
        {
            item.SetTrigger("Scream");
        }

        leftFootAnimator.SetTrigger("Scream");
        rightFootAnimator.SetTrigger("Scream");
    }
}
