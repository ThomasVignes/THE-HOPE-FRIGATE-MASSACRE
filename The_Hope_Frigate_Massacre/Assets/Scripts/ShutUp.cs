using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutUp : MonoBehaviour
{
    [SerializeField] private GameObject toShut;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            toShut.SetActive(!toShut.activeSelf);
        }
    }
}
