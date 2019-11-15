using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help : MonoBehaviour
{
    public GameObject Menu_panel;

    void Start()
    {
        if (PlayerPrefs.GetInt("Help") == 1)
        {
            Menu_panel.SetActive(true);
            gameObject.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("Help") == 0)
        {
            Menu_panel.SetActive(false);
            gameObject.SetActive(true);
        }

    }

}
