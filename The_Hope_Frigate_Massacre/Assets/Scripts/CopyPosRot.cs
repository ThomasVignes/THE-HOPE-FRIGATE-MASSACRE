using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosRot : MonoBehaviour
{
    [SerializeField] private Transform original;

    private void Update()
    {
        transform.localPosition = original.localPosition;
        transform.localRotation = original.localRotation;
    }
}
