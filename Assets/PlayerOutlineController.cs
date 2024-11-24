using UnityEngine;
using System.Collections;

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

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ganti referensi shader
        outlineMaterial = new Material(Shader.Find("Custom/OutlineGlow"));
        if (outlineMaterial == null)
        {
            Debug.LogError("Shader 'Custom/OutlineGlow' tidak ditemukan!");
            return;
        }

        spriteRenderer.material = outlineMaterial; // Langsung gunakan outlineMaterial

        // Set properti awal
        outlineMaterial.SetFloat("_OutlineSize", 0);
        outlineMaterial.SetColor("_OutlineColor", normalOutlineColor);
        outlineMaterial.SetFloat("_GlowIntensity", glowIntensity);

        StartCoroutine(FadeInOutline());
    }


    void LateUpdate()
    {
        // Update material properties berdasarkan sprite orientation
        if (spriteRenderer != null)
        {
            outlineMaterial.SetFloat("_FlipX", spriteRenderer.flipX ? 1 : 0);
            outlineMaterial.SetFloat("_FlipY", spriteRenderer.flipY ? 1 : 0);
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
