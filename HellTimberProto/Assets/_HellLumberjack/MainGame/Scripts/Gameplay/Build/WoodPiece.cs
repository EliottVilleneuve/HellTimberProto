using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodPiece : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up * 90 * Time.deltaTime);
    }
}
