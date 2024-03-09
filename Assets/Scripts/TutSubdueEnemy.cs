using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TutSubdueEnemy : MonoBehaviour
{
    NavMeshAgent nav;
    Animator anim;
    public Player player;
    [SerializeField] bool isPlayerSubdue = false;
    bool isDead = false;

    public float subdueCoolTime;
    float curSubdueCoolTime;

    public float subdueProgress;
    public float curSubdueProgress;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        nav.speed = 10f;
    }

    void Update()
    {
        TargetPlayer();
        PlayerSubdue();
    }

    void TargetPlayer()
    {
        if(!isDead)
            nav.SetDestination(player.transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerSubdue = true;
            player.SubdueObject = transform.gameObject;
        }
    }

    void PlayerSubdue()
    {
        if (isPlayerSubdue)
        {
            UIManager.Instance.SubDueSlider.gameObject.SetActive(true);
            transform.LookAt(player.transform.position);
            player.transform.LookAt(transform.position);

            player.isSubdue = true;
            nav.isStopped = true;
            nav.velocity = Vector3.zero;

            curSubdueCoolTime += Time.deltaTime;
            curSubdueProgress += Time.deltaTime;
            UIManager.Instance.SubDueSlider.value = curSubdueProgress / subdueProgress;

            if (curSubdueCoolTime > subdueCoolTime)
            {
                //���� ���н� (�ð��� �� ����������)

                GetComponent<BoxCollider>().enabled = false;
                curSubdueCoolTime = 0;
                isPlayerSubdue = false;
                player.isSubdue = false;
                UIManager.Instance.SubDueSlider.gameObject.SetActive(false);
                ProgressManager.Instance.curProgress -= 1;
                isDead = true;
                Invoke("Dead", 1);
                //�ִϸ��̼�
            }

            if (curSubdueProgress > subdueProgress)
            {
                //���� ������

                GetComponent<BoxCollider>().enabled = false;
                curSubdueCoolTime = 0;
                player.isSubdue = false;
                isPlayerSubdue = false;
                UIManager.Instance.SubDueSlider.gameObject.SetActive(false);
                ProgressManager.Instance.curProgress -= 1;
                isDead = true;
                Invoke("Dead", 1);
                //�ִϸ��̼�
            }
        }
    }

    void Dead()
    {
        transform.gameObject.SetActive(false);
    }

}
