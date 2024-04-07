using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class DroneTextTrigger : MonoBehaviour
{
    [Serializable]
    public struct TextBox
    {
        [TextArea]
        public string droneText;
        public float duration;
    }


    public TextBox[] textBox;
    public int curTextCount = 0;
    public int ID;
    public bool isClick;
    private Player player;
    bool isCommunicate = false;
    bool isNextReady = false;
    bool fDown; // АјАн
    BoxCollider boxCollider;

    void Start()
    {
        if(TextManager.Instance.TextObjects[ID])
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }

        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        fDown = Input.GetButtonDown("Fire1");

        if(fDown && isCommunicate && isClick && isNextReady)
        {
            isNextReady = false;
            StartCoroutine(TextBoxOut(textBox[curTextCount].duration));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(StartTextBox(textBox[curTextCount].duration));
            GetComponent<BoxCollider>().enabled = false;
            TextManager.Instance.TextObjects[ID] = false;
            if (isClick)
            {
                player = other.GetComponent<Player>();
                player.isCommunicate = true;
                isCommunicate = true;
            }
        }
    }

    IEnumerator StartTextBox(float durationTime)
    {
        UIManager.Instance.uiAnim.SetTrigger("Text_Open");
        UIManager.Instance.DroneText.text = textBox[curTextCount].droneText;

        if (!isClick)
        {
            yield return new WaitForSeconds(durationTime);
            StartCoroutine(TextBoxOut(textBox[curTextCount].duration));
        }
        isNextReady = true;
        yield return null;
    }
    IEnumerator TextBoxOut(float durationTime)
    {
        UIManager.Instance.uiAnim.SetTrigger("Text_Out");

        yield return new WaitForSeconds(1f);
        if(curTextCount < textBox.Length - 1)
        {
            curTextCount += 1;

            StartCoroutine(StartTextBox(durationTime));
        }
        else
        {
            if (isClick)
            {
                player.isCommunicate = false;
                isCommunicate = false;
                isNextReady = false;
            }
        }
        yield return null;
    }
}
