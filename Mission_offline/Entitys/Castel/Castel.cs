using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Mission_offline;
public class Castel : MonoBehaviour
{
    public Color Color_Player;
    public Color Color_enemy;
    public GameObject Ring;
    public Button BTN_castle;

    Castel_setting castel_setting_local;
    GameObject castel_place;

    int anim_castel = 0;

    public void Change_value_castel(Castel_setting castel_setting)
    {
        //cahnge value
        castel_setting_local = castel_setting;
        BTN_castle.onClick.AddListener(() =>
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



        //fide place
        foreach (var item in castel_setting_local.All_place)
        {
            if (item.transform.position == gameObject.transform.position)
            {
                castel_place = item;
            }
        }

        if (castel_place == null)
        {
            Destroy(gameObject);
            print("destroy");
        }


        //change color && work 
        switch (castel_setting_local.Place_for)
        {
            case Place_for.Enemy:
                {
                    GetComponent<SpriteRenderer>().color = Color_enemy;
                    print(castel_place.name);
                    castel_place.GetComponent<Place>().Count += 2;
                    castel_place.GetComponent<Place>().Setting_place.Place_for = Place_for.Enemy;
                }
                break;
            case Place_for.Player:
                {
                    GetComponent<SpriteRenderer>().color = Color_Player;
                    castel_place.GetComponent<Place>().Count += 2;
                    castel_place.GetComponent<Place>().Setting_place.Place_for = Place_for.Player;

                }
                break;
        }


        //active castel
        StartCoroutine(Acitve_castel());
    }

    private void Update()
    {

        if (castel_place.GetComponent<Place>().Count == 0)
        {
            Destroy(gameObject);
            print("destroy");
        }

        //contorl dead 
        if (castel_setting_local.Place_for == Place_for.Player && castel_place.GetComponent<Place>().Setting_place.Place_for == Place_for.Enemy)
        {
            anim_castel = 1;
        }
        else if (castel_setting_local.Place_for == Place_for.Enemy && castel_place.GetComponent<Place>().Setting_place.Place_for == Place_for.Player)
        {
            anim_castel = 1;
        }

        ///contorl dead anim 
        if (anim_castel == 0)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.1f, 0.1f, 0), 0.01f);
        }
        else if (anim_castel == 1)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 0.01f);
            if (transform.localScale == Vector3.zero)
            {
                Destroy(gameObject);
            }
        }
    }



    IEnumerator Acitve_castel()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            castel_place.GetComponent<Place>().Count++;
        }

    }

    public struct Castel_setting
    {
        public GameObject[,] All_place;
        public Place_for Place_for;
    }
}
