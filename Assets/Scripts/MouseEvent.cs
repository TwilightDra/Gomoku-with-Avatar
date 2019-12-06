using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEvent : MonoBehaviour
{
    //GameManagement gm=new GameManagement();
    private void OnMouseEnter()
    {
        Color color = this.gameObject.GetComponent<Renderer>().material.color;
        color.a = 0.5f;
        this.gameObject.GetComponent<Renderer>().material.color = color;
    }
    private void OnMouseExit()
    {
        Color color = this.gameObject.GetComponent<Renderer>().material.color;
        color.a = 0f;
        this.gameObject.GetComponent<Renderer>().material.color = color;
    }
    void OnMouseDown()
    {
        GameObject.Find("GameManager").GetComponent<GameManagement>().DestroyHint();
        //movePos;
        GameObject.Find("P1").GetComponent<Drive>().target = this.gameObject.GetComponent<Transform>();
        GameObject.Find("P1").GetComponent<Drive>().StartPathFind();

        GameObject.Find("GameManager").GetComponent<GameManagement>().SetPiece(this.gameObject.GetComponent<Transform>().position);
        //Debug.Log(this.gameObject.GetComponent<Transform>().position);
        GameObject.Find("A*").GetComponent<MyGrid>().UpdateGrid();
    }
}
