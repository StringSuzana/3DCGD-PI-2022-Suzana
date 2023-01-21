using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Data;
using Global;
using MyGame;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using Weapons;

namespace Characters
{
    public abstract class BasePlayerController : MonoBehaviour
    {
        [SerializeField] protected GameObject gameManager;
        [SerializeField] protected HealthBar healthBar;
        [SerializeField] protected CharacterController characterController;
        [SerializeField] protected Animator animator;
        [SerializeField] protected float sensitivity;
        [SerializeField] protected float speed;

        protected float _lastSpeed;
        [SerializeField] protected float jumpHeight;
        [SerializeField] protected Vector3 velocity = Vector3.zero;
        [SerializeField] protected Transform groundDetector;
        [SerializeField] protected LayerMask groundLayer;
        [SerializeField] protected AudioSource sfxAudioSource;
        [SerializeField] protected AudioClip walkAudioClip;
        [SerializeField] protected AudioClip jumpAudioClip;
        [SerializeField] protected AudioClip hurtAudioClip;

        protected float _currentHealth;
        protected PlayerInputActions _playerInput;
        protected InputAction _move;
        protected Keyboard _keyboard;
        protected InputAction _look;
        protected InputAction _jump;
        protected InputAction _run;
        protected Mouse _mouse;
        protected float _rotation;
        protected float _gravity = 1.8f;
        protected float _cameraJumpHeight;

        protected static readonly int Run = Animator.StringToHash("run");
        protected static readonly int JumpTriggerAnim = Animator.StringToHash("jump");
        protected static readonly int SpeedFloatAnim = Animator.StringToHash("speed");
        protected static readonly int DieTriggerAnim = Animator.StringToHash("die");

        #region Unity methods

        private void Awake()
        {
            _playerInput = new PlayerInputActions();
            _mouse = InputSystem.GetDevice<Mouse>();
            _keyboard = InputSystem.GetDevice<Keyboard>();
            _cameraJumpHeight = jumpHeight * 5;
        }

        protected abstract void Start();

        protected abstract void Update();


        protected void OnEnable()
        {
            _move = _playerInput.Player.Move;
            _move.Enable();

            _jump = _playerInput.Player.Jump;
            _jump.Enable();
            _jump.performed += JumpInput;


            _look = _playerInput.Player.Look;
            _look.Enable();
        }

        protected void OnDisable()
        {
            _look.Disable();
            _move.Disable();
            _jump.Disable();
        }

        protected void OnDrawGizmos()
        {
            Gizmos.DrawSphere(groundDetector.position, 0.3f);
        }

        #endregion

        protected void RunIfAltKeyIsPressed()
        {
            if (_keyboard.altKey.wasPressedThisFrame)
            {
                animator.SetBool(Run, true);
                speed += 5;
                _lastSpeed = speed;
                PlayRunSoundFx();
            }
            else if (_keyboard.altKey.isPressed)
            {
                PlayRunSoundFx();
            }

            else if (_keyboard.altKey.wasReleasedThisFrame)
            {
                speed -= 5;
                _lastSpeed = speed;
                animator.SetBool(Run, false);
            }
        }

        protected void PlayWalkSoundFx()
        {
            if (sfxAudioSource.isPlaying)
                return;

            sfxAudioSource.pitch = 0.5f;
            sfxAudioSource.PlayOneShot(walkAudioClip);
        }

        protected void PlayJumpSoundFx()
        {
            if (sfxAudioSource.isPlaying)
                sfxAudioSource.Stop();

            sfxAudioSource.pitch = 0.5f;
            sfxAudioSource.PlayOneShot(jumpAudioClip);
        }

        protected void PlayRunSoundFx()
        {
            if (sfxAudioSource.isPlaying)
                return;
            sfxAudioSource.pitch = 1f;
            sfxAudioSource.PlayOneShot(walkAudioClip);
        }

        public abstract bool IsVictory();
        public abstract void GameOver();
        protected abstract void HandleRotation();

        protected void HandleMovement()
        {
            float moveX = _move.ReadValue<Vector2>().x;
            float moveZ = _move.ReadValue<Vector2>().y;

            Transform t = characterController.transform;

            Vector3 moveVector = t.forward * moveZ;
            moveVector += t.right * moveX;
            moveVector *= speed * Time.deltaTime;

            /*gravity*/
            velocity.y -= _gravity * Time.deltaTime;
            if (IsGrounded() && velocity.y < 0)
            {
                velocity.y = 0;
            }

            characterController.Move(moveVector + velocity);
            AnimateMovement(moveVector);
        }


        protected void AnimateMovement(Vector3 moveVector)
        {
            animator.SetFloat(SpeedFloatAnim, speed);

            if (IsPlayerRunning()) return;
            if (moveVector.x != 0 || moveVector.z != 0)
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

        protected void StopCurrentSoundFx()
        {
            sfxAudioSource.Stop();
        }

        private bool IsPlayerRunning() => _keyboard.altKey.isPressed;

        protected bool IsGrounded()
        {
            return Physics.CheckSphere(groundDetector.position, 0.3f, groundLayer);
        }

        protected IEnumerator JumpAction()
        {
            animator.SetTrigger(JumpTriggerAnim);
            Debug.Log($"[JumpAction 1] Velocity.y {velocity.y} ");

            yield return new WaitForSeconds(0.5f);

            velocity.y = jumpHeight;
            PlayJumpSoundFx();
            Debug.Log($"[JumpAction 2] Velocity.y {velocity.y} ");
        }

        protected void JumpInput(InputAction.CallbackContext ctx)
        {
            if (IsGrounded())
            {
                StartCoroutine(JumpAction());
            }
        }
    }
}