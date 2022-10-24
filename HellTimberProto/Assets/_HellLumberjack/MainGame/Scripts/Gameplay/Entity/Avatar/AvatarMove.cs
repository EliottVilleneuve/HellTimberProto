using UnityEngine;

namespace HellLumber
{
    public class AvatarMove : MonoBehaviour
    {
        public CharacterController characterController;

        public float speed = 5;

        private float speedMultiplier;

        public Vector3 Direction { get; internal set; }

        void Start()
        {
            speedMultiplier = 1;
        }

        void Update()
        {
            Vector2 input = Controller.GetVector("Move");//new Vector2(Controller.GetAxis("Horizontal"), Controller.GetAxis("Vertical"));

            Direction = Vector3.right * input.x + Vector3.forward * input.y;
            if (Direction.magnitude > 1) Direction.Normalize();

            characterController.Move(Direction * speed * speedMultiplier * Time.deltaTime);
        }

        public void SetMultiplier(float newValue)
        {
            speedMultiplier = newValue;
        }
    }
}

