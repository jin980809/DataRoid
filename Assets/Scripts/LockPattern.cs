using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPattern : PassWord
{
    private Dictionary <int, CircleIdentifier> circles;
    public GameObject linePrefab;
    public Canvas canvas;
    public string passWord;
    private string inputPassWord = "";
    private List<CircleIdentifier> lines;

    private GameObject lineOnEdit;
    private RectTransform lineOnEditRcTs;
    private CircleIdentifier circleOnEdit;

    private bool unLocking;
    bool qDown;

    void Start()
    {
        circles = new Dictionary<int, CircleIdentifier>();
        lines = new List<CircleIdentifier>();


        for(int i = 0; i < transform.childCount; i++)
        {
            Transform circle = transform.GetChild(i);
            CircleIdentifier identifier = circle.GetComponent<CircleIdentifier>();
            identifier.id = i;
            circles.Add(i, identifier);
        }
    }

    
    void Update()
    {
        qDown = Input.GetButtonDown("Cancel");

        if (unLocking)
        {
            Vector3 mousePos = canvas.transform.InverseTransformPoint(Input.mousePosition);

            lineOnEditRcTs.sizeDelta = new Vector2(lineOnEditRcTs.sizeDelta.x, Vector2.Distance(mousePos, circleOnEdit.transform.localPosition));
            lineOnEditRcTs.rotation = Quaternion.FromToRotation(
                Vector3.up,
                (mousePos - circleOnEdit.transform.localPosition).normalized
                );
        }

        if (qDown)
        {
            ExitPassWord();
        }
    }

    GameObject CreateLine(Vector3 pos, int id)
    {
        GameObject line = Instantiate(linePrefab, canvas.transform);
        line.transform.localPosition = pos;

        CircleIdentifier lineIdf = line.AddComponent<CircleIdentifier>();

        lineIdf.id = id;
        lines.Add(lineIdf);

        return line;
    }

    void TrySetLineEdit(CircleIdentifier circle)
    {
        foreach(CircleIdentifier line in lines)
        {
            if(line.id == circle.id)
            {
                return;
            }
        }

        lineOnEdit = CreateLine(circle.transform.localPosition, circle.id);
        lineOnEditRcTs = lineOnEdit.GetComponent<RectTransform>();
        circleOnEdit = circle;
        inputPassWord += circle.id.ToString();
    }

    public void OnMouseEnterCircle(CircleIdentifier idf)
    {
        if(unLocking)
        {
            lineOnEditRcTs.sizeDelta = new Vector2(lineOnEditRcTs.sizeDelta.x, Vector3.Distance(circleOnEdit.transform.localPosition, idf.transform.localPosition));
            lineOnEditRcTs.rotation = Quaternion.FromToRotation(
                Vector3.up,
                (idf.transform.localPosition - circleOnEdit.transform.localPosition).normalized
                );

            TrySetLineEdit(idf);
        }
    }

    public void OnMouseExitCircle(CircleIdentifier idf)
    {
    }

    public void OnMouseDownCircle(CircleIdentifier idf)
    {
        unLocking = true;

        TrySetLineEdit(idf);
    }

    public void OnMouseUpCircle(CircleIdentifier idf)
    {
        unLocking = false;

        foreach(CircleIdentifier line in lines)
        {
            Destroy(line.gameObject);
        }

        if(string.Equals(passWord, inputPassWord))
        {
            Debug.Log("unlock");
            isDone = true;
            PassWordResult();
            ExitPassWord();
        }
        else
        {
            Debug.Log("false " + inputPassWord);
            inputPassWord = "";
            lines.Clear();

            lineOnEdit = null;
            lineOnEditRcTs = null;
            circleOnEdit = null;
        }
    }

}
