using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject Mission_offline;

    public Button BTN_singel_play;

    void Start()
    {
        BTN_singel_play.onClick.AddListener(() =>
        {
            Instantiate(Mission_offline);
            gameObject.SetActive(false);

        });

    }

}
