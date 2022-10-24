using HellLumber;
using UnityEngine;

public class AvatarAim : MonoBehaviour
{
    public Camera cam;
    public Transform aimingPart;
    public AvatarMove avatarMove;
    public bool aimForward;

    public Vector3 Direction { get; private set; }

    private bool controlledByMouse;
    private Vector3 lastMousePos;

    // Start is called before the first frame update
    void Start()
    {
        controlledByMouse = false;
        lastMousePos = Controller.MousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 joystickAxis = Controller.GetVector("Aim");//new Vector2(Controller.GetAxis("Horizontal2"), Controller.GetAxis("Vertical2"));

        if (controlledByMouse && joystickAxis.magnitude > 0) controlledByMouse = false;
        else if(!controlledByMouse && Controller.MousePosition != lastMousePos) controlledByMouse = true;

        if (controlledByMouse)
        {
            lastMousePos = Controller.MousePosition;
            Ray ray = cam.ScreenPointToRay(lastMousePos);

            new Plane(Vector3.up, transform.position).Raycast(ray, out float distanceHit);

            Vector3 target = ray.GetPoint(distanceHit);
            target.y = aimingPart.position.y;
            aimingPart.LookAt(target);
            Direction = aimingPart.forward;
        }
        else
        {
            if (joystickAxis.magnitude > 0.5f)
            {
                Direction = new Vector3(joystickAxis.x, 0, joystickAxis.y).normalized;
            }
            else if (joystickAxis.magnitude < 0.1f && aimForward && avatarMove.Direction.magnitude > 0.1f)
            {
                Direction = avatarMove.Direction;
            }
            else return;
            aimingPart.rotation = Quaternion.LookRotation(Direction);
        }

        

        
    }
}
