using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{

    public AnimationClip a_idle;
    public AnimationClip a_walk;
    public AnimationClip a_run;
    public AnimationClip a_jump;

    //public Transform target;
    public Vector3 target;
    public int moveSpeed;
    public float rotationSpeed;
    float corutTimer;
    float dist;
    float rRot;

    Ray ray, rayUp, rayDown, rayDownLeft, rayDownRight, rayRight, rayLeft, rayRun;
    RaycastHit hit, hitDown, hitRight, hitLeft;

    Vector3 p, p1;

    bool startCast;
    bool turnedAccess;
    int tempTurn;

    Animation anim;
    private Transform myTrans;

    void Awake()
    {
        turnedAccess = true;
        myTrans = transform;
        moveSpeed = Mathf.RoundToInt(Random.Range(6, 8.5f));
        rotationSpeed = moveSpeed * 0.5f;
        corutTimer = 0.3f / moveSpeed;
        startCast = true;
        anim = this.GetComponent<Animation>();
    }

    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        //target = go.transform;
        StartCoroutine(OrientationEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        RunFromPlayer();
    }

    void RunFromPlayer()
    {
        if (target != Vector3.zero)
        {
            if (dist > 2)
            {
                if (Physics.Raycast(rayDown, out hitDown, 2))
                {
                    if (turnedAccess)
                    {
                        Quaternion rawRotation = Quaternion.Slerp(myTrans.rotation,
                                                                     Quaternion.LookRotation(target - myTrans.position),
                                                                 rotationSpeed * Time.deltaTime);
                        myTrans.rotation = new Quaternion(0, rawRotation.y, 0, rawRotation.w);
                    }

                    myTrans.position += myTrans.forward * moveSpeed * Time.deltaTime;
                }
                else
                {
                    myTrans.rotation *= Quaternion.Euler(new Vector3(0, -45, 0));
                }


            }
            else
            {
                anim.Play(a_idle.name);
            }
        }
        else anim.Play(a_idle.name);
    }

    IEnumerator OrientationEnemy()
    {
        while (startCast)
        {
            yield return new WaitForSeconds(corutTimer);
            //проверяем дистанцию до цели
            if (target != null)
            {
                dist = Vector3.Distance(myTrans.position, target);
            }
            //проверяем наличие поверхности под телом
            rayDown = new Ray(transform.position, transform.forward + Vector3.down);
            rayDownLeft = new Ray(transform.position, -transform.right + Vector3.down);
            rayDownRight = new Ray(transform.position, transform.right + Vector3.down);
            //если дистанция до цели больше и есть поверхность делаем проверки для определения непроходимых мест
            if (dist > 2 && Physics.Raycast(rayDown, out hitDown, 2))
            {       //луч вперед из центра
                ray = new Ray(transform.position, transform.forward);
                //луч около головы
                rayUp = new Ray(transform.position + Vector3.up * 0.5f, transform.forward);

                //проверяем левую и правую сторону
                rayRight = new Ray(transform.position + Vector3.down * 0.5f, transform.right + Vector3.up * 0.5f);
                rayLeft = new Ray(transform.position + Vector3.down * 0.5f, -transform.right + Vector3.up * 0.5f);

                if (Physics.Raycast(rayRight, out hitRight, 2) && hitRight.collider.name != "Enemy" || Physics.Raycast(rayLeft, out hitLeft, 2) && hitLeft.collider.name != "Enemy")
                {
                    turnedAccess = false;
                }
                else
                {
                    if (Physics.Raycast(rayDownLeft, out hitDown, 2) && Physics.Raycast(rayDownRight, out hitDown, 2))
                        turnedAccess = true;
                }

                if ((Physics.Raycast(ray, out hit, 2) && hit.collider.name == "Collider") || (Physics.Raycast(rayUp, out hit, 2) && hit.collider.name != "Enemy"))
                {
                    tempTurn = Mathf.RoundToInt(Random.Range(0, 1.9f));
                    if (tempTurn == 0) myTrans.rotation *= Quaternion.Euler(new Vector3(0, -45, 0));
                    else myTrans.rotation *= Quaternion.Euler(new Vector3(0, 45, 0));

                    /*if (hitRight.collider != null && hitLeft.collider == null)
                    {
                            myTrans.rotation *= Quaternion.Euler(new Vector3(0, 45, 0));
                    }
                    else if (hitLeft.collider != null && hitRight.collider == null)
                    {
                            myTrans.rotation *= Quaternion.Euler(new Vector3(0, -45, 0));
                    }
                    else {
                            tempTurn = Mathf.RoundToInt(Random.Range(0,1.9f));
                            if (tempTurn == 0) myTrans.rotation *= Quaternion.Euler(new Vector3(0, -45, 0));
                            else myTrans.rotation *= Quaternion.Euler(new Vector3(0, 45, 0));
                    }*/

                }
                //отрисовка линий для теста
                Debug.DrawRay(transform.position + Vector3.up * 0.5f, transform.forward);
                Debug.DrawRay(transform.position, transform.forward);
                Debug.DrawRay(transform.position, transform.forward * 2 + Vector3.down * 2);
                Debug.DrawRay(transform.position + Vector3.down * 0.5f, transform.right + Vector3.up * 0.5f);
                Debug.DrawRay(transform.position + Vector3.down * 0.5f, -transform.right + Vector3.up * 0.5f);

                if (target != Vector3.zero)
                    anim.Play(a_run.name);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            rRot = Random.Range(0, 270); //рандомный кгол поворота для убегания
            myTrans.rotation *= Quaternion.Euler(new Vector3(0, rRot, 0));
            rayRun = new Ray(myTrans.position, transform.forward * 30);
            target = rayRun.GetPoint(30); //точка в качестве таргета
        }
    }

}

