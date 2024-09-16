using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DotConnectManager : PassWord
{
    private Dictionary<int, Dot> circles;
    public GameObject linePrefab;
    public Canvas canvas;
    public int colorPairAmount;
    public int dotAmount;

    private bool[] isColorConnect;
    private int[] dupLineCheck;
    private int firstColorIndex;
    private List<Dot> lines;
    private List<Dot> tempLines;
    private List<Dot> dupLines;

    private int linesLastIndex = 0;

    private GameObject lineOnEdit;
    private RectTransform lineOnEditRcTs;
    private Dot circleOnEdit;

    private bool unLocking;

    private Dot lastDot;

    private Dot preDot;
    bool qDown;

    void Start()
    {
        circles = new Dictionary<int, Dot>();
        lines = new List<Dot>();
        tempLines = new List<Dot>();
        dupLines = new List<Dot>();
        isColorConnect = new bool[colorPairAmount + 1];
        dupLineCheck = new int[dotAmount];

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Transform circle = transform.GetChild(i);
            Dot identifier = circle.GetComponent<Dot>();
            identifier.id = i;
            circles.Add(i, identifier);
        }
    }


    void Update()
    {


        //qDown = Input.GetButtonDown("Cancel");

        /*if (unLocking && !lastDot.isConnect)
        {
            Vector3 mousePos = canvas.transform.InverseTransformPoint(Input.mousePosition);

            lineOnEditRcTs.sizeDelta = new Vector2(lineOnEditRcTs.sizeDelta.x, Vector2.Distance(mousePos, circleOnEdit.transform.localPosition));
            lineOnEditRcTs.rotation = Quaternion.FromToRotation(
                Vector3.up,
                (mousePos - circleOnEdit.transform.localPosition).normalized
                );
        }*/

        //if (qDown)
        //{
        //    ExitPassWord();
        //}
    }

    GameObject CreateLine(Vector3 pos, int id)
    {
        GameObject line = Instantiate(linePrefab, canvas.transform);

        line.transform.localPosition = pos;

        Dot lineIdf = line.AddComponent<Dot>();

        lineIdf.id = id;
        lines.Add(lineIdf);
        dupLines.Add(lineIdf);
        return line;
    }

    void TrySetLineEdit(Dot circle, int colorIndex)
    {
        foreach (Dot line in dupLines)
        {
            if (line.id == circle.id || (Mathf.Abs(preDot.id - circle.id) != 5 && Mathf.Abs(preDot.id - circle.id) != 1))
            { 
                return;
            }
        }

        //Debug.Log(preDot.id + " to " + circle.id);

        lastDot = circle;

        lineOnEdit = CreateLine(circle.transform.localPosition, circle.id);

        switch (colorIndex)
        {
            case 0:
                break;
            case 1:
                lineOnEdit.GetComponent<Image>().color = Color.red;
                break;
            case 2:
                lineOnEdit.GetComponent<Image>().color = Color.green;
                break;
            case 3:
                lineOnEdit.GetComponent<Image>().color = Color.blue;
                break;
        }

        lineOnEditRcTs = lineOnEdit.GetComponent<RectTransform>();
        circleOnEdit = circle;
        preDot = circle;
        //Debug.Log(lastDot.id);
    }

    public void OnMouseEnterCircle(Dot idf)
    {
        //Debug.Log(idf.id);
       
        if (unLocking /*&& distance <= 100*/ && (Mathf.Abs(preDot.id - idf.id) == 5 || Mathf.Abs(preDot.id - idf.id) == 1))
        {
            if (preDot.id % 5 == 0 && preDot.id - idf.id == 1)
                return;            

            if (preDot.id % 5 == 4 && preDot.id - idf.id == -1)
                return;

            lineOnEditRcTs.sizeDelta = new Vector2(lineOnEditRcTs.sizeDelta.x, Vector3.Distance(circleOnEdit.transform.localPosition, idf.transform.localPosition));
            lineOnEditRcTs.rotation = Quaternion.FromToRotation(
                Vector3.up,
                (idf.transform.localPosition - circleOnEdit.transform.localPosition).normalized
                );

            TrySetLineEdit(idf, firstColorIndex);
        }
    }

    public void OnMouseExitCircle(Dot idf)
    {

    }

    public void OnMouseDownCircle(Dot idf)
    {
        preDot = idf;
        unLocking = true;
        firstColorIndex = idf.colorIndex;
        TrySetLineEdit(idf, idf.colorIndex);
    }

    public void OnMouseUpCircle(Dot idf)
    {
        if ((idf.colorIndex != lastDot.colorIndex || !lastDot.isNode || lines.Count - linesLastIndex == 1) && unLocking)
        {
            //Debug.Log("Not Connect " + linesLastIndex);

            for (int i = 0; i < linesLastIndex; i++)
            {
                tempLines.Add(lines[i]);
            }
            for(int i = linesLastIndex; i < lines.Count; i++)
            {
                Destroy(lines[i].gameObject);
            }

            lines.Clear();
            for(int i = 0; i < tempLines.Count; i++)
            {
                lines.Add(tempLines[i]);
            }
            
            tempLines.Clear();
            /*lineOnEdit = null;
            lineOnEditRcTs = null;
            circleOnEdit = null;*/
        }
        else
        {
            Debug.Log("Connect");

            //dupLineCheck = { 0 };

            linesLastIndex = lines.Count;

            for (int i = 0; i < dotAmount; i++)
            {
                dupLineCheck[i] = 0;
            }

            for (int i = 0; i < linesLastIndex; i++)
            {
                dupLineCheck[lines[i].id]++;
            }

            isColorConnect[idf.colorIndex] = true;

            //lines.Clear();

            //lineOnEdit = null;
            //lineOnEditRcTs = null;
            //circleOnEdit = null;
        }

        dupLines.Clear();
        firstColorIndex = 0;

        //if (string.Equals(passWord, inputPassWord))
        //{
        //    Debug.Log("unlock");
        //    isDone = true;
        //    PassWordResult();
        //    NameTagOnOff();
        //    ExitPassWord();
        //}
        //else
        //{
        //    Debug.Log("false " + inputPassWord);
        //    inputPassWord = "";
        //    lines.Clear();

        //    lineOnEdit = null;
        //    lineOnEditRcTs = null;
        //    circleOnEdit = null;
        //}

        unLocking = false;

        for(int i = 1; i < colorPairAmount + 1; i++)
        {
            if (!isColorConnect[i])
                return;
        }
        //중복체크
        for(int i = 0; i < dotAmount; i++)
        {
            if (dupLineCheck[i] >= 2)
                return;
        }


        Debug.Log("unlock");

        for (int i = 0; i < lines.Count; i++)
        {
            Destroy(lines[i].gameObject);
        }

        isDone = true;
        PassWordResult();
        NameTagOnOff();
        ExitPassWord();
    }

    public void Reset()
    {
        for(int i = 0; i < lines.Count; i++)
        {
            Destroy(lines[i].gameObject);
        }

        lines.Clear();
        tempLines.Clear();
        dupLines.Clear();

        for (int i = 0; i < dotAmount; i++)
        {
            dupLineCheck[i] = 0;
        }

        linesLastIndex = 0;

        lineOnEdit = null;
        lineOnEditRcTs = null;
        circleOnEdit = null;

        lastDot = null;

        for (int i = 1; i < colorPairAmount + 1; i++)
        {
            isColorConnect[i] = false;
        }
    }

}
