using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_mission_offline : MonoBehaviour
{
    public Button BTN_Atack;
    public Button BTN_Defance;

    public Button BTN_close_attack;
    public Button BTN_close_Defance;



    public GameObject Content_difance;
    public GameObject Content_attack;
    public GameObject Content_BTN_attack_defance;

    public int Anim_attack;
    public int Anim_defance;

    void Start()
    {
        BTN_Atack.onClick.AddListener(() =>
        {
            Anim_attack = 1;

        });
        BTN_close_attack.onClick.AddListener(() =>
        {
            Anim_attack = 0;
        });

        BTN_Defance.onClick.AddListener(() =>
        {
            Anim_defance = 1;

        });
        BTN_close_Defance.onClick.AddListener(() =>
        {
            Anim_defance = 0;

        });

    }

    void Update()
    {
        if (Anim_attack == 0)
        {
            Content_attack.transform.localPosition = Vector3.MoveTowards(Content_attack.transform.localPosition, new Vector3(0, -6, 0), 0.1f);
            Content_BTN_attack_defance.transform.localPosition = Vector3.MoveTowards(Content_BTN_attack_defance.transform.localPosition, new Vector3(1.8f, 0, 0), 0.1f);
        }
        else
        {
            Content_attack.transform.localPosition = Vector3.MoveTowards(Content_attack.transform.localPosition, new Vector3(0, -4f, 0), 0.1f);
            Content_BTN_attack_defance.transform.localPosition = Vector3.MoveTowards(Content_BTN_attack_defance.transform.localPosition, new Vector3(0, -0.6f, 0), 0.1f);
        }

    }
}
