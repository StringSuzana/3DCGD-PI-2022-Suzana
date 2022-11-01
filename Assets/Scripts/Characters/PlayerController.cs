using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace MyGame
{
    public class PlayerController : MonoBehaviour, IPlayer
    {
        [Tooltip("Hides default camera")] public new Camera camera;

        [SerializeField] private HealthBar healthBar;

        [SerializeField] private GameObject charactersContainer;

        [SerializeField] private Transform shootFromPoint;

        [SerializeField] [Tooltip("Motion")] private CharacterController characterController;

        [SerializeField] private Animator animator;

        [SerializeField] private float sensitivity;

        [SerializeField] private float speed;

        [SerializeField] private float jumpHeight;

        [SerializeField] private Vector3 velocity = Vector3.zero;

        [Tooltip("Ground")] [SerializeField] private Transform groundDetector;

        [SerializeField] private LayerMask groundLayer;


        [Tooltip("Damage intake")] [SerializeField]
        private GameObject scratchView;

        [Tooltip("Audio sources")] [SerializeField]
        private AudioSource audioSource;

        [SerializeField] private AudioClip walkAudioClip;
        [SerializeField] private AudioClip jumpAudioClip;
        [SerializeField] private AudioClip hurtAudioClip;

        /**Health**/
        private float _currentHealth;

        /**Motion**/
        private PlayerInputActions _playerInput;

        private InputAction _move;
        private InputAction _fire;
        private Keyboard _keyboard;
        private InputAction _look;
        private InputAction _jump;
        private InputAction _weaponChange;
        private Mouse _mouse;


        private float _rotation;
        private float _gravity = 9.8f;

        /**weapons**/
        private Weapon _selectedWeapon;

        private int _currentWeaponIndex;
        private List<Weapon> _weapons;
        private IWeaponService _weaponService;


        private void Awake()
        {
            _playerInput = new PlayerInputActions();
            _mouse = InputSystem.GetDevice<Mouse>();
            _keyboard = InputSystem.GetDevice<Keyboard>();
        }

        private void Start()
        {
            _currentHealth = PlayerPrefs.GetFloat(PlayerPrefNames.Health);
            _weaponService = ServiceProvider.WeaponService();
            _weapons = _weaponService.GetWeapons(shootFromPoint);
            SelectWeapon();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Update()
        {
            #if ENABLE_INPUT_SYSTEM
            HandleRotation();
            HandleMovement();
            #endif

            #if ENABLE_LEGACY_INPUT_MANAGER
            HandleRotation_OldInputSystem();
            HandleMovement_OldInputSystem();
           

            
            

            //move character
            characterController.Move(moveVector + velocity);

            //animate walk and run
            if ((moveVector + velocity).x > 0 || (moveVector + velocity).z > 0)
            {
                animator.SetFloat("speed", speed);
                if (!audioSource.isPlaying)
                {
                    audioSource.pitch = 0.5f;
                    audioSource.PlayOneShot(walkAudioClip);
                }
            }
            else
            {
                animator.SetFloat("speed", 0);
                audioSource.Stop();
            }
            if (Input.GetButtonDown(InputNames.Run))
            {
                animator.SetBool("run", true);
                speed += 5;

                audioSource.pitch = 1f;
                audioSource.PlayOneShot(walkAudioClip);
                Debug.Log("run");
            }
            if (Input.GetButtonUp(InputNames.Run))
            {
                speed -= 5;
                animator.SetBool("run", false);
            }


            if (IsVictory())
            {
                Debug.Log("VICTORY");
            }
            #endif
        }

        private void OnEnable()
        {
            _move = _playerInput.Player.Move;
            _move.Enable();

            _jump = _playerInput.Player.Jump;
            _jump.Enable();
            _jump.performed += JumpInput;

            _look = _playerInput.Player.Look;
            _look.Enable();

            _fire = _playerInput.Player.Fire;
            _fire.Enable();
            _fire.performed += Shoot;

            _weaponChange = _playerInput.Player.WeaponChange;
            _weaponChange.Enable();
            _weaponChange.performed += ChangeWeapon;
        }

        private void OnDisable()
        {
            _look.Disable();
            _fire.Disable();
            _move.Disable();
            _weaponChange.Disable();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(groundDetector.position, 0.3f);
        }


        public IEnumerator TakeDamage(float damageAmount)
        {
            _currentHealth -= damageAmount;
            if (_currentHealth <= 0)
            {
                //TODO
                //game over!
            }

            healthBar.SetHealth(_currentHealth);
            scratchView.SetActive(true);
            var image = scratchView.GetComponentInChildren<Canvas>();

            yield return new WaitForSecondsRealtime(0.4f);
            image.enabled = false;

            yield return new WaitForSecondsRealtime(0.2f);
            image.enabled = true;

            yield return new WaitForSecondsRealtime(0.2f);
            scratchView.SetActive(false);
        }

        private void HandleRotation()
        {
            var delta = _mouse.delta.ReadValue();
            var x = delta.x * sensitivity * Time.deltaTime;
            var y = delta.y * sensitivity * Time.deltaTime;

            characterController.transform.Rotate(Vector3.up * x);

            _rotation -= y;
            _rotation = Mathf.Clamp(_rotation, -70f, 60f);

            camera.transform.localRotation = Quaternion.Euler(_rotation, 0f, 0f);
            shootFromPoint.localRotation = Quaternion.Euler(_rotation,   0f, 0f);
        }

        private void HandleRotation_OldInputSystem()
        {
            float x = Input.GetAxis(InputNames.MouseX) * sensitivity * Time.deltaTime;
            float y = Input.GetAxis(InputNames.MouseY) * sensitivity * Time.deltaTime;

            characterController.transform.Rotate(Vector3.up * x);

            _rotation -= y;
            _rotation = Mathf.Clamp(_rotation, -70f, 60f);

            camera.transform.localRotation = Quaternion.Euler(_rotation, 0f, 0f);
            shootFromPoint.localRotation = Quaternion.Euler(_rotation,   0f, 0f);
        }


        private void HandleMovement()
        {
            float moveX = _move.ReadValue<Vector2>().x;
            float moveZ = _move.ReadValue<Vector2>().y;

            Transform t = characterController.transform;

            Vector3 moveVector = t.forward * moveZ;
            moveVector += t.right * moveX;
            moveVector *= speed * Time.deltaTime;

            /*gravity*/
            velocity.y += _gravity * Time.deltaTime * Time.deltaTime;

            if (IsGrounded() && velocity.y < 0)
            {
                velocity.y = 0;
            }

            //Todo: minus velocity?
            characterController.Move(moveVector - velocity);
        }


        private void HandleMovement_OldInputSystem()
        {
            //Movement
            float moveX = Input.GetAxis(InputNames.Horizontal);
            float moveZ = Input.GetAxis(InputNames.Vertical);

            Vector3 moveVector = characterController.transform.forward * moveZ;
            moveVector += characterController.transform.right * moveX;

            moveVector *= speed * Time.deltaTime;
        }

        private IEnumerator Jump()
        {
            animator.SetTrigger("jump");

            yield return new WaitForSeconds(0.5f);
            Debug.Log($"velocity.y={velocity.y}");
            velocity.y = jumpHeight;
        }

        private void Shoot(InputAction.CallbackContext ctx)
        {
            _selectedWeapon.Shoot();
        }

        private void JumpInput(InputAction.CallbackContext ctx)
        {
            if (IsGrounded())
            {
                Debug.Log($"Grounded");

                StartCoroutine(Jump());
            }
            else
            {
                Debug.Log($"NOT Grounded");

            }
        }

        private void ChangeWeapon(InputAction.CallbackContext ctx)
        {
            _currentWeaponIndex = (_currentWeaponIndex + 1) % _weapons.Count;
            SelectWeapon();
        }

        private bool IsGrounded()
        {
            return Physics.CheckSphere(groundDetector.position, 0.3f, groundLayer);
        }

        private void SelectWeapon()
        {
            _selectedWeapon = _weapons[_currentWeaponIndex];
            UIManager.shared.SetWeaponName(_selectedWeapon.name);
        }

        public bool IsVictory()
        {
            var characters = charactersContainer.GetComponentsInChildren<Transform>();
            foreach (var item in characters)
                if (item.gameObject.tag == "Enemy")
                    return false;

            return true;
        }
    }

    public interface IPlayer
    {
        IEnumerator TakeDamage(float damageAmount);
    }
}