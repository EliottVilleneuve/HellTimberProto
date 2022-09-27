using UnityEngine;

public class AvatarAim : MonoBehaviour
{
    public Camera cam;
    public Transform aimingPart;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        new Plane(Vector3.up, transform.position).Raycast(ray, out float distanceHit);

        Vector3 target = ray.GetPoint(distanceHit);
        target.y = aimingPart.position.y;
        aimingPart.LookAt(target);
    }
}
