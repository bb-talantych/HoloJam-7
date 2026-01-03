using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BB_Temp : MonoBehaviour
{
    [Range(0f, 1f), SerializeField]
    private float OutlineSize = 0.15f;

    [HideInInspector]
    public Shader outlineShader;
    [HideInInspector]
    public Shader outlineShader2d;
    private Material outlineMaterial;

    private MeshRenderer mshRenderer;
    private SpriteRenderer sprRenderer;

    List<Material> objMats;

    private bool isMeshRenderer;

    private void OnEnable()
    {
        if (HasRenderer())
        {
            if (mshRenderer != null && outlineShader != null)
            {
                outlineMaterial = new Material(outlineShader);
            }
            else if (outlineShader2d != null)
                outlineMaterial = new Material(outlineShader2d);
        }

        if (OutlineCondition())
        {
            if (mshRenderer != null)
            {
                var mats = mshRenderer.materials;
                objMats = new List<Material>(mats);

                if (!HasMaterial(mshRenderer, outlineMaterial))
                {
                    objMats.Add(outlineMaterial);
                    mshRenderer.materials = objMats.ToArray();
                }
            }
            else if (!HasMaterial(sprRenderer, outlineMaterial))
            {
                sprRenderer.material = outlineMaterial;
            }
        }
    }
    private void OnMouseEnter()
    {
        if (OutlineCondition())
        {
            outlineMaterial.SetFloat("_Thickness", OutlineSize);
            outlineMaterial.EnableKeyword("OUTLINE_ON");
        }
    }

    private void OnMouseExit()
    {
        if (OutlineCondition())
        {
            outlineMaterial.DisableKeyword("OUTLINE_ON");
        }
    }

    bool OutlineCondition()
    {
        if (outlineMaterial == null)
            return false;
        if (!HasRenderer())
            return false;

        return true;
    }

    bool HasRenderer()
    {
        if (mshRenderer == null && sprRenderer == null)
        {
            TryGetComponent<MeshRenderer>(out mshRenderer);
            TryGetComponent<SpriteRenderer>(out sprRenderer);
        }
        if (mshRenderer == null && sprRenderer == null)
            return false;

        return true;
    }

    bool HasMaterial(Renderer r, Material mat)
    {
        foreach (var m in r.materials)
        {
            if (m == mat)
                return true;
        }
        return false;
    }
}
