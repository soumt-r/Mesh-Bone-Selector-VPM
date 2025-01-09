using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class RC_MeshBoneSelector : EditorWindow
{
    private GameObject selectedMeshObject;

    [MenuItem("ReraC/Mesh Bone Selector")]
    public static void ShowWindow()
    {
        var window = GetWindow<RC_MeshBoneSelector>();
        window.minSize = new Vector2(300, 220);
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 25;
        GUILayout.Label("Mesh Bone Selector.");
        GUI.skin.label.fontSize = 10;
        GUI.skin.label.alignment = TextAnchor.MiddleRight;
        GUILayout.Label("V1 by Rera*C");
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;

        EditorGUILayout.Space(10);

        GUILayout.Label("Select Influential Bones", EditorStyles.boldLabel);

        // ���õ� �޽� ������Ʈ
        selectedMeshObject = EditorGUILayout.ObjectField(
            "Mesh Object",
            selectedMeshObject,
            typeof(GameObject),
            true
        ) as GameObject;

        GUILayout.Space(10); 

        if (GUILayout.Button("Select Influential Bones", GUILayout.Height(100)))
        {
            SelectInfluentialBones();
        }
    }

    private void SelectInfluentialBones()
    {
        if (selectedMeshObject == null)
        {
            Debug.LogWarning("Please select a mesh object!");
            return;
        }

        var skinnedMeshRenderer = selectedMeshObject.GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer == null)
        {
            Debug.LogWarning("Selected object does not have a SkinnedMeshRenderer component!");
            return;
        }

        var mesh = skinnedMeshRenderer.sharedMesh;
        if (mesh == null)
        {
            Debug.LogWarning("No mesh found in the SkinnedMeshRenderer!");
            return;
        }

        var boneWeights = mesh.boneWeights;
        var bones = skinnedMeshRenderer.bones;

        HashSet<Transform> influentialBones = new HashSet<Transform>();

        // ��� ���ؽ��� �� ����ġ�� �м�
        for (int i = 0; i < boneWeights.Length; i++)
        {
            var boneWeight = boneWeights[i];

            if (boneWeight.weight0 > 0) influentialBones.Add(bones[boneWeight.boneIndex0]);
            if (boneWeight.weight1 > 0) influentialBones.Add(bones[boneWeight.boneIndex1]);
            if (boneWeight.weight2 > 0) influentialBones.Add(bones[boneWeight.boneIndex2]);
            if (boneWeight.weight3 > 0) influentialBones.Add(bones[boneWeight.boneIndex3]);
        }

        // ������ ��ġ�� �� ����
        List<GameObject> Selects = new List<GameObject>();
        foreach (var bone in influentialBones)
        {
            if (bone != null)
            {
                Selects.Add(bone.gameObject);
                    
                Debug.Log($"Selected influential bone: {bone.name}");
            }
        }

        Selection.objects = Selects.ToArray();

        Debug.Log("Influential bone selection complete.");
    }
}
