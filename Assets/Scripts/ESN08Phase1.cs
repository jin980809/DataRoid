using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ESN08Phase1 : MonoBehaviour
{
    [SerializeField] bool isPlayerSubdue = false;
    Animator anim;
    public CapsuleCollider attack;
    public float subdueProgress;
    public float curSubdueProgress;
    NavMeshAgent nav;
    Player player;

    void Start()
    {
        curSubdueProgress = subdueProgress;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Subdue();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
            player.SubdueObject = transform.gameObject;
            isPlayerSubdue = true;   
        }
    }

    void Subdue()
    {
        if (isPlayerSubdue)
        {
            anim.SetTrigger("doSubdue");
            //�÷��̾� ī�޶� ���� �� �̵� ����
            UIManager.Instance.SubDueSlider.gameObject.SetActive(true);
            attack.enabled = false;
            transform.LookAt(player.transform.position);
            player.transform.LookAt(transform.position);
            player.isSubdue = true;
            nav.isStopped = true;
            nav.velocity = Vector3.zero;

            UIManager.Instance.SubDueSlider.value = curSubdueProgress / subdueProgress;

            //if (curSubdueCoolTime > subdueCoolTime)
            //{
            //    //���� ���н� (�ð��� �� ����������)
                
            //    //curSubdueCoolTime = 0;
            //    nav.isStopped = false;
            //    UIManager.Instance.SubDueSlider.gameObject.SetActive(false);
            //    anim.SetTrigger("doSubdueOut");
            //    Invoke("SubdueOut", 0.3f);
            //}

            if(curSubdueProgress <= 0)
            {
                //���� ������
                GameManager.Instance.ESN08Phase2Diff++;
                nav.isStopped = false;
                UIManager.Instance.SubDueSlider.gameObject.SetActive(false);
                anim.SetTrigger("doSubdueOut");
                Invoke("SubdueOut", 0.3f);
            }
        }
    }

    void SubdueOut()
    {
        isPlayerSubdue = false;
        player.isSubdue = false;
    }

    void OnFootstep()
    {

    }
}
