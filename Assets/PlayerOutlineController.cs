using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOutlineController : MonoBehaviour
{
    private Material outlineMaterial;
    private SpriteRenderer spriteRenderer;

    [Header("Outline Settings")]
    [SerializeField] private Color normalOutlineColor = Color.red;
    [SerializeField] private Color deathOutlineColor = Color.black;
    [SerializeField] private float outlineSize = 1.0f;
    [SerializeField] private float glowIntensity = 1.5f;

    [Header("Fade Settings")]
    [SerializeField] private float fadeInDuration = 1f;
    private float currentFadeTime = 0f;

    private bool lastFlipX = false;  // Track last flipX value

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        outlineMaterial = new Material(Shader.Find("Custom/OutlineGlow"));
        if (outlineMaterial == null)
        {
            Debug.LogError("Shader 'Custom/OutlineGlow' tidak ditemukan!");
            return;
        }

        spriteRenderer.material = outlineMaterial;

        outlineMaterial.SetFloat("_OutlineSize", 0);
        outlineMaterial.SetColor("_OutlineColor", normalOutlineColor);
        outlineMaterial.SetFloat("_GlowIntensity", glowIntensity);

        StartCoroutine(FadeInOutline());
    }

    void LateUpdate()
    {
        if (spriteRenderer != null && outlineMaterial != null)
        {
            bool currentFlipX = spriteRenderer.flipX;

            // Update material properties
            outlineMaterial.SetFloat("_FlipX", spriteRenderer.flipX ? 1 : 0);
            outlineMaterial.SetFloat("_FlipY", spriteRenderer.flipY ? 1 : 0);

            // Add scale compensation if object is rotated
            Vector3 scale = transform.lossyScale;
            outlineMaterial.SetFloat("_ScaleX", Mathf.Sign(scale.x));
            outlineMaterial.SetFloat("_ScaleY", Mathf.Sign(scale.y));

            // Always use positive outline size
            outlineMaterial.SetFloat("_OutlineSize", outlineSize);
        }
    }


    private IEnumerator FadeInOutline()
    {
        currentFadeTime = 0f;

        while (currentFadeTime < fadeInDuration)
        {
            currentFadeTime += Time.deltaTime;
            float progress = currentFadeTime / fadeInDuration;
            float currentSize = Mathf.Lerp(0, outlineSize, progress);
            outlineMaterial.SetFloat("_OutlineSize", currentSize);
            yield return null;
        }

        outlineMaterial.SetFloat("_OutlineSize", outlineSize);
    }

    public void SetDeathOutline()
    {
        outlineMaterial.SetColor("_OutlineColor", deathOutlineColor);
    }

    public void ResetOutline()
    {
        outlineMaterial.SetColor("_OutlineColor", normalOutlineColor);
    }
}
