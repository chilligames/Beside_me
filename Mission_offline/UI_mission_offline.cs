using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_mission_offline : MonoBehaviour
{

    public Button BTN_bomb;
    public Button BTN_Turret;

    public Button BTN_close_menu;
    public GameObject Content_build;

    public int Anim_build;

    void Start()
    {
        BTN_close_menu.onClick.AddListener(() =>
        {
            Anim_build = 0;
        });

        //builder action
        BTN_bomb.onClick.AddListener(() =>
        {
            foreach (var item in GetComponent<Mission_offline>().Places)
            {
                item.GetComponent<Mission_offline.Raw_Place_script>().Anim_builds = 3;
            }
            foreach (var item in GetComponent<Mission_offline>().Place_blocks)
            {

                if (item.GetComponent<Mission_offline.Raw_Place_script>().Setting_place.Type_place == Mission_offline.Raw_Place_script.Type_place.Block)
                {
                    item.GetComponent<Mission_offline.Raw_Place_script>().Anim_boomb = 1;
                }
            }
        });

        BTN_Turret.onClick.AddListener(() =>
        {
            foreach (var item in GetComponent<Mission_offline>().Places)
            {
                item.GetComponent<Mission_offline.Raw_Place_script>().Anim_boomb = 3;

            }

            foreach (var item in GetComponent<Mission_offline>().Places)
            {
                if (item.GetComponent<Mission_offline.Raw_Place_script>().Setting_place.Type_place == Mission_offline.Raw_Place_script.Type_place.Place)
                {
                    item.GetComponent<Mission_offline.Raw_Place_script>().Anim_builds = 1;
                    item.GetComponent<Mission_offline.Raw_Place_script>().Type_Build = Mission_offline.Type_Build.Turret;
                }
            }


        });

    }

    void Update()
    {
        if (Anim_build == 0)
        {
            Content_build.transform.localPosition = Vector3.MoveTowards(Content_build.transform.localPosition, new Vector3(0, -6, 0), 0.1f);
        }
        else if (Anim_build == 1)
        {
            Content_build.transform.localPosition = Vector3.MoveTowards(Content_build.transform.localPosition, new Vector3(0, -4, 0), 0.1f);
        }

    }
}
