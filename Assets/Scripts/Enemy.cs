using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("�����")]
    public Transform player2;
    private float distance;

    [Header("������ ���������")]
    public float radius = 20;

    [Header("������ ����������")]
    public float preRadius = 30;


    private NavMeshAgent agent;
    private Animator animator;

    [Header("�������")]
    public List<Transform> positions;


    private Transform player;

    private bool isRun = false;

    /*
     *  ������, ����� ����� �������� ����������� � ������� ������, ����� �����, ���� �����
     *  �� ������ �� ������� � ������� 3 ������, ����� �������� �������
     *     
     */
    private void Start()
    {
        player = GameObject.Find("Main Camera").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);
        Debug.Log(distance);

        if(distance < radius && !isRun)
        {
            Run();
        }
        if(distance > 100)
        {
            isRun = false;
            agent.enabled = false;
            animator.speed = 0.37f;
            animator.Play("Naklon");
        }
    }

    private void Run()
    {
        //run
        int numberPoint = Random.Range(0, 8);
        agent.enabled = true;
        agent.SetDestination(positions[numberPoint].position);
        animator.speed = 1.5f;
        animator.Play("Run");
        isRun = true;
    }
}
