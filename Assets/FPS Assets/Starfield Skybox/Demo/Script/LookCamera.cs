using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

namespace MyGame
{
    public class LookCamera : MonoBehaviour
    {
        [Header("Input Settings")]
        private PlayerInputActions playerInput;
        private InputAction look;
        private InputAction uButton;

        public float speedNormal = 10.0f;
        public float speedFast = 50.0f;

        public float mouseSensitivityX = 5.0f;
        public float mouseSensitivityY = 5.0f;

        float rotY = 0.0f;
        private void Awake()
        {
            playerInput = new PlayerInputActions();

        }
        private void OnEnable()
        {
            Debug.Log("bla");
            look = playerInput.Player.Look;
            look.Enable();
            look.performed += LookAt;
            
            uButton = playerInput.Player.Fire;
            uButton.Enable();
        }
        private void OnDisable()
        {
            look.Enable();
            uButton.Enable();
        }
        private void LookAt(InputAction.CallbackContext ctx)
        {
            Debug.Log("moving camera x "+ ctx.ReadValue<Vector2>().x + "moving camera y "+ ctx.ReadValue<Vector2>().y);
        }
        void Start()
        {
            if (GetComponent<Rigidbody>())
                GetComponent<Rigidbody>().freezeRotation = true;
        }

        void Update()
        {
           if (Input.GetMouseButton(1))
            {
                float rotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivityX;
                rotY += Input.GetAxis("Mouse Y") * mouseSensitivityY;
                rotY = Mathf.Clamp(rotY, -89.5f, 89.5f);
                transform.localEulerAngles = new Vector3(-rotY, rotX, 0.0f);
            }

            if (Input.GetKey(KeyCode.U))
            {
                gameObject.transform.localPosition = new Vector3(0.0f, 3500.0f, 0.0f);
            }
           

        }
    }
}
