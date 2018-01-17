using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    //Rocks
    [SerializeField]
    private GameObject _rocksVarView;
    private SizeModel _rocksVarModel;
    private SizeController _rocksVarController;

    private GameObject _dataCapsule;

	// Use this for initialization
	void Start ()
    {
        _dataCapsule = Resources.Load<GameObject>("DataCapsule");

        // Width
        _xSizeModel = new SizeModel();
        _xSizeModel.Axis = "Width";
        _xSizeModel.SizeValue = 10;

        _xSizeController = new SizeController();
        _xSizeController.Model = _xSizeModel;
        _xSizeController.View = _xSizeView.GetComponent<ISizeView>();

        // Height
        _ySizeModel = new SizeModel();
        _ySizeModel.Axis = "Height";
        _ySizeModel.SizeValue = 10;

        _ySizeController = new SizeController();
        _ySizeController.Model = _ySizeModel;
        _ySizeController.View = _ySizeView.GetComponent<ISizeView>();

        // Rocks
        _rocksVarModel = new SizeModel();
        _rocksVarModel.Axis = "Rocks";
        _rocksVarModel.SizeValue = 30;

        _rocksVarController = new SizeController();
        _rocksVarController.Model = _rocksVarModel;
        _rocksVarController.View = _rocksVarView.GetComponent<ISizeView>();

        _xSizeController.Init();
        _ySizeController.Init();
        _rocksVarController.Init();
	}

    public void LoadScene()
    {
        GameObject data = GameObject.Instantiate(_dataCapsule);
        ParsingScript script = data.GetComponent<ParsingScript>();

        //Give variables
        script.Width = _xSizeModel.SizeValue;
        script.Height = _ySizeModel.SizeValue;
        script.RocksVariable = _rocksVarModel.SizeValue;
        SceneManager.LoadScene("GameScene");
    }
}
