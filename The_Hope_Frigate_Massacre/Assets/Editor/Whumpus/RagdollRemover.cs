#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RagdollRemover : EditorWindow
{
    public GameObject Parent;

    [MenuItem("Whumpus/Ragdolls/RagdollRemover")]
    public static void ShowWindow()
    {
        GetWindow(typeof(RagdollRemover));
    }

    public void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Insert Parent of ragdoll hierarchy");
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("THIS ACTION CANNOT BE REVERTED");
        EditorGUILayout.Space();

        //SetRagdollManager
        EditorGUILayout.BeginHorizontal();
        Parent = (GameObject)EditorGUILayout.ObjectField("Parent", Parent, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Remove Ragdoll"))
        {
            RemoveRagdoll();
        }
    }

    private void RemoveRagdoll()
    {
        int limbsint, jointsint, forcesint, rigidbodiesint;
        CopyLimb[] copyLimbs = Parent.transform.GetComponentsInChildren<CopyLimb>();
        ConfigurableJoint[] joints = Parent.transform.GetComponentsInChildren<ConfigurableJoint>();
        ConstantForce[] constantForces = Parent.transform.GetComponentsInChildren<ConstantForce>();
        Rigidbody[] rigidbodies = Parent.transform.GetComponentsInChildren<Rigidbody>();

        limbsint = copyLimbs.Length + 1;
        jointsint = joints.Length + 1;
        forcesint = constantForces.Length + 1;
        rigidbodiesint = rigidbodies.Length + 1;

        if (Parent.GetComponent<Rigidbody>())
            DestroyImmediate(Parent.GetComponent<Rigidbody>());

        if (Parent.GetComponent<DiversuitRagdoll>())
            DestroyImmediate(Parent.GetComponent<DiversuitRagdoll>());

        foreach (var item in copyLimbs)
        {
            DestroyImmediate(item);
        }

        foreach (var item in joints)
        {
            DestroyImmediate(item);
        }

        foreach (var item in constantForces)
        {
            DestroyImmediate(item);
        }

        foreach (var item in rigidbodies)
        {
            DestroyImmediate(item);
        }

        Debug.Log("Destroyed " + limbsint + " limbs, " + jointsint + " joints, " + forcesint + " constant forces, and " + rigidbodiesint + " rigidbodies");
    }
}

#endif