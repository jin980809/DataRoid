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

    public float subdueProgress;
    public float curSubdueProgress;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        nav.speed = 10f;
        curSubdueProgress = subdueProgress;
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
            anim.SetTrigger("doSubdue");
            isPlayerSubdue = true;
            player.SubdueObject = transform.gameObject;
        }
    }

    void PlayerSubdue()
    {
        if (isPlayerSubdue)
        {
            //UIManager.Instance.SubDueSlider.gameObject.SetActive(true);
            transform.LookAt(player.transform.position);
            player.transform.LookAt(transform.position);

            player.isSubdue = true;
            nav.isStopped = true;
            nav.velocity = Vector3.zero;

            UIManager.Instance.SubDueSlider.value = curSubdueProgress / subdueProgress;

            if (curSubdueProgress <= 0)
            {
                //제압 성공시
                GetComponent<BoxCollider>().enabled = false;
                //curSubdueCoolTime = 0;
                isPlayerSubdue = false;
                UIManager.Instance.SubDueSlider.gameObject.SetActive(false);
                ProgressManager.Instance.curData -= 1;
                isDead = true;
                Invoke("Dead", 1);
                anim.SetTrigger("doSubdueOut");
                //애니메이션
            }
        }
    }

    void Dead()
    {
        transform.gameObject.SetActive(false);
        player.isSubdue = false;
    }

}
