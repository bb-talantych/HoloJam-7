using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BB_AddOutline : MonoBehaviour
{
    [Range(0f, 1f), SerializeField]
    private float OutlineSize = 0.015f;
    [SerializeField]
    private Color OutlineColor = Color.black;

    [HideInInspector]
    public Shader outlineShader_3d;
    [HideInInspector]
    public Shader outlineShader_2d;
    private Material outlineMaterial;


    private MeshRenderer mshRenderer;
    private SpriteRenderer sprRenderer;

    private void OnEnable()
    {
        // intialize one of the renderers
        if (!HasRenderer()) 
        {
            TryGetComponent<MeshRenderer>(out mshRenderer);
            TryGetComponent<SpriteRenderer>(out sprRenderer);
        }
        // create outline material instance
        if(HasRenderer() && HasShaders())
        {
            outlineMaterial = mshRenderer != null ? 
                new Material(outlineShader_3d) : new Material(outlineShader_2d);
        }
        // add/assign this material, depending on renderer
        if(HasRenderer() && outlineMaterial != null)
        {
            if(mshRenderer != null)
            {
                var meshMaterials = mshRenderer.materials;
                var objMats = new List<Material>(meshMaterials);
                objMats.Add(outlineMaterial);
                mshRenderer.materials = objMats.ToArray();
            }
            else
            {
                sprRenderer.material = outlineMaterial;
            }
        }
    }

    private void OnMouseEnter()
    {
        if (outlineMaterial != null)
        {
            outlineMaterial.SetFloat("_Thickness", OutlineSize);
            outlineMaterial.SetColor("_OutlineColor", OutlineColor);
            outlineMaterial.EnableKeyword("OUTLINE_ON");
        }
    }

    private void OnMouseExit()
    {
        if (outlineMaterial != null)
        {
            outlineMaterial.DisableKeyword("OUTLINE_ON");
        }
    }

    #region Local bools and conditions
    bool HasRenderer()
    {
        if (mshRenderer == null && sprRenderer == null)
            return false;

        return true;
    }
    bool HasShaders()
    {
        if (outlineShader_3d == null && outlineShader_2d == null)
            return false;

        return true;
    }

    #endregion

}

