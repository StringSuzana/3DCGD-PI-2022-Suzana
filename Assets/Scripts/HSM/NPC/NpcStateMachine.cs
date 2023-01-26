using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Global;
using HSM;
using MyGame;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NpcStateMachine : MonoBehaviour
{
    public IFpsPlayer IFpsPlayer;

    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;

    //[SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private Hint hintText;
    [SerializeField] private Canvas npcAlertCanvas;
    [SerializeField] private Canvas interactionInstructionsCanvas;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip interactClip;
    private PlayerInputActions _playerInput;
    private InputAction _talk;


    //Patroling
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Vector3 nextWalkPoint;
    [SerializeField] private GameObject[] wayPoints;
    private int _currentWayPoint = 0;

    [SerializeField] private float sightRange;

    #region Getters anSetters

    public Transform Player => player;
    public Animator Animator => animator;
    public AudioClip InteractClip => interactClip;
    public Hint HintText => hintText;
    public Canvas InteractionInstructionsCanvas => interactionInstructionsCanvas;
    public Canvas NpcAlertCanvas => npcAlertCanvas;
    public NavMeshAgent Agent => agent;
    public GameObject[] WayPoints => wayPoints;

    public int CurrentWayPoint
    {
        get => _currentWayPoint;
        set => _currentWayPoint = value;
    }

    public Vector3 NextWalkPoint
    {
        get => nextWalkPoint;
        set => nextWalkPoint = value;
    }

    #endregion

    public readonly int Talk = Animator.StringToHash("Talk");
    public readonly int Idle = Animator.StringToHash("Idle");
    public readonly int Walk = Animator.StringToHash("Walk");
    public readonly int InSight = Animator.StringToHash("InSight");

    //HSM
    //States
    //public bool IsPatrolling = true;
    public bool HasNextWayPoint = false;
    public bool IsPlayerInSight = false;
    public bool IsInInteractRange = false;
    public bool IsFollowingPlayer = false;
    public bool IsTalking = false;
    public bool IsIdle = false;

    public NpcBaseState CurrentState { get; set; }
    public NpcStateFactory StateFactory { get; set; }

    private void Awake()
    {
        _playerInput = new PlayerInputActions();

        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        IFpsPlayer = player.parent.GetComponent<IFpsPlayer>();

        StateFactory = new NpcStateFactory(this);
        CurrentState = StateFactory.Patrol();
        CurrentState.EnterState();
    }

    private void Update()
    {
        CurrentState.UpdateStates();
        CurrentState.CheckSwitchStates();

        if (!agent.pathPending && agent.remainingDistance < 0.5) HasNextWayPoint = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IPlayer>() == null) return;
        if (IsPlayerInSight == false)
        {
            IsPlayerInSight = true;
        }
        else
        {
            IsInInteractRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IPlayer>() == null) return;
        if (IsInInteractRange)
        {
            IsInInteractRange = false;
        }
        else
        {
            IsPlayerInSight = false;
        }
    }

    protected void OnEnable()
    {
        _talk = _playerInput.Player.Talk;
        _talk.Enable();

        _talk.performed += TalkPressed;
    }

    protected void OnDisable()
    {
        _talk.Disable();
    }

    private void TalkPressed(InputAction.CallbackContext callbackContext)
    {
        IsTalking = !HintManager.Instance.IsHintActive();
    }


    private void OnDrawGizmosSelected()
    {
        Vector3 npcPosition = transform.position;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(npcPosition, sightRange);
    }
}