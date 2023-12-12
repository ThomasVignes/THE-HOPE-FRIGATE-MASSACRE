using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosRot : MonoBehaviour
{
    [SerializeField] private UpdateMode updateMode;
    [SerializeField] private CopyMode copyMode;
    [SerializeField] private bool copyPos = true, copyRot = true;
    [SerializeField] private Transform original;

    private void Update()
    {
        if (updateMode != UpdateMode.Update)
            return;

        Copy();
    }

    private void FixedUpdate()
    {
        if (updateMode != UpdateMode.FixedUpdate) 
            return;

        Copy();
    }

    private void LateUpdate()
    {
        if (updateMode != UpdateMode.LateUpdate)
            return;

        Copy();
    }

    private void Copy()
    {
        switch (copyMode)
        {
            case CopyMode.Local:
                if (copyPos)
                    transform.localPosition = original.localPosition;

                if (copyRot)
                    transform.localRotation = original.localRotation;
                break;

            case CopyMode.Global:
                if (copyPos)
                    transform.position = original.position;

                if (copyRot)
                    transform.rotation = original.rotation;
                break;
        }
    }


    private enum UpdateMode
    {
        Update,
        FixedUpdate,
        LateUpdate
    }

    private enum CopyMode
    {
        Local,
        Global
    }
}
