using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Mission_offline;
public class Castel : MonoBehaviour
{
    public Color Color_Player;
    public Color Color_enemy;

    Castel_setting castel_setting_local;
    GameObject castel_place;

    int anim_castel = 0;

    public void Change_value_castel(Castel_setting castel_setting)
    {
        //cahnge value
        castel_setting_local = castel_setting;

        //fide place
        foreach (var item in castel_setting_local.All_place)
        {

            if (Vector3.Distance(item.transform.position, gameObject.transform.position) == 0)
            {
                castel_place = item;
            }
        }

        //change color && work 
        switch (castel_setting_local.Place_for)
        {
            case Raw_Place_script.Place_for.Enemy:
                {
                    GetComponent<SpriteRenderer>().color = Color_enemy;
                    castel_place.GetComponent<Raw_Place_script>().Count += 2;
                    castel_place.GetComponent<Raw_Place_script>().Setting_place.Place_for = Raw_Place_script.Place_for.Enemy;
                }
                break;
            case Raw_Place_script.Place_for.Player:
                {
                    GetComponent<SpriteRenderer>().color = Color_Player;
                    castel_place.GetComponent<Raw_Place_script>().Count += 2;
                    castel_place.GetComponent<Raw_Place_script>().Setting_place.Place_for = Raw_Place_script.Place_for.Player;

                }
                break;
        }

        //active castel
        StartCoroutine(Acitve_castel());
        StartCoroutine(Deactive_castel());
    }

    private void Update()
    {
        if (castel_place.GetComponent<Raw_Place_script>().Count == 0)
        {
            print("destroy");
        }

        if (castel_setting_local.Place_for == Raw_Place_script.Place_for.Player && castel_place.GetComponent<Raw_Place_script>().Setting_place.Place_for == Raw_Place_script.Place_for.Enemy)
        {
            anim_castel = 1;
        }
        else if (castel_setting_local.Place_for == Raw_Place_script.Place_for.Enemy && castel_place.GetComponent<Raw_Place_script>().Setting_place.Place_for == Raw_Place_script.Place_for.Player)
        {
            anim_castel = 1;
        }

        if (anim_castel == 0)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.02f, 0.02f, 0), 0.001f);
        }
        else if (anim_castel == 1)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 0.001f);
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
            yield return new WaitForSeconds(1f);
            if (castel_place.GetComponent<Raw_Place_script>().Count >= 1)
            {
                castel_place.GetComponent<Raw_Place_script>().Count++;
            }
        }

    }

    IEnumerator Deactive_castel()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }

    public struct Castel_setting
    {
        public GameObject[,] All_place;
        public Raw_Place_script.Place_for Place_for;
    }
}
