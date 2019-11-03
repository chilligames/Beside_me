using UnityEngine;
using UnityEngine.UI;
using static Mission_offline;

public class UI_mission_offline : MonoBehaviour
{
    public Button BTN_Atack;
    public Button BTN_Defance;

    public Button BTN_close_attack;
    public Button BTN_close_Defance;


    public Button BTN_Bomb;
    public Button BTN_turret;
    public Button BTN_Castel;



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
            All_animaation_stop();
        });

        BTN_Defance.onClick.AddListener(() =>
        {
            Anim = 3;

        });
        BTN_close_Defance.onClick.AddListener(() =>
        {
            Anim = 4;
            All_animaation_stop();
        });

        //action builds
        BTN_Bomb.onClick.AddListener(() =>
        {
            foreach (var item in GetComponentInParent<Mission_offline>().All_place)
            {
                if (item.GetComponent<Place>().Setting_place.Type_place == Type_place.Block)
                {
                    item.GetComponent<Place>().Anim_boomb = 1;
                }
            }
        });

        BTN_turret.onClick.AddListener(() =>
        {
            foreach (var item in GetComponentInParent<Mission_offline>().All_place)
            {
                if (item.GetComponent<Place>().Setting_place.Type_place == Type_place.Place && item.GetComponent<Place>().Setting_place.Place_for != Place_for.Enemy)
                {
                    item.GetComponent<Place>().Anim_builds = 1;
                    item.GetComponent<Place>().Type_Build = Type_Build.Turret;
                }
            }

        });

        BTN_Castel.onClick.AddListener(() =>
        {
            foreach (var item in GetComponentInParent<Mission_offline>().All_place)
            {
                if (item.GetComponent<Place>().Setting_place.Type_place == Type_place.Place && item.GetComponent<Place>().Setting_place.Place_for == Place_for.Empity)
                {
                    item.GetComponent<Place>().Anim_builds = 1;
                    item.GetComponent<Place>().Type_Build = Type_Build.Castel;
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

    void All_animaation_stop()
    {
        foreach (var item in GetComponentInParent<Mission_offline>().All_place)
        {
            item.GetComponent<Place>().Anim_boomb = 3;
            item.GetComponent<Place>().Anim_builds = 3;
            item.GetComponent<Place>().Type_Build = Type_Build.Null;
        }
    }
}
