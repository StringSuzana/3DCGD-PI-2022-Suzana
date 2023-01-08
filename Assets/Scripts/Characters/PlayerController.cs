using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Global;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace MyGame
{
    public class PlayerController : MonoBehaviour, IPlayer
    {
        [SerializeField] private GameObject GameManager;

        [Tooltip("Virtual camera on plazez")] public Camera camera;

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
        private InputAction _run;
        private InputAction _weaponChange;
        private Mouse _mouse;


        private float _rotation;
        private float _gravity = 9.8f;

        /**weapons**/
        private Weapon _selectedWeapon;

        private int _currentWeaponIndex;
        private List<Weapon> _weapons;
        private IWeaponService _weaponService;
        private static readonly int Run = Animator.StringToHash("run");
        private static readonly int ShootTriggerAnim = Animator.StringToHash("shoot");
        private static readonly int JumpTriggerAnim = Animator.StringToHash("jump");
        private static readonly int SpeedFloatAnim = Animator.StringToHash("speed");
        private static readonly int DieTriggerAnim = Animator.StringToHash("die");


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
            RunIfAltKeyIsPressed();
            #endif

            #if ENABLE_LEGACY_INPUT_MANAGER
            HandleRotation_OldInputSystem();
            HandleMovement_OldInputSystem();
            RunIfAltKeyIsPressed();
            #endif

            if (IsVictory())
            {
                Debug.Log("VICTORY");
            }
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


        private void RunIfAltKeyIsPressed()
        {
            if (_keyboard.altKey.wasPressedThisFrame)
            {
                animator.SetBool(Run, true);
                speed += 5;
                PlayRunSoundFx();
            }
            else if (_keyboard.altKey.isPressed)
            {
                PlayRunSoundFx();
            }

            else if (_keyboard.altKey.wasReleasedThisFrame)
            {
                speed -= 5;
                animator.SetBool(Run, false);
            }
        }


        private void OnDisable()
        {
            _look.Disable();
            _fire.Disable();
            _move.Disable();
            _jump.Disable();
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
                animator.SetTrigger(DieTriggerAnim);
                GameManager.GetComponent<IGameManager>().PlayGameOver();
                //TODO
                //Start timeline
                //game over!
                //Display something
            }
            else
            {
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
        }

        public void Heal(int healAmount)
        {
            _currentHealth += healAmount;
            if (_currentHealth > GameData.MaxPlayerHealth)
            {
                _currentHealth = GameData.MaxPlayerHealth;
            }

            healthBar.SetHealth(_currentHealth);
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
            AnimateMovement(moveVector);
            characterController.Move(moveVector - velocity);
        }

        private void AnimateMovement(Vector3 moveVector)
        {
            if (IsPlayerRunning()) return;
            if (moveVector.x > 0 || moveVector.z > 0)
            {
                animator.SetFloat(SpeedFloatAnim, speed);
                PlayWalkSoundFx();
            }
            else
            {
                animator.SetFloat(SpeedFloatAnim, 0);
                StopCurrentSoundFx();
            }
        }

        private bool IsPlayerRunning() => _keyboard.altKey.isPressed;

        private void StopCurrentSoundFx()
        {
            audioSource.Stop();
        }

        private void PlayWalkSoundFx()
        {
            if (audioSource.isPlaying)
                return;

            audioSource.pitch = 0.5f;
            audioSource.PlayOneShot(walkAudioClip);
        }

        private void PlayRunSoundFx()
        {
            if (audioSource.isPlaying)
                return;
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(walkAudioClip);
        }

        private IEnumerator Jump()
        {
            animator.SetTrigger(JumpTriggerAnim);

            yield return new WaitForSeconds(0.5f);

            velocity.y = jumpHeight;
        }

        private bool CanShoot()
        {
            //TODO
            return true;
        }

        private IEnumerator Shoot()
        {
            animator.SetTrigger(ShootTriggerAnim);

            yield return new WaitForSeconds(1f);

            _selectedWeapon.Shoot();
        }

        private void Shoot(InputAction.CallbackContext ctx)
        {
            if (CanShoot())
            {
                StartCoroutine(Shoot());
            }
        }

        private void JumpInput(InputAction.CallbackContext ctx)
        {
            if (IsGrounded())
            {
                StartCoroutine(Jump());
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

        /**Victory is when there are no more enemies in character container*/
        public bool IsVictory()
        {
            var characters = charactersContainer.GetComponentsInChildren<Transform>();
            return characters.All(item => !item.gameObject.CompareTag("Enemy"));
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

        private void HandleMovement_OldInputSystem()
        {
            float moveX = Input.GetAxis(InputNames.Horizontal);
            float moveZ = Input.GetAxis(InputNames.Vertical);

            var transform1 = characterController.transform;
            Vector3 moveVector = transform1.forward * moveZ;
            moveVector += transform1.right * moveX;

            moveVector *= speed * Time.deltaTime;
            velocity.y += _gravity * Time.deltaTime * Time.deltaTime;

            if (IsGrounded() && velocity.y < 0)
            {
                velocity.y = 0;
            }

            characterController.Move(moveVector + velocity);
        }
    }

    public interface IPlayer
    {
        IEnumerator TakeDamage(float damageAmount);
        void Heal(int healAmount);
    }
}