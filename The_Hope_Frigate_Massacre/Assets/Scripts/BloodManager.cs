using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodManager : MonoBehaviour
{
    public static BloodManager Instance;
    [SerializeField] private GameObject bloodPrefab;

    [SerializeField] private float UIBloodStay;
    [SerializeField] private Image UIBlood;

    private float UIBloodTimer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        if (UIBloodTimer > 0)
            UIBloodTimer -= Time.deltaTime;
        else
        {
            if (UIBloodTimer != 0)
                UIBloodTimer = 0;

            if (UIBlood.color.a > 0)
            {
                Color old = UIBlood.color;
                Color newCol = new Color(old.r, old.g, old.b, Mathf.Lerp(old.a, 0, Time.deltaTime));

                UIBlood.color = newCol;
            }
        }
    }

    public void ActivateUIBlood()
    {
        Color old = UIBlood.color;
        Color newCol = new Color(old.r, old.g, old.b, 1);

        UIBlood.color = newCol;

        UIBloodTimer = UIBloodStay;
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
