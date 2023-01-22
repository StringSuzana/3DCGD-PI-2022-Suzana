using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Characters
{
    public class NpcIntroAnimation : MonoBehaviour
    {
        [SerializeField] private TpsPlayerController player;
        [SerializeField] private GameObject timer;
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Transform runToPoint;
        [SerializeField] private Button nextButton;

        [SerializeField] private int maxButtonPressCount;
        private int _buttonPressCount;
        private readonly float _lerpDuration = 5;
        private float _workOnGroundBlend = 0;
        private float _argueStandingBlend = 1;
        private float valueToLerp;
        private static readonly int Argue = Animator.StringToHash("Argue");
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Run = Animator.StringToHash("Run");
        private bool _isRunning = false;

        private void Start()
        {
            player.StopPlayerMotion(true);
            _buttonPressCount = 0;
            nextButton.onClick.AddListener(StartConversationAnimation);
            animator.SetBool(Idle, false);
            animator.SetFloat(Argue, _workOnGroundBlend);


        }

        public void StartIntroTrigger()
        {
            StartCoroutine(PlayBlendTree());

        }

        private void Update()
        {
            if (_isRunning && agent.hasPath && agent.remainingDistance < 0.1f)
            {
                Destroy(gameObject);
            }
        }

        private void StartConversationAnimation()
        {
            _buttonPressCount++;
            StartCoroutine(PlayConversationAnimation());
        }

        private IEnumerator PlayBlendTree()
        {
            animator.SetBool(Idle, false);

            StartCoroutine(Lerp());
            yield return new WaitForSeconds(_lerpDuration+1);

            animator.SetBool(Idle, true);
            yield return new WaitForSeconds(2);


            Debug.Log("Finished first part of animation");
            while (_buttonPressCount < maxButtonPressCount)
            {
                yield return null;
            }

            animator.SetBool(Run, true);
            agent.SetDestination(runToPoint.position);
            _isRunning = true;
            player.StopPlayerMotion(false);
            timer.GetComponent<GameTimer>().StartTimerTrigger();
        }

        private IEnumerator PlayConversationAnimation()
        {
            animator.SetBool(Idle, false);
            animator.SetFloat(Argue, _argueStandingBlend);
            yield return new WaitForSeconds(5);
            animator.SetBool(Idle, true);
            yield return new WaitForSeconds(2);
        }

        private IEnumerator Lerp()
        {
            float timeElapsed = 0;
            while (timeElapsed < _lerpDuration)
            {
                valueToLerp = Mathf.Lerp(_workOnGroundBlend, _argueStandingBlend, timeElapsed / _lerpDuration);

                animator.SetFloat(Argue, valueToLerp);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            valueToLerp = _argueStandingBlend;
        }
    }
}