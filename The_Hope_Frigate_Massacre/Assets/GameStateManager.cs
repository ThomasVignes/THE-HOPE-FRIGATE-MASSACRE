using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen;

    public bool Lost;

    public void Lose()
    {
        if (!Lost)
        {
            Lost = true;
            StartCoroutine(DeathScreen());
        }
    }

    IEnumerator DeathScreen()
    {
        yield return new WaitForSeconds(3.3f);

        deathScreen.SetActive(true);

        yield return new WaitForSeconds(4.7f);
    }
}
