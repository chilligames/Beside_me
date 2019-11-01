using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_mission_offline : MonoBehaviour
{

    public Button BTN_bomb;
    public GameObject Content_build;

    public int Anim_build;

    void Start()
    {
        BTN_bomb.onClick.AddListener(() =>
        {
            print("Active BOmb");
            foreach (var item in GetComponent<Mission_offline>().Place_blocks)
            {
                if (item.GetComponent<Mission_offline.Raw_Place_script>().Type == Mission_offline.Raw_Place_script.Type_place.Block)
                {
                    item.GetComponent<Mission_offline.Raw_Place_script>().Anim_boomb = 1;
                }
            }
        });

    }

    void Update()
    {
        if (Anim_build == 0)
        {
            Content_build.transform.localPosition = Vector3.MoveTowards(Content_build.transform.localPosition, new Vector3(0,-6, 0), 0.1f);
        }

    }
}
