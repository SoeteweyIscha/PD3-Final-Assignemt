using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using views;
using models;

public class Application : MonoBehaviour {

    [SerializeField]
    private GameObject xSizeView;
    private SizeModel xSizeModel;
    private SizeController xSizeController;

	// Use this for initialization
	void Start () {
        xSizeModel = new SizeModel();
        xSizeModel.Axis = "Width";
        xSizeModel.SizeValue = 10;

        xSizeController = new SizeController();
        xSizeController.Model = xSizeModel;
        xSizeController.View = xSizeView.GetComponent<ISizeView>();

        xSizeController.Init(); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
