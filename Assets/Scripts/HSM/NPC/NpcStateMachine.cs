using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using HSM;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NpcStateMachine : MonoBehaviour
{
    [SerializeField] private TpsPlayerController player;
    [SerializeField] private GameObject timer;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    //Patroling
    [SerializeField] private Vector3 nextWalkPoint;
    [SerializeField] private GameObject[] wayPoints;

    [SerializeField] private Button nextButton; //listener for closed conversation

    private static readonly int Talk = Animator.StringToHash("Talk");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");

    //HSM
    public bool IsPatrolling = true;
    public bool IsPlayerInSight = false;

    public NpcBaseState CurrentState { get; set; }
    public NpcStateFactory StateFactory { get; set; }

    private void Awake()
    {
        StateFactory = new NpcStateFactory(this);
        CurrentState = StateFactory.Idle();
        CurrentState.EnterState();
    }

    private void Start()
    {
    }

    private void Update()
    {
        CurrentState.UpdateState();
        CurrentState.CheckSwitchStates();
    }
}
