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
    public Button BTN_Bomb;


    public GameObject Content_difance;
    public GameObject Content_attack;
    public GameObject Content_BTN_attack_defance;

    public int Anim;


    void Start()
    {
        //contol btn ui;
        BTN_Atack.onClick.AddListener(() =>
        {
            Anim = 1;

        });
        BTN_close_attack.onClick.AddListener(() =>
        {
            Anim = 2;
        });

        BTN_Defance.onClick.AddListener(() =>
        {
            Anim = 3;

        });
        BTN_close_Defance.onClick.AddListener(() =>
        {
            Anim = 4;

        });

        //action builds
        BTN_Bomb.onClick.AddListener(() =>
        {
            foreach (var item in GetComponent<Mission_offline>().All_place)
            {
                if (item.GetComponent<Mission_offline.Raw_Place_script>().Setting_place.Type_place == Mission_offline.Raw_Place_script.Type_place.Block)
                {
                    item.GetComponent<Mission_offline.Raw_Place_script>().Anim_boomb = 1;
                }
            }
        });
        
    }

    void Update()
    {
        if (Anim == 1)
        {
            Content_attack.transform.localPosition = Vector3.MoveTowards(Content_attack.transform.localPosition, new Vector3(0, -4, 0), 0.2f);
            Content_BTN_attack_defance.transform.localPosition = Vector3.MoveTowards(Content_BTN_attack_defance.transform.localPosition, new Vector3(0, -6f, 0), 0.2f);
        }
        else if (Anim == 2)
        {
            Content_attack.transform.localPosition = Vector3.MoveTowards(Content_attack.transform.localPosition, new Vector3(0, -6f, 0), 0.2f);
            Content_BTN_attack_defance.transform.localPosition = Vector3.MoveTowards(Content_BTN_attack_defance.transform.localPosition, new Vector3(0, -4.4f, 0), 0.2f);
        }
        else if (Anim == 3)
        {
            Content_difance.transform.localPosition = Vector3.MoveTowards(Content_difance.transform.localPosition, new Vector3(0, -4, 0), 0.2f);
            Content_BTN_attack_defance.transform.localPosition = Vector3.MoveTowards(Content_BTN_attack_defance.transform.localPosition, new Vector3(0, -6f, 0), 0.2f);
        }
        else if (Anim == 4)
        {
            Content_difance.transform.localPosition = Vector3.MoveTowards(Content_difance.transform.localPosition, new Vector3(0, -6f, 0), 0.2f);
            Content_BTN_attack_defance.transform.localPosition = Vector3.MoveTowards(Content_BTN_attack_defance.transform.localPosition, new Vector3(0, -4.4f, 0), 0.2f);
        }

    }
}
