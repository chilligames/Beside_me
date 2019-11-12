using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sniper : MonoBehaviour
{

    public GameObject Ring;
    public Button BTN_sniper;


    Setting_sniper Setting;


    GameObject Place_for_shot;

    GameObject place_sniper;



    public void Change_value_sniper(Setting_sniper setting_Sniper)
    {
        //chagne value
        Setting = setting_Sniper;

        BTN_sniper.onClick.AddListener(() =>
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


        //finde place sniper
        foreach (var item in Setting.All_place)
        {
            if (item.transform.position == gameObject.transform.position)
            {
                place_sniper = item;
                break;
            }
        }
        //destory in move
        if (place_sniper == null)
        {
            Destroy(gameObject);
        }
        //start work
        StartCoroutine(Start_sniper());
    }


    /// <summary>
    /// finde target finde
    /// </summary>
    public void Finde_target()
    {

        var Target = new GameObject[30];
        var count_target = 0;

        foreach (var item in Setting.All_place)
        {
            if (Vector3.Distance(item.transform.position, gameObject.transform.position) <= 2 && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Block && item.GetComponent<Place>().Setting_place.Place_for != Place_for.Block)
            {
                for (int i = 0; i < Target.Length; i++)
                {
                    if (Target[i] == null)
                    {
                        Target[i] = item;
                        count_target++;
                        break;
                    }
                }
            }
        }

        var new_pos = new GameObject[count_target];

        foreach (var item in Target)
        {
            if (item != null)
            {
                for (int i = 0; i < new_pos.Length; i++)
                {
                    if (new_pos[i] == null)
                    {
                        new_pos[i] = item;
                        break;
                    }
                }
            }
        }
        Target = new_pos;

        var rand_number = Random.Range(0, count_target);
        Place_for_shot = Target[rand_number];
    }


    IEnumerator Start_sniper()
    {
        //anim start
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.1f, 0.1f, 0), 0.01f);
            if (transform.localScale == new Vector3(0.1f, 0.1f, 0))
            {
                break;
            }
        }

        //work
        while (true)
        {
            Finde_target();

            if (Setting.Magezin >= 1)
            {
                switch (Setting.place_For)
                {
                    case Place_for.Enemy:
                        {
                            if (Place_for_shot.GetComponent<Place>().Count >= 1 && Place_for_shot.GetComponent<Place>().Setting_place.Place_for == Place_for.Player)
                            {
                                Place_for_shot.GetComponent<Place>().Count -= 1;
                                Setting.Magezin -= 1;
                            }
                            else
                            {
                                Finde_target();
                            }
                        }
                        break;
                    case Place_for.Player:
                        {
                            if (Place_for_shot.GetComponent<Place>().Count >= 1 && Place_for_shot.GetComponent<Place>().Setting_place.Place_for == Place_for.Enemy)
                            {
                                Place_for_shot.GetComponent<Place>().Count -= 1;
                                Setting.Magezin -= 1;
                            }
                            else
                            {
                                Finde_target();
                            }
                        }
                        break;
                }
            }
            else if (Setting.Magezin <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(1);
        }

        //anim end
        while (true)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 0.01f);
            if (transform.localScale == Vector3.zero)
            {
                Destroy(gameObject);
            }
        }
    }



    public struct Setting_sniper
    {
        public GameObject[,] All_place;
        public Place_for place_For;
        public int Magezin;
    }
}

