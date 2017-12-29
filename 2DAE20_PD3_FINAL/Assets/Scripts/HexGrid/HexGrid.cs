using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexScript : MonoBehaviour
{

    [SerializeField]
    private int _column;
    [SerializeField]
    private int _row;


    public int Row
    {
        get { return _row; }
        set { _row = value; }
    }

    public int Column
    {
        get { return _column; }
        set { _column = value; }
    }

}
