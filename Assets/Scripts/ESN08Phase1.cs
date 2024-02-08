using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ESN08Phase1 : MonoBehaviour
{
    public float subdueCoolTime;
    float curSubdueCoolTime;
    [SerializeField] bool isPlayerSubdue = false;

    public float subdueProgress;
    public float curSubdueProgress;
    NavMeshAgent nav;
    Player player;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
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
            player.phase1ESN08 = GetComponent<ESN08Phase1>();
            isPlayerSubdue = true;   
        }
    }

    void Subdue()
    {
        if (isPlayerSubdue)
        {
            //�÷��̾� ī�޶� ���� �� �̵� ����
            UIManager.Instance.SubDueSlider.gameObject.SetActive(true);
            transform.LookAt(player.transform.position);
            player.isSubdue = true;
            nav.isStopped = true;
            nav.velocity = Vector3.zero;

            curSubdueCoolTime += Time.deltaTime;
            curSubdueProgress += Time.deltaTime;
            UIManager.Instance.SubDueSlider.value = curSubdueProgress / subdueProgress;

            if (curSubdueCoolTime > subdueCoolTime)
            {
                //���� ���н� (�ð��� �� ����������)
                GetComponentInChildren<BoxCollider>().enabled = false;
                curSubdueCoolTime = 0;
                nav.isStopped = false;
                isPlayerSubdue = false;
                player.isSubdue = false;
                UIManager.Instance.SubDueSlider.gameObject.SetActive(false);
            }

            if(curSubdueProgress > subdueProgress)
            {
                //���� ������
                GameManager.Instance.ESN08Phase2Diff++;
                GetComponentInChildren<BoxCollider>().enabled = false;
                player.isSubdue = false;
                isPlayerSubdue = false;
                nav.isStopped = false;
                UIManager.Instance.SubDueSlider.gameObject.SetActive(false);
            }
        }
    }

    void OnFootstep()
    {

    }
}
