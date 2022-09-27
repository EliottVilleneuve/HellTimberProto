using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashColor : MonoBehaviour
{
    public Renderer[] flashingRenderer;
    public Material flashMaterial;

    public Color defaultFlashColor = Color.white;
    public float defaultFlashDuration = 0.2f;
    public int defaultFlashNumber = 1;

    private int numberMaterials;
    private Material customFlashMaterial;
    private Material[] defaultMaterials;

    private IEnumerator currentCoroutine;

    void Start()
    {
        customFlashMaterial = new Material(flashMaterial);

        numberMaterials = flashingRenderer.Length;
        SetDefaultMaterial();
    }

    public void SetDefaultMaterial()
    {
        defaultMaterials = new Material[numberMaterials];
        for (int i = 0; i < numberMaterials; i++)
        {
            defaultMaterials[i] = flashingRenderer[i].material;
        }
    }

    public void DefaultFlash()
    {
        Flash(defaultFlashColor, defaultFlashDuration, defaultFlashNumber);
    }

    public void CancelFlash()
    {
        if(currentCoroutine != null) StopCoroutine(currentCoroutine);
    }

    public void Flash(Color color, float flashDuration, int flashNumber = 1)
    {
        currentCoroutine = FlashCoroutine(color, flashDuration, flashNumber);
        StartCoroutine(currentCoroutine);
    }

    private IEnumerator FlashCoroutine(Color color, float flashDuration, int flashNumber = 1)
    {
        for (int i = 0; i < flashNumber; i++)
        {
            SwitchMaterial(color);
            yield return new WaitForSeconds(flashDuration);
            SwitchMaterial();

            if (i < flashNumber - 1) yield return new WaitForSeconds(flashDuration);
        }
        currentCoroutine = null;
    }

    private void SwitchMaterial(Color color) => SwitchMaterial(false, color);
    private void SwitchMaterial(bool defaultMaterial = true, Color color = default)
    {
        customFlashMaterial.color = color;
        customFlashMaterial.SetColor("_EmissionColor", color);
        for (int i = 0; i < numberMaterials; i++)
        {
            flashingRenderer[i].material = defaultMaterial? defaultMaterials[i] : customFlashMaterial;
        }
    }
}
