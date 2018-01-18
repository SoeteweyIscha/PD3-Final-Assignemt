using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParsingScript : MonoBehaviour {

    public int Width;
    public int Height;
    public int RocksVariable;


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
