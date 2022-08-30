using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//接收所有其他通報的實例
public class GameManagerSingleton
{
    private GameObject gameObject;

    //主要通報對象
    private static GameManagerSingleton main_Instance;
    //接收其他通報的接口
    public static GameManagerSingleton Instance
    {
        get
        {
            //如果今天沒有實例存在，產生一個物件並擁有GameManagerSingleton()的功能
            if(main_Instance == null)
            {
                main_Instance = new GameManagerSingleton();
                main_Instance.gameObject = new GameObject("GameManager");
                //在產生物件上新增InputController的腳本
                main_Instance.gameObject.AddComponent<InputController>();
            }
            //如果已經擁有實例，直接回傳即可（不執行上述功能）
            return main_Instance;
        }
    }


    //登記其他通報
    private InputController main_InputController;
    public InputController InputController
    {
        get
        {
            if(main_InputController == null)
            {
                main_InputController = gameObject.GetComponent<InputController>();
            }
            return main_InputController;
        }
    }
}
