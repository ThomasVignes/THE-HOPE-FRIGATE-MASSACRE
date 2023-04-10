using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject instaCutMessage;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            player.instaCut = !player.instaCut;

            instaCutMessage.SetActive(player.instaCut);
        }
    }
}
