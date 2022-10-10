using UnityEngine;

namespace HellLumber
{
    public class AvatarMove : MonoBehaviour
    {
        public CharacterController characterController;

        public float speed = 5;

        private float speedMultiplier;

        void Start()
        {
            speedMultiplier = 1;
        }

        void Update()
        {
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            Vector3 movement = Vector3.right * input.x + Vector3.forward * input.y;
            if (movement.magnitude > 1) movement.Normalize();

            characterController.Move(movement * speed * speedMultiplier * Time.deltaTime);
        }

        public void SetMultiplier(float newValue)
        {
            speedMultiplier = newValue;
        }
    }
}

