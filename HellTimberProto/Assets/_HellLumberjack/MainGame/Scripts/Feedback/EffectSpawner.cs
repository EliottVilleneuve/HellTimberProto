using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    public GameObject effectPrefab;
    public int lifeTime = 0;

    public void Spawn()
    {
        GameObject instance = Instantiate(effectPrefab, transform.position, transform.rotation);
        if (lifeTime > 0) Destroy(instance, lifeTime);
    }
}
