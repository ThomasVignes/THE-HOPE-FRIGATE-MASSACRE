#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ParentJoint
{
    public GameObject Go;
    public bool HasChildren;
    public List<ParentJoint> Children = new List<ParentJoint>();
}

public class JointHierarchy : EditorWindow
{
    public List<ParentJoint> Joints = new List<ParentJoint>();
    public float RagdollSpring, RagdollDamper;
    Vector2 scrollPos;
    
    [MenuItem("Whumpus/Ragdolls/JointHierarchy", false, 1)]
    public static void ShowWindow()
    {
        GetWindow(typeof(JointHierarchy));

    }

    public void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();

        ShowList(Joints, 0);

        //SetValues
        EditorGUILayout.BeginHorizontal();
        RagdollSpring = EditorGUILayout.FloatField("Ragdoll Spring", RagdollSpring);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RagdollDamper = EditorGUILayout.FloatField("Ragdoll Damper", RagdollDamper);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Attatch"))
        {
            Attach(Joints);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }

    private void Attach(List<ParentJoint> Joints)
    {
        foreach (var joint in Joints)
        {
            joint.Go.AddComponent<Rigidbody>();
            ConfigurableJoint jointSettings = joint.Go.AddComponent<ConfigurableJoint>();

            jointSettings.xMotion = ConfigurableJointMotion.Locked;
            jointSettings.yMotion = ConfigurableJointMotion.Locked;
            jointSettings.zMotion = ConfigurableJointMotion.Locked;

            JointDrive XDrive = new JointDrive();
            JointDrive YZDrive = new JointDrive();
            XDrive.positionSpring = RagdollSpring;
            XDrive.positionDamper = RagdollDamper;
            XDrive.maximumForce = jointSettings.xDrive.maximumForce;
            YZDrive.positionSpring = RagdollSpring;
            YZDrive.positionDamper = RagdollDamper;
            YZDrive.maximumForce = jointSettings.angularYZDrive.maximumForce;

            jointSettings.angularXDrive = XDrive;
            jointSettings.angularYZDrive = YZDrive;


            Attach(joint.Children, joint.Go.GetComponent<Rigidbody>());
        }
    }

    private void Attach(List<ParentJoint> Joints, Rigidbody rb)
    {
        foreach (var joint in Joints)
        {
            joint.Go.AddComponent<Rigidbody>();
            ConfigurableJoint jointSettings = joint.Go.AddComponent<ConfigurableJoint>();

            jointSettings.connectedBody = rb;
            jointSettings.xMotion = ConfigurableJointMotion.Locked;
            jointSettings.yMotion = ConfigurableJointMotion.Locked;
            jointSettings.zMotion = ConfigurableJointMotion.Locked;

            JointDrive XDrive = new JointDrive();
            JointDrive YZDrive = new JointDrive();
            XDrive.positionSpring = RagdollSpring;
            XDrive.positionDamper = RagdollDamper;
            XDrive.maximumForce = jointSettings.xDrive.maximumForce;
            YZDrive.positionSpring = RagdollSpring;
            YZDrive.positionDamper = RagdollDamper;
            YZDrive.maximumForce = jointSettings.angularYZDrive.maximumForce;

            jointSettings.angularXDrive = XDrive;
            jointSettings.angularYZDrive = YZDrive;


            Attach(joint.Children, joint.Go.GetComponent<Rigidbody>());
        }
    }

    public void ShowList(List<ParentJoint> jointList, int serialization)
    {
        EditorGUILayout.BeginHorizontal();

        for (int j = 0; j < serialization; j++)
        {
            EditorGUILayout.Space();
        }

        int newCount = Mathf.Max(0, EditorGUILayout.IntField("Hierarchy " + serialization, jointList.Count));

        while (newCount < jointList.Count)
            jointList.RemoveAt(jointList.Count - 1);
        while (newCount > jointList.Count)
            jointList.Add(new ParentJoint());

        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < jointList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < serialization; j++)
            {
                EditorGUILayout.Space();
            }
            EditorGUILayout.LabelField("Joint " + i);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < serialization; j++)
            {
                EditorGUILayout.Space();
            }
            jointList[i].Go = (GameObject)EditorGUILayout.ObjectField(jointList[i].Go, typeof(GameObject));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < serialization; j++)
            {
                EditorGUILayout.Space();
            }
            jointList[i].HasChildren = EditorGUILayout.Toggle("Has Children", jointList[i].HasChildren);
            EditorGUILayout.EndHorizontal();

            if (jointList[i].HasChildren)
                ShowList(jointList[i].Children, serialization + 1);
        }
    }

    public void Setup(GameObject limb, GameObject connected, GameObject refLimb)
    {
        if (connected != null)
            limb.GetComponent<ConfigurableJoint>().connectedBody = connected.GetComponent<Rigidbody>();

        if (refLimb != null)
            limb.GetComponent<RagdollLimb>().TargetLimb = refLimb.transform;
    }
}

#endif