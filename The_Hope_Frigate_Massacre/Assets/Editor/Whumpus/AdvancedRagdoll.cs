#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AdvancedRagdoll : EditorWindow
{

    public GameObject Parent, Pelvis, LeftHip, LeftKnee, LeftFoot, RightHip, RightKnee, RightFoot, 
        LeftArm, LeftElbow, LeftHand, RightArm, RightElbow, RightHand, Head;

    public List<GameObject> Spine = new List<GameObject>(0);

    public GameObject RefPelvis, RefLeftHip, RefLeftKnee, RefLeftFoot, RefRightHip, RefRightKnee, RefRightFoot,
    RefLeftArm, RefLeftElbow, RefLeftHand, RefRightArm, RefRightElbow, RefRightHand, RefHead;

    public List<GameObject> RefSpine = new List<GameObject>(0);

    public float RagdollSpring, RagdollDamper;

    Vector2 scrollPos;

    [MenuItem("Whumpus/Ragdolls/AdvancedRagdoll", false, 1)]
    public static void ShowWindow()
    {
        GetWindow(typeof(AdvancedRagdoll));

    }

    public void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Ragdoll parts");

        //SetRagdollManager
        EditorGUILayout.BeginHorizontal();
        Parent = (GameObject)EditorGUILayout.ObjectField("Parent", Parent, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        //SetRagdoll
        EditorGUILayout.BeginHorizontal();
        Pelvis = (GameObject)EditorGUILayout.ObjectField("Pelvis", Pelvis, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        LeftHip = (GameObject)EditorGUILayout.ObjectField("Left Hip", LeftHip, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        LeftKnee = (GameObject)EditorGUILayout.ObjectField("Left Knee", LeftKnee, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        LeftFoot = (GameObject)EditorGUILayout.ObjectField("Left Foot", LeftFoot, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RightHip = (GameObject)EditorGUILayout.ObjectField("Right Hip", RightHip, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RightKnee = (GameObject)EditorGUILayout.ObjectField("Right Knee", RightKnee, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RightFoot = (GameObject)EditorGUILayout.ObjectField("Right Foot", RightFoot, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        LeftArm = (GameObject)EditorGUILayout.ObjectField("Left Arm", LeftArm, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        LeftElbow = (GameObject)EditorGUILayout.ObjectField("Left Elbow", LeftElbow, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        LeftHand = (GameObject)EditorGUILayout.ObjectField("Left Hand", LeftHand, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RightArm = (GameObject)EditorGUILayout.ObjectField("Right Arm", RightArm, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RightElbow = (GameObject)EditorGUILayout.ObjectField("Right Elbow", RightElbow, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RightHand = (GameObject)EditorGUILayout.ObjectField("Right Hand", RightHand, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        int newCount = Mathf.Max(0, EditorGUILayout.IntField("Spine length", Spine.Count));
        while (newCount < Spine.Count)
            Spine.RemoveAt(Spine.Count - 1);
        while (newCount > Spine.Count)
            Spine.Add(null);
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < Spine.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            Spine[i] = (GameObject)EditorGUILayout.ObjectField("Spine " + i, Spine[i], typeof(GameObject));
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.BeginHorizontal();
        Head = (GameObject)EditorGUILayout.ObjectField("Head", Head, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Animation reference parts");

        //SetRef
        EditorGUILayout.BeginHorizontal();
        RefPelvis = (GameObject)EditorGUILayout.ObjectField("Ref Pelvis", RefPelvis, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RefLeftHip = (GameObject)EditorGUILayout.ObjectField("Ref Left Hip", RefLeftHip, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RefLeftKnee = (GameObject)EditorGUILayout.ObjectField("Ref Left Knee", RefLeftKnee, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RefLeftFoot = (GameObject)EditorGUILayout.ObjectField("Ref Left Foot", RefLeftFoot, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RefRightHip = (GameObject)EditorGUILayout.ObjectField("Ref Right Hip", RefRightHip, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RefRightKnee = (GameObject)EditorGUILayout.ObjectField("Ref Right Knee", RefRightKnee, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RefRightFoot = (GameObject)EditorGUILayout.ObjectField("Ref Right Foot", RefRightFoot, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RefLeftArm = (GameObject)EditorGUILayout.ObjectField("Ref Left Arm", RefLeftArm, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RefLeftElbow = (GameObject)EditorGUILayout.ObjectField("Ref Left Elbow", RefLeftElbow, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RefLeftHand = (GameObject)EditorGUILayout.ObjectField("Ref Left Hand", RefLeftHand, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RefRightArm = (GameObject)EditorGUILayout.ObjectField("Ref Right Arm", RefRightArm, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RefRightElbow = (GameObject)EditorGUILayout.ObjectField("Ref Right Elbow", RefRightElbow, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RefRightHand = (GameObject)EditorGUILayout.ObjectField("Ref Right Hand", RefRightHand, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        int newCount2 = Mathf.Max(0, EditorGUILayout.IntField("Ref Spine length", RefSpine.Count));
        while (newCount2 < RefSpine.Count)
            RefSpine.RemoveAt(RefSpine.Count - 1);
        while (newCount2 > RefSpine.Count)
            RefSpine.Add(null);
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < RefSpine.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            RefSpine[i] = (GameObject)EditorGUILayout.ObjectField("Ref Spine " + i, RefSpine[i], typeof(GameObject));
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.BeginHorizontal();
        RefHead = (GameObject)EditorGUILayout.ObjectField("Ref Head", RefHead, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Values");

        //SetValues
        EditorGUILayout.BeginHorizontal();
        RagdollSpring = EditorGUILayout.FloatField("Ragdoll Spring", RagdollSpring);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        RagdollDamper = EditorGUILayout.FloatField("Ragdoll Damper", RagdollDamper);
        EditorGUILayout.EndHorizontal();


        if (GUILayout.Button("Create"))
        {
            CreateRagdoll();
        }
    }

    private void CreateRagdoll()
    {
        DiversuitRagdoll manager = Parent.AddComponent<DiversuitRagdoll>();
        Rigidbody parentRb = Parent.AddComponent<Rigidbody>();
        parentRb.isKinematic = true;

        List<GameObject> limbs = new List<GameObject>(){Pelvis, LeftHip, LeftKnee, LeftFoot, RightHip, RightKnee, RightFoot,
        LeftArm, LeftElbow, LeftHand, RightArm, RightElbow, RightHand, Head};

        foreach (var part in Spine)
        {
            if (!limbs.Contains(part))
                limbs.Add(part);
        }

        foreach (var limb in limbs)
        {
            if (limb != null)
            {
                limb.AddComponent<CopyLimb>();
                limb.AddComponent<Rigidbody>();
                ConfigurableJoint joint = limb.AddComponent<ConfigurableJoint>();

                if (limb != Pelvis)
                {
                    joint.xMotion = ConfigurableJointMotion.Locked;
                    joint.yMotion = ConfigurableJointMotion.Locked;
                    joint.zMotion = ConfigurableJointMotion.Locked;

                    JointDrive XDrive = new JointDrive();
                    JointDrive YZDrive = new JointDrive();
                    XDrive.positionSpring = RagdollSpring;
                    XDrive.positionDamper = RagdollDamper;
                    XDrive.maximumForce = joint.xDrive.maximumForce;
                    YZDrive.positionSpring = RagdollSpring;
                    YZDrive.positionDamper = RagdollDamper;
                    YZDrive.maximumForce = joint.angularYZDrive.maximumForce;

                    joint.angularXDrive = XDrive;
                    joint.angularYZDrive = YZDrive;
                }
                else
                {
                    joint.angularXMotion = ConfigurableJointMotion.Locked;
                    joint.angularYMotion = ConfigurableJointMotion.Locked;
                    joint.angularZMotion = ConfigurableJointMotion.Locked;
                    manager.Root = limb;
                    manager.mainRb = limb.GetComponent<Rigidbody>();
                }
            }
        }

        GameObject topSpine = Spine[Spine.Count - 1];

        //Pelvis
        Pelvis.GetComponent<ConfigurableJoint>().connectedBody = parentRb;

        //Head
        Setup(Head, topSpine, RefHead);

        //LeftArm
        Setup(LeftHand, LeftElbow, RefLeftHand);
        Setup(LeftElbow, LeftArm, RefLeftArm);
        Setup(LeftArm, topSpine, RefLeftArm);

        //RightArm
        Setup(RightHand, RightElbow, RefRightHand);
        Setup(RightElbow, RightArm, RefRightElbow);
        Setup(RightArm, topSpine, RefRightArm);

        //Spine
        for (int i = Spine.Count - 1; i >= 0; i--)
        {
            if (i == 0)
            {
                Setup(Spine[i], Pelvis, RefSpine[i]);
            }
            else
            {
                Setup(Spine[i], Spine[i - 1], RefSpine[i]);
            }
        }

        //LeftLeg
        Setup(LeftFoot, LeftKnee, RefLeftFoot);
        Setup(LeftKnee, LeftHip, RefLeftKnee);
        Setup(LeftHip, Pelvis, RefLeftHip);

        //RightLeg
        Setup(RightFoot, RightKnee, RefRightFoot);
        Setup(RightKnee, RightHip, RefRightKnee);
        Setup(RightHip, Pelvis, RefRightHip);
    }

    public void Setup(GameObject limb, GameObject connected, GameObject refLimb)
    {
        if (connected != null)
            limb.GetComponent<ConfigurableJoint>().connectedBody = connected.GetComponent<Rigidbody>();

        if (refLimb != null)
            limb.GetComponent<CopyLimb>().TargetLimb = refLimb.transform;
    }
}

#endif