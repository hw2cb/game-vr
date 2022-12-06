using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Игрок")]
    public Transform player2;
    private float distance;

    [Header("Радиус видимости")]
    public float radius = 20;

    [Header("Радиус подозрения")]
    public float preRadius = 30;


    private NavMeshAgent agent;
    private Animator animator;

    [Header("Позиции")]
    public List<Transform> positions;


    private Transform player;

    private bool isRun = false;

    /*
     *  Радиус, когда олень начинает тригериться и вертеть бошкой, после этого, если игрок
     *  не выйдет из радиуса в течении 3 секунд, олень начинает убегать
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
