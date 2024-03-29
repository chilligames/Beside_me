﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    Bomb_setting Setting;
    public GameObject Ring;
    public Button BTN_show_ring;

    public void Change_value_bomb(Bomb_setting Bomb_setting)
    {
        Setting = Bomb_setting;

        BTN_show_ring.onClick.AddListener(() =>
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

    }

    private void Update()
    {
        if (transform.localScale != Vector3.zero)
        {
           transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 0.001f);
        }
        else
        {
            foreach (var item in Setting.All_Place)
            {
                if (Vector3.Distance(item.transform.position, transform.position) <= 0.8 && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Enemy && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Player)
                {
                    item.GetComponent<Place>().Count = 0;
                    item.GetComponent<Place>().Setting_place.Place_for = Place_for.Empity;
                    item.GetComponent<Place>().Setting_place.Type_place = Type_place.Place;
                    item.GetComponent<Place>().Update_place_from_pointers();
                }
            }

            Destroy(gameObject);
        }

    }

    public struct Bomb_setting
    {
        public GameObject[,] All_Place;
    }
}
