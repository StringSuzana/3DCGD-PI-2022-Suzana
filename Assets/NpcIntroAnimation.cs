using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Characters
{
    public class NpcIntroAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Transform runToPoint;
        [SerializeField] private Button nextButton;

        [SerializeField] private int maxButtonPressCount;
        private int _buttonPressCount;
        private float lerpDuration = 5;
        private float startValue = 1;
        private float endValue = 0;
        private float valueToLerp;
        private static readonly int Blend = Animator.StringToHash("Blend");
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Run = Animator.StringToHash("Run");

        private void Start()
        {
            _buttonPressCount = 0;

            nextButton.onClick.AddListener(StartConversationAnimation);
            StartCoroutine(PlayBlendTree());
        }

        public void StartConversationAnimation()
        {
            Debug.Log("Button pressed, start coroutine");
            StartCoroutine(PlayConversationAnimation());
            _buttonPressCount++;
        }

        private IEnumerator PlayConversationAnimation()
        {
            Debug.Log("Play conversation coroutine");
            animator.SetBool(Idle, false);
            animator.SetFloat(Blend, 0);
            yield return new WaitForSeconds(5);
            animator.SetBool(Idle, true);
            yield return new WaitForSeconds(2);
        }

        private IEnumerator PlayBlendAnimation()
        {
            float startBlendValue = 1f;
            float targetBlendValue = 0f;
            float smoothness = 0.4f;

            animator.SetFloat(Blend, startBlendValue);
            yield return new WaitForSeconds(1);

            StartCoroutine(Lerp());
            animator.SetBool(Idle, false);

            yield return new WaitForSeconds(2);

            animator.SetFloat(Blend, targetBlendValue);
            yield return new WaitForSeconds(15);
        }

        private IEnumerator Lerp()
        {
            float timeElapsed = 0;
            while (timeElapsed < lerpDuration)
            {
                valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);

                animator.SetFloat(Blend, valueToLerp * Time.deltaTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            valueToLerp = endValue;
        }

        private IEnumerator PlayBlendTree()
        {
            StartCoroutine(PlayBlendAnimation());

            animator.SetBool(Idle, true);
            yield return new WaitForSeconds(2);

            Debug.Log("Finished first part of animation");
            while (_buttonPressCount < maxButtonPressCount)
            {
                yield return null;
            }

            animator.SetBool(Run, true);
            agent.SetDestination(runToPoint.position);
        }
    }
}