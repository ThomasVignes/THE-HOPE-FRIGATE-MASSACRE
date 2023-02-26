using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelvisFollower : MonoBehaviour
{
    [SerializeField] private GameObject pelvis;

    private void Update()
    {
        transform.position = pelvis.transform.position;
    }
}
