using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Игрок")]
    public Transform player;
    private float distance;

    [Header("Радиус видимости")]
    public float radius = 20;

    [Header("Радиус подозрения")]
    public float preRadius = 30;


    private NavMeshAgent agent;
    private Animator animator;

    [Header("Позиции")]
    public List<Transform> positions;

    /*
     *  Радиус, когда олень начинает тригериться и вертеть бошкой, после этого, если игрок
     *  не выйдет из радиуса в течении 3 секунд, олень начинает убегать
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
