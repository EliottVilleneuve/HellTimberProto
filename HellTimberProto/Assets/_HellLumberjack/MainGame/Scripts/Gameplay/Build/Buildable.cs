using System;
using UnityEngine;
using UnityEngine.Events;

public class Buildable : MonoBehaviour
{
    public new Collider collider;

    public Renderer[] renderers;
    public Material placementMaterial;

    private Color unbuildablePlacementColor = Color.red;

    private int numRenderer;
    private Material[] previousMaterials;

    private bool canBePlaced = true;

    public UnityEvent OnBuilt;

    private void Start()
    {
        collider.enabled = false;
        unbuildablePlacementColor.a = placementMaterial.color.a;

        numRenderer = renderers.Length;

        previousMaterials = new Material[numRenderer];

        for (int i = 0; i < numRenderer; i++)
        {
            previousMaterials[i] = renderers[i].material;
            renderers[i].material = placementMaterial;

            //We make the thing red if we can't place it
            if (!canBePlaced) renderers[i].material.color = unbuildablePlacementColor;
        }
    }

    public void Place(Transform spawnPoint)
    {
        transform.SetParent(null);
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        for (int i = 0; i < numRenderer; i++)
        {
            renderers[i].material = previousMaterials[i];
        }
        collider.enabled = true;

        OnBuilt?.Invoke();
    }

    public void Setup(bool canPlaceBuild)
    {
        canBePlaced = canPlaceBuild;
    }
}
