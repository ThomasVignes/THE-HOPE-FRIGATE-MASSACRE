using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodManager : MonoBehaviour
{
    public static BloodManager Instance;
    [SerializeField] private GameObject bloodPrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void SpawnBlood(Vector3 pos)
    {
        GameObject blood = Instantiate(bloodPrefab);

        blood.transform.position = pos;
    }

    public void SpawnBlood(Vector3 pos, Vector3 lookAt)
    {
        GameObject blood = Instantiate(bloodPrefab);

        blood.transform.position = pos;
        blood.transform.LookAt(lookAt);
    }

    public void SpawnBlood(Vector3 pos, Vector3 lookAt, Transform parent)
    {
        GameObject blood = Instantiate(bloodPrefab);

        blood.transform.parent = parent;
        blood.transform.position = pos;
        blood.transform.LookAt(lookAt);
    }

    public void SpawnBlood(Vector3 pos, Vector3 lookAt, Transform parent, Vector3 scale)
    {
        GameObject blood = Instantiate(bloodPrefab);

        blood.transform.parent = parent;
        blood.transform.position = pos;
        blood.transform.LookAt(lookAt);
        blood.transform.localScale = scale;
    }
}
