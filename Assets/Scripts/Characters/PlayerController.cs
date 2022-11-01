using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyGame
{
    public class PlayerController : MonoBehaviour, IPlayer
    {
        public HealthBar HealthBar;

        public GameObject charactersContainer;
        [Tooltip("Camera")] public new Camera camera;

        [Tooltip("Shooting")] public Transform shootFromPoint;

        [Tooltip("Motion")] public CharacterController characterController;

        public Animator animator;

        public float sensitivity;

        public float speed;

        public float jumpHeight;

        public Vector3 velocity = Vector3.zero;

        [Tooltip("Ground")] public Transform groundDetector;
        public LayerMask groundLayer;


        [Tooltip("Damage intake")] public GameObject scratchView;

        [Tooltip("Audio sources")] public AudioSource audioSource;

        public AudioClip walkAudioClip;
        public AudioClip jumpAudioClip;
        public AudioClip hurtAudioClip;
        private readonly InputAction _move;
        private float _currentHealth;
        private int _currentWeaponIndex;
        private InputAction _fire;
        private Keyboard _keyboard;
        private InputAction _look;
        private Mouse _mouse;

        [Header("Input Settings")] private PlayerInputActions _playerInput;
        private float _rotation;
        private Weapon _selectedWeapon;
        private InputAction _weaponChange;

        //weapons
        private List<Weapon> _weapons;
        private IWeaponService _weaponService;

        public PlayerController(InputAction move)
        {
            _move = move;
        }

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
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            //Rotation of a player
            float x = Input.GetAxis(InputNames.MouseX) * sensitivity * Time.deltaTime;
            float y = Input.GetAxis(InputNames.MouseY) * sensitivity * Time.deltaTime;

            characterController.transform.Rotate(Vector3.up * x);

            rotation -= y;
            rotation = Mathf.Clamp(rotation, -70f, 60f);

            camera.transform.localRotation = Quaternion.Euler(rotation, 0f, 0f);
            //So player can shoot where she looks at
            shootFromPoint.localRotation = Quaternion.Euler(rotation, 0f, 0f);

            //Movement
            float moveX = Input.GetAxis(InputNames.Horizontal);
            float moveZ = Input.GetAxis(InputNames.Vertical);

            Vector3 moveVector = characterController.transform.forward * moveZ;
            moveVector += characterController.transform.right * moveX;

            moveVector *= speed * Time.deltaTime;

            //gravity
            velocity.y += gravity * Time.deltaTime * Time.deltaTime;

            if (IsGrounded() && velocity.y < 0)
            {
                velocity.y = 0;
            }

            //jump
            if (Input.GetButtonDown(InputNames.JumpButton) && IsGrounded())
            {
                StartCoroutine(Jump());
            }

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
            _look = _playerInput.Player.Look;
            _look.Enable();
            //look.performed += LookAt;

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

            HealthBar.SetHealth(_currentHealth);
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
            var pos = _mouse.delta.ReadValue();
            Debug.Log($"Position: {pos}");
            var x = pos.x * sensitivity * Time.deltaTime;
            var y = pos.y * sensitivity * Time.deltaTime;
            /* Ray ray = camera.ScreenPointToRay(pos);
 
             if (Physics.Raycast(ray, out var hit, 100))
             {
                 transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
             }*/

            characterController.transform.Rotate(Vector3.up * x);

            _rotation -= y;
            _rotation = Mathf.Clamp(_rotation, -70f, 60f);

            camera.transform.localRotation = Quaternion.Euler(_rotation, 0f, 0f);
            //So player can shoot where she looks at
            shootFromPoint.localRotation = Quaternion.Euler(_rotation, 0f, 0f);
        }

        private IEnumerator Jump()
        {
            animator.SetTrigger("jump");

            yield return new WaitForSeconds(0.5f);

            velocity.y = jumpHeight;
        }

        private void Shoot(InputAction.CallbackContext ctx)
        {
            _selectedWeapon.Shoot();
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