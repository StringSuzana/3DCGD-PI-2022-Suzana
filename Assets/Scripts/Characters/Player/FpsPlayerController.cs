using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public class FpsPlayerController : BasePlayerController, IFpsPlayer
    {
        [SerializeField] private GameObject charactersContainer;

        [SerializeField] private Transform shootFromPoint;
        [SerializeField] private GameObject scratchView;

        private InputAction _fire;
        private InputAction _weaponChange;
        private Weapon _selectedWeapon;
        private int _currentWeaponIndex;
        private List<Weapon> _weapons;
        private IWeaponService _weaponService;
        private static readonly int ShootTriggerAnim = Animator.StringToHash("shoot");

        #region Unity methods

        protected override void Start()
        {
            _currentHealth = PlayerPrefs.GetFloat(PlayerPrefNames.Health);
            _weaponService = ServiceProvider.WeaponService();
            _weapons = _weaponService.GetWeapons(shootFromPoint);
            SelectWeapon();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        protected void FixedUpdate()
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
        }

        protected override void Update()
        {
            //velocity.y -= _gravity * Time.deltaTime;
            //characterController.Move(velocity * Time.deltaTime);

            if (IsVictory())
            {
                Debug.Log("VICTORY");
                gameManager.GetComponent<IGameManager>().PlayLevelCompleted();
            }
        }

        protected new void OnEnable()
        {
            base.OnEnable();
            _fire = _playerInput.Player.Fire;
            _fire.Enable();
            _fire.performed += Shoot;

            _weaponChange = _playerInput.Player.WeaponChange;
            _weaponChange.Enable();
            _weaponChange.performed += ChangeWeapon;
        }

        protected new void OnDisable()
        {
            base.OnDisable();
            _fire.Disable();
            _weaponChange.Disable();
        }

        #endregion


        public override bool IsVictory()
        {
            /**Victory is when there are no more enemies in character container*/
            var characters = charactersContainer.GetComponentsInChildren<Transform>();
            return characters.All(item => !item.gameObject.CompareTag("Enemy"));
        }

        public override void GameOver()
        {
            PlayerPrefs.SetFloat(PlayerPrefNames.Health, 0);
            _currentHealth = 0;

            //stop enemies
            Transform[] enemies = charactersContainer.GetComponentsInChildren<Transform>();
            foreach (Transform enemy in enemies)
            {
                if (enemy.TryGetComponent(out IEnemy enemyCharacter))
                {
                    enemyCharacter.Stop();
                }
            }

            sfxAudioSource.Stop();
            animator.SetTrigger(DieTriggerAnim);
            gameManager.GetComponent<IGameManager>().PlayGameOverTimeline();
        }

        protected override void HandleRotation()
        {
            Vector2 delta = _mouse.delta.ReadValue();
            float x = delta.x * sensitivity * Time.deltaTime;
            float y = delta.y * sensitivity * Time.deltaTime;

            characterController.transform.Rotate(Vector3.up * x);

            _rotation -= y;
            _rotation = Mathf.Clamp(_rotation, -70f, 60f);

            camera.transform.localRotation = Quaternion.Euler(_rotation, 0f, 0f);
            shootFromPoint.localRotation = Quaternion.Euler(_rotation,   0f, 0f);
        }

        #region IPlayer methods

        public IEnumerator TakeDamage(float damageAmount)
        {
            _currentHealth -= damageAmount;
            if (_currentHealth <= 0)
            {
                GameOver();
            }
            else
            {
                PlayerPrefs.SetFloat(PlayerPrefNames.Health, _currentHealth);
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

            PlayerPrefs.SetFloat(PlayerPrefNames.Health, _currentHealth);
            healthBar.SetHealth(_currentHealth);
        }

        #endregion


        private bool CanShoot()
        {
            //TODO
            return true;
        }

        private IEnumerator ShootAction()
        {
            animator.SetTrigger(ShootTriggerAnim);

            yield return new WaitForSeconds(1f);

            _selectedWeapon.Shoot();
        }

        private void Shoot(InputAction.CallbackContext ctx)
        {
            if (CanShoot())
            {
                StartCoroutine(ShootAction());
            }
        }

        private void ChangeWeapon(InputAction.CallbackContext ctx)
        {
            _currentWeaponIndex = (_currentWeaponIndex + 1) % _weapons.Count;
            SelectWeapon();
        }

        protected void SelectWeapon()
        {
            _selectedWeapon = _weapons[_currentWeaponIndex];
            UIManager.shared.SetWeaponName(_selectedWeapon.Name);
        }

        #region Old input system

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

        #endregion
    }
}