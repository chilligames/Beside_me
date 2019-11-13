using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject Mission_offline;

    public Button BTN_singel_play;
    int Anim;

    public Vector3 Distiny_postion_object;

    void Start()
    {
        BTN_singel_play.onClick.AddListener(() =>
        {
            Distiny_postion_object = new Vector3(Random.Range(10, -10), Random.Range(-10, 10), 0);
            Instantiate(Mission_offline);
            Anim = 1;
        });
    }
    public void Update()
    {

        if (Anim == 1)
        {
           
            foreach (var item in GetComponentsInChildren<Transform>())
            {
                if (item.transform.localScale != Vector3.zero)
                {
                    item.transform.localScale = Vector3.MoveTowards(item.transform.localScale, Vector3.zero, 0.1f);
                }
            }
        }
        else if (Anim == 2)
        {
        }

    }

}

