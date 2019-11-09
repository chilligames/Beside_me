using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    public GameObject Ring;
    public Button BTN_spawner;


    Setting_teleport Setting;

    GameObject place_teleport;
    GameObject place_enemy;
    GameObject place_player;

    int anim_teleport;

    public void Chnage_value_teleport(Setting_teleport Setting_Teleport)
    {
        //change values
        Setting = Setting_Teleport;
        BTN_spawner.onClick.AddListener(() =>
        {
            if (Ring.activeInHierarchy)
            {
                Ring.SetActive(false);
            }
            else
            {
                Ring.SetActive(true);
            }
        });


        //finde place player enemy
        foreach (var item in Setting.All_place)
        {
            if (item.GetComponent<Place>().Setting_place.Type_place == Type_place.Enemy)
            {
                place_enemy = item;
            }
            else if (item.GetComponent<Place>().Setting_place.Type_place == Type_place.Player)
            {
                place_player = item;
            }

        }



        foreach (var item in Setting.All_place)
        {
            if (Vector3.Distance(item.transform.position, transform.position) == 0)
            {
                place_teleport = item;
                break;
            }
        }

        //control nulll place
        if (place_teleport == null)
        {
            Destroy(gameObject);
        }

    }


    private void Update()
    {

        //work
        switch (Setting.Place_for)
        {
            case Place_for.Enemy:
                {
                    if (place_teleport.transform.position == Setting.Pointer_player.transform.position)
                    {
                        Setting.Pointer_player.transform.position = place_player.transform.position;

                        anim_teleport = 1;
                        Setting.Pointer_player.GetComponentInParent<Mission_offline>().Reset_pointer_player();
                    }
                }
                break;
            case Place_for.Player:
                {
                    if (place_teleport.transform.position == Setting.Pointer_enemy.transform.position)
                    {
                        Setting.Pointer_enemy.transform.position = place_enemy.transform.position;

                        Setting.Pointer_enemy.GetComponent<Pointer_enemy>().Reset_boat();
                        anim_teleport = 1;
                    }
                }
                break;
        }

        //anim spawn

        if (anim_teleport == 0)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.1f, 0.1f, 0), 0.1f);
        }
        else if (anim_teleport == 1)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 0.1f);
            if (transform.localScale == Vector3.zero)
            {
                Destroy(gameObject);

            }
        }


    }

    public struct Setting_teleport
    {
        public GameObject Pointer_player;
        public GameObject Pointer_enemy;
        public GameObject[,] All_place;
        public Place_for Place_for;
    }
}
