using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    void OnDestroy()
    {
        if (this == _instance)
            _instance = null;
    }
    void Awake()
    {

        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    public void SlashButtonAction() => EventManager.SlashButton();
}