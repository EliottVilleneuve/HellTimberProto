using UnityEngine;

public class AvatarAim : MonoBehaviour
{
    public Camera cam;
    public Transform aimingPart;

    public Vector3 Direction { get; private set; }

    private bool controlledByMouse;
    private Vector3 lastMousePos;

    // Start is called before the first frame update
    void Start()
    {
        controlledByMouse = false;
        lastMousePos = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 joystickAxis = new Vector2(Input.GetAxisRaw("Horizontal2"), Input.GetAxisRaw("Vertical2"));

        if (controlledByMouse && joystickAxis.magnitude > 0) controlledByMouse = false;
        else if(!controlledByMouse && Input.mousePosition != lastMousePos) controlledByMouse = true;

        if (controlledByMouse)
        {
            lastMousePos = Input.mousePosition;
            Ray ray = cam.ScreenPointToRay(lastMousePos);

            new Plane(Vector3.up, transform.position).Raycast(ray, out float distanceHit);

            Vector3 target = ray.GetPoint(distanceHit);
            target.y = aimingPart.position.y;
            aimingPart.LookAt(target);
            Direction = aimingPart.forward;
        }
        else
        {
            if(joystickAxis.magnitude > 0.5f)
            {
                Direction = new Vector3(joystickAxis.x, 0, joystickAxis.y).normalized;
                aimingPart.rotation = Quaternion.LookRotation(Direction);
            }
        }

        

        
    }
}
