using UnityEngine;

public class PacmanTeleport : MonoBehaviour {

    public Vector2 halfArenaSize = Vector2.one;

    private void Start () {
        
    }

    private void LateUpdate () {

        Vector3 pos = transform.position;

        if(pos.x > halfArenaSize.x) pos.x = -halfArenaSize.x;
        else if (pos.x < -halfArenaSize.x) pos.x = halfArenaSize.x;

        if (pos.z > halfArenaSize.y) pos.z = -halfArenaSize.y;
        else if (pos.z < -halfArenaSize.y) pos.z = halfArenaSize.y;

        transform.position = pos;
    }
}
