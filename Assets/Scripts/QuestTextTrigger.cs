using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTextTrigger : MonoBehaviour
{
    public string questText;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            UIManager.Instance.questUIAnim.SetTrigger("QuestUpdate");
            UIManager.Instance.questText.text = questText;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.questUIAnim.SetTrigger("QuestOut");
            //UIManager.Instance.questText.text = questText;
        }
    }
}
