using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("�����")]
    public Transform player;
    private float distance;

    [Header("������ ���������")]
    public float radius = 20;

    [Header("������ ����������")]
    public float preRadius = 30;


    private NavMeshAgent agent;
    private Animator animator;

    [Header("�������")]
    public List<Transform> positions;

    /*
     *  ������, ����� ����� �������� ����������� � ������� ������, ����� �����, ���� �����
     *  �� ������ �� ������� � ������� 3 ������, ����� �������� �������
     *     
     */
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);
        if (distance >= radius)
        {
            agent.enabled = false;
            animator.speed = 0.37f;
            animator.Play("Naklon");
        }
            

        if(distance < radius)
        {
            Debug.Log("Run");
            
            //run
            agent.enabled = true;
            //agent.destination = enemy.position;

            Vector3 newPos = player.forward * 30;
            Debug.Log(newPos);
            agent.SetDestination(newPos);
            animator.speed = 1f;
            animator.Play("Run");
        }
        if(distance <= 1.5)
        {
            //transform.LookAt(enemy);
            agent.enabled = false;
            animator.speed = 0.37f;
            animator.Play("Naklon");
        }
    }
}
