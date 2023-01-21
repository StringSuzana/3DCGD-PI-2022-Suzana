using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Data;
using Global;
using MyGame;
using UnityEditor.Recorder.Input;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using Weapons;

namespace Characters
{
    public class TpsPlayerController : BasePlayerController, ITpsPlayer
    {
        
        [SerializeField]
        private float grabDistance;
        [SerializeField] private Transform grabPoint;

        [SerializeField]
        private AudioClip grabAudioClip;
        private InputAction _grab;
        [SerializeField]
        private InventoryObject inventoryOfBags;

        protected override void Start()
        {
            _currentHealth = PlayerPrefs.GetFloat(PlayerPrefNames.Health);
            if (inventoryOfBags == null)
            {
                inventoryOfBags = ScriptableObject.CreateInstance<InventoryObject>();
            }

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

        }
        protected override void Update()
        {
            HandleMovement();
            HandleRotation();
            HandleInteractions();
        }

        protected new void OnEnable()
        {
            base.OnEnable();
            _grab = _playerInput.Player.Grab;
            _grab.Enable();
            _grab.performed += Grab;
        }

        protected new void OnDisable()
        {
            base.OnDisable();
            _grab.Disable();
        }


        private void HandleMovement()
        {
            float moveX = _move.ReadValue<Vector2>().x;
            float moveZ = _move.ReadValue<Vector2>().y;

            Transform t = characterController.transform;
            Vector3 moveVector = t.forward * moveZ;
            moveVector += t.right * moveX;
            moveVector *= speed * Time.deltaTime;

            AnimateMovement(moveVector);
            characterController.Move(moveVector);
        }

        public override bool IsVictory()
        {
            throw new System.NotImplementedException();
        }

        public override void GameOver()
        {
            throw new System.NotImplementedException();
        }

        protected override void HandleRotation()
        {
            Vector2 delta = _mouse.delta.ReadValue();
            float mouseX = delta.x * sensitivity * Time.deltaTime;
            float mouseY = delta.y * sensitivity * Time.deltaTime;


            // Rotate the player
            transform.Rotate(Vector3.up * mouseX);

            virtualCamera.m_YAxis.Value += mouseX * sensitivity;


            //camera.transform.Rotate(Vector3.left * mouseY);
            //camera.transform.LookAt(transform);
        }

        private void Grab(InputAction.CallbackContext obj)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, grabDistance))
            {
                BagObject bag = hit.collider.GetComponent<BagObject>();
                if (bag != null)
                {
                    GrabVaccineBag(bag);
                }
            }
        }
       
        public void GrabVaccineBag(BagObject bag)
        {
            Debug.Log("Add bag to inventory");
            inventoryOfBags.AddItem(bag, 1);
            Destroy(bag);
        }
        private void HandleInteractions()
        {
            if (!_keyboard.eKey.wasPressedThisFrame) return;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, grabDistance);
            foreach (Collider col in hitColliders)
            {
                INpc npc = col.GetComponent<INpc>();
                npc?.Interact();
            }
        }
        protected new void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.DrawSphere(grabPoint.position, grabDistance);
        }

      
    }
}