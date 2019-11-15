using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Step_6 : MonoBehaviour
{
    public Button BTN_finish;
    public GameObject Menu;
    public GameObject help;


    void Start()
    {
        BTN_finish.onClick.AddListener(() =>
        {
            Menu.SetActive(true);
            help.SetActive(false);
            PlayerPrefs.SetInt("Language", 1);
            gameObject.SetActive(false);
        });
    }

}
