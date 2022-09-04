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
        private Mouse mouse;
        private Keyboard keyboard;

        private InputAction uButton;

        [Header("Speed Settings")]
        public float speedNormal = 10.0f;
        public float speedFast = 50.0f;

        [Header("Sensitivity Settings")]
        public float mouseSensitivityX = 5.0f;
        public float mouseSensitivityY = 5.0f;

        float rotY = 0.0f;
        private void Awake()
        {
            playerInput = new PlayerInputActions();
            mouse = InputSystem.GetDevice<Mouse>();
            keyboard = InputSystem.GetDevice<Keyboard>();

        }
        private void OnEnable()
        {
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
        //    Debug.Log("moving camera x "+ ctx.ReadValue<Vector2>().x + "moving camera y "+ ctx.ReadValue<Vector2>().y);
        }
        void Start()
        {
            if (GetComponent<Rigidbody>())
                GetComponent<Rigidbody>().freezeRotation = true;
        }

        void Update()
        {
           if (mouse.leftButton.wasPressedThisFrame)
            {
                float rotX = transform.localEulerAngles.y + mouse.position.ReadValue().x * mouseSensitivityX;
                rotY += mouse.position.ReadValue().y * mouseSensitivityY;
                rotY = Mathf.Clamp(rotY, -89.5f, 89.5f);
                transform.localEulerAngles = new Vector3(-rotY, rotX, 0.0f);
            }

            if (keyboard.uKey.wasPressedThisFrame)
            {
                Debug.Log("U key");
                gameObject.transform.localPosition = new Vector3(0.0f, 3500.0f, 0.0f);
            }
           

        }
    }
}
