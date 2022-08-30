using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSingleton
{
    private GameObject gameObject;

    private static GameManagerSingleton main_Instance;
    public static GameManagerSingleton Instance
    {
        get
        {
            if(main_Instance == null)
            {
                main_Instance = new GameManagerSingleton();
                main_Instance.gameObject = new GameObject("GameManager");
                main_Instance.gameObject.AddComponent<InputController>();
            }
            return main_Instance;
        }
    }


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
