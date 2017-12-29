using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{

    [SerializeField]
    private GameObject _tile;
    [SerializeField]
    private int _xSize = 5;
    [SerializeField]
    private int _zSize = 5;
    [SerializeField]
    private float _xOffSet = 2f;
    [SerializeField]
    private float _zOffSet = 1.7f;

    private float _halfOffset;


    private void Start()
    {
        _halfOffset = _xOffSet / 2;
        GenerateMaze();
    }
    private void GenerateMaze()
    {
        List<GameObject> Hexagons = new List<GameObject>();
        GameObject hexGrid = new GameObject("HexGrid");
        hexGrid.transform.position = Vector3.zero;
        hexGrid.transform.rotation = Quaternion.identity;
        for (int z = 0; z < _zSize; z++)
        {
            for (int x = 0; x < _xSize; x++)
            {
                float xPos = x * _xOffSet + (z % 2) * _halfOffset;
                float zPos = z * _zOffSet;
                Vector3 currentPos = new Vector3(xPos, 0, zPos);

                GameObject hex = Instantiate(
                    _tile, currentPos, Quaternion.identity, hexGrid.transform);
                hex.GetComponent<HexScript>().Row = z;
                hex.GetComponent<HexScript>().Column = x;
                Hexagons.Add(hex);
            }
        }

    }

}
