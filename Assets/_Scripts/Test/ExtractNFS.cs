using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ExtractNFS : MonoBehaviour
{
    public string pathMaterials;
    public string pathTextures;

    private MeshRenderer mesh;
    private List<Material> mats;
    private List<Material> newMats;

    private string matName;
    private Texture tex;
    private Material mat;

    [ContextMenu("Translate")]
    public void Translate()
    {
        mesh = GetComponentInChildren<MeshRenderer>();

        mats = new List<Material>();
        mesh.GetSharedMaterials(mats);

        for (int i = 0; i < mats.Count; i++)
        {
            matName = mats[i].name;
            tex = mats[i].GetTexture("baseColorTexture");
            mat = new Material(Shader.Find("Universal Render Pipeline/Lit")) { name = matName };
            mat.SetTexture("_BaseMap", tex);

            AssetDatabase.CreateAsset(mat, $"{pathMaterials}/{matName}.mat");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        newMats = new List<Material>();
        for (int i = 0; i < mats.Count; i++)
        {
            newMats.Add((Material)AssetDatabase.LoadAssetAtPath($"{pathMaterials}/{mats[i].name}.mat", typeof(Material)));
        }

        mesh.SetSharedMaterials(newMats);

        Debug.Log("Finish");
    }

    [ContextMenu("Translate Textures")]
    public void TranslateTextures()
    {
        for (int i = 0; i < newMats.Count; i++)
        {
            string matName = newMats[i].name.Split('.')[0]; // sin .mat normalmente

            // Buscar texturas que coincidan
            string[] guids = AssetDatabase.FindAssets(matName + " t:Texture2D");

            Texture2D bestMatch = null;

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string fileName = Path.GetFileNameWithoutExtension(path).Split('.')[0];

                // Match principal: empieza igual
                if (fileName.StartsWith(matName))
                {
                    bestMatch = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                    break;
                }
            }

            if (bestMatch != null)
            {
                newMats[i].SetTexture("_BaseMap", bestMatch);
                newMats[i].SetTextureScale("_BaseMap", new Vector2(1, -1));

                if (bestMatch.format == TextureFormat.DXT5)
                {
                    newMats[i].SetFloat("_AlphaClip", 1);
                    newMats[i].EnableKeyword("_ALPHATEST_ON");
                }

                EditorUtility.SetDirty(mat);
            }
            else
            {
                Debug.LogWarning($"No texture found for {matName.Split('.')[0]}", newMats[i]);
            }
        }

        Debug.Log("Finish");
    }
}