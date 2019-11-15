using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Step_1 : MonoBehaviour
{
    public Button BTN_English;
    public Button BTN_FA;
    public GameObject Step_2;

    void Start()
    {
        BTN_English.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("Language", 0);
            gameObject.SetActive(false);
            Step_2.SetActive(true);

        });

        BTN_FA.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("Language", 0);
            gameObject.SetActive(false);
            Step_2.SetActive(true);
        });
    }

}
