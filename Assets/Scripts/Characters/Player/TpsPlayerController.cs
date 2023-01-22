using System;
using Cinemachine;
using MyGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters
{
    public class TpsPlayerController : BasePlayerController, ITpsPlayer
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float grabDistance;
        [SerializeField] private Transform grabPoint;

        [SerializeField] private AudioClip grabAudioClip;
        private InputAction _grab;
        private bool _enableGrab = false;
        private Item _itemForGrab;
        [SerializeField] private InventoryObject inventoryOfBags;
        [SerializeField] private bool _stopPlayerMotion;

        #region Unity methods


        protected new void Awake()
        {
            base.Awake();
            _stopPlayerMotion = false;
        }

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
            if (_stopPlayerMotion == false)
            {
                HandleMovement();
                HandleRotation();
                RunIfAltKeyIsPressed();
            }

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

        protected new void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.DrawSphere(grabPoint.position, grabDistance);
        }

        #endregion

        protected override void HandleRotation()
        {
            Vector2 delta = _mouse.delta.ReadValue();
            float mouseX = delta.x * sensitivity * Time.deltaTime;
            float mouseY = delta.y * sensitivity * Time.deltaTime;


            // Rotate the player
            transform.Rotate(Vector3.up * mouseX);

            virtualCamera.transform.Rotate(Vector3.left * mouseY * 0.5f);
        }

        protected override void PlayWalkSoundFx()
        {
            if (sfxAudioSource.isPlaying)
                return;

            sfxAudioSource.pitch = 1.5f;
            sfxAudioSource.PlayOneShot(walkAudioClip);
        }

        protected override void PlayRunSoundFx()
        {
            if (sfxAudioSource.isPlaying)
                return;
            sfxAudioSource.pitch = 2.6f;
            sfxAudioSource.PlayOneShot(walkAudioClip);
        }

        private void Grab(InputAction.CallbackContext obj)
        {
            Debug.Log("Grab method");
            if (_enableGrab && _itemForGrab != null)
            {
                var itemGameObject = _itemForGrab.gameObject;
                Debug.Log($"itemGameObject {itemGameObject}");
                Debug.Log($"_itemForGrab: {_itemForGrab}");
                Debug.Log($"_itemForGrab.itemObject.name: {_itemForGrab.itemObject.name}");

                GrabVaccineBag(itemGameObject, _itemForGrab.itemObject);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            var item = other.GetComponent<Item>();
            if (item != null)
            {
                Debug.Log($"OnTriggerEnter {item}");
                _enableGrab = true;
                _itemForGrab = item;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            var item = other.GetComponent<Item>();
            if (item != null)
            {
                Debug.Log($"OnTriggerExit {item}");
                _enableGrab = false;
                _itemForGrab = null;
            }
        }

        public void GrabVaccineBag(GameObject itemGameObject, ItemObject bag)
        {
            Debug.Log("Add bag to inventory");
            inventoryOfBags.AddItem(bag, 1);
            Destroy(itemGameObject);
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

        public override bool IsVictory()
        {
            throw new System.NotImplementedException();
        }

        public override void GameOver()
        {
            throw new System.NotImplementedException();
        }

        public void StopPlayerMotion(bool stop)
        {
            _stopPlayerMotion = stop;
        }
    }
}