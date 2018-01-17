using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using views;
using models;

public class Application : MonoBehaviour {


    // Width
    [SerializeField]
    private GameObject _xSizeView;
    private SizeModel _xSizeModel;
    private SizeController _xSizeController;

    // Height
    [SerializeField]
    private GameObject _ySizeView;
    private SizeModel _ySizeModel;
    private SizeController _ySizeController;

	// Use this for initialization
	void Start ()
    {
        // Width
        _xSizeModel = new SizeModel();
        _xSizeModel.Axis = "Width";
        _xSizeModel.SizeValue = 10;

        _xSizeController = new SizeController();
        _xSizeController.Model = _xSizeModel;
        _xSizeController.View = _xSizeView.GetComponent<ISizeView>();

        //Height
        _ySizeModel = new SizeModel();
        _ySizeModel.Axis = "Height";
        _ySizeModel.SizeValue = 10;

        _ySizeController = new SizeController();
        _ySizeController.Model = _ySizeModel;
        _ySizeController.View = _ySizeView.GetComponent<ISizeView>();

        _xSizeController.Init();
        _ySizeController.Init();
	}
}
