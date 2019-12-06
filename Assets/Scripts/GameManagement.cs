using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagement : MonoBehaviour
{

    public bool isBlack;

    public GameObject Selections;
    public GameObject BlackPiece, WhitePiece, HintObj;
    public Image Iblack, Iwhite;
    public Text IP1, IP2;
    private GameObject playPiece,hintObj;
    private GameObject[,] boardData = new GameObject[15, 15];
    private int Offset = 7, WhoWin=0;
    private bool firstMove=true, Finished = false;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = -7; i <= 7; i++)
        {
            for (int j = 7; j >= -7; j--)
            {
                GameObject selectable = Instantiate(Selections, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                selectable.transform.parent = GameObject.Find("Selectable").transform;
            }
        }
    }
    private void Start()
    {
        isBlack = true;
        Color tempcolor = Iwhite.color;
        tempcolor.a = 0.3f;
        Iwhite.color = tempcolor;
        GoAI.board = new int[15, 15];
    }

    private void Update()
    {
        if (!Finished)
        {
            if (isBlack)
                AIAction();
        }
        else
        {
            if (WhoWin == 2)
            {
                IP2.color = Color.white;
                Color tempcolor = Iblack.color;
                tempcolor.a = 0.3f;
                Iblack.color = tempcolor;
            }
            else if (WhoWin == 1)
            {
                IP1.color = Color.white;
                Color tempcolor = Iwhite.color;
                tempcolor.a = 0.3f;
                Iwhite.color = tempcolor;
            }
        }
    }

    public void SetPiece(Vector3 Pos)
    {
        if (isBlack)  //AI
        {
            playPiece = Instantiate(BlackPiece, Pos, Quaternion.identity) as GameObject;
            hintObj= Instantiate(HintObj, Pos, Quaternion.identity) as GameObject;
            playPiece.transform.parent = GameObject.Find("PlayPiece").transform;
            Color color = playPiece.GetComponent<Renderer>().material.color;
            color.a = 0f;
            playPiece.GetComponent<Renderer>().material.color = color;

            /*GoAI.board[(int)playPiece.transform.position.x-Offset, (int)playPiece.transform.position.z-Offset] = 1;
            Finished = GoAI.CheckWinner((int)playPiece.transform.position.x - Offset, (int)playPiece.transform.position.z - Offset, 1);
            Debug.Log(Finished);*/
            //isBlack = false;
        }
        else
        {
            Debug.Log(Pos);

            playPiece = Instantiate(WhitePiece, Pos, Quaternion.identity) as GameObject;
            hintObj = Instantiate(HintObj, Pos, Quaternion.identity) as GameObject;
            playPiece.transform.parent = GameObject.Find("PlayPiece").transform;
            Color color = playPiece.GetComponent<Renderer>().material.color;
            color.a = 0f;
            playPiece.GetComponent<Renderer>().material.color = color;

            GoAI.board[(int)playPiece.transform.position.x + Offset, (int)playPiece.transform.position.z + Offset] = 1;
            Finished = GoAI.CheckWinner((int)playPiece.transform.position.x + Offset, (int)playPiece.transform.position.z + Offset, 1);
            if (Finished) WhoWin = 2;
            Debug.Log(Finished);
            //isBlack = true;
        }
    }

    private void AIAction()
    {
        if (firstMove)
        {
            firstMove = false;
            SetPiece(new Vector3(7-Offset, 0, 7-Offset));
            GoAI.board[7, 7] = 2;
            Finished = GoAI.CheckWinner(7, 7, 2);
            Debug.Log(Finished);
            DispPiece();
            return;
        }
        DestroyHint();

        int[] maxPoint = GoAI.NextMove(2);
        Debug.Log("Black:"+maxPoint[0]+" "+ maxPoint[1]+" "+ maxPoint[2]);
        SetPiece(new Vector3(maxPoint[0] - Offset, 0, maxPoint[1] - Offset));
        GoAI.board[maxPoint[0], maxPoint[1]] = 2;
        Finished = GoAI.CheckWinner(maxPoint[0], maxPoint[1], 2);
        if (Finished) WhoWin = 1;
        Debug.Log(Finished);
        DispPiece();
    }

    public void DispPiece()
    {
        Color color = playPiece.GetComponent<Renderer>().material.color;
        color.a = 1f;
        playPiece.GetComponent<Renderer>().material.color = color;
        switchPlayer();
    }

    public void DestroyHint()
    {
        if (hintObj = GameObject.FindGameObjectWithTag("hint"))
            Destroy(hintObj);
    }

    void switchPlayer()
    {
        if (isBlack)
        {
            Color tempcolor = Iblack.color;
            tempcolor.a = 0.3f;
            Iblack.color = tempcolor;

            Color tempcolor2 = Iwhite.color;
            tempcolor2.a = 1f;
            Iwhite.color = tempcolor2;
            isBlack = false;
        }
        else {
            Color tempcolor = Iwhite.color;
            tempcolor.a = 0.3f;
            Iwhite.color = tempcolor;

            Color tempcolor2 = Iblack.color;
            tempcolor2.a = 1f;
            Iblack.color = tempcolor2;

            isBlack = true;
        }
    }


}
