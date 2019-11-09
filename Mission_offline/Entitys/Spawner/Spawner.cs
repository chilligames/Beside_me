using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Setting_spawner Setting;

    public GameObject[] Place_farms;

    public LineRenderer[] Line;



    public void Change_values_spawner(Setting_spawner Setting_spawner)
    {
        //change value
        Setting = Setting_spawner;


        //finde place farm
        Place_farms = new GameObject[5];
        int Count_place = 0;

        foreach (var item in Setting.All_place)
        {
            if (Vector3.Distance(item.transform.position, gameObject.transform.position) <= 0.7f)
            {
                if (
                    item.GetComponent<Place>().Setting_place.Place_for != Place_for.Block
                    && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Block
                    && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Enemy
                    && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Player
                    )
                {
                    for (int i = 0; i < Place_farms.Length; i++)
                    {
                        if (Place_farms[i] == null)
                        {
                            Place_farms[i] = item;
                            Count_place++;
                            break;
                        }
                    }
                }
            }
        }

        var New_place_farm = new GameObject[Count_place];

        foreach (var item in Place_farms)
        {
            if (item != null)
            {
                for (int i = 0; i < New_place_farm.Length; i++)
                {
                    if (New_place_farm[i] == null)
                    {
                        New_place_farm[i] = item;
                        break;
                    }
                }
            }
        }

        Place_farms = New_place_farm;


        //work
        StartCoroutine(Spawn_turn());

    }


    IEnumerator Spawn_turn()
    {
        //anim creat    
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            //anim_spawener
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.1f, 0.1f, 0), 0.01f);
            if (transform.localScale == new Vector3(0.1f, 0.1f, 0))
            {
                break;
            }
        }

        //work
        while (true)
        {
            yield return new WaitForSeconds(1);

            switch (Setting.place_For)
            {
                case Place_for.Enemy:
                    {
                        foreach (var item in Place_farms)
                        {
                            if (
                                item.GetComponent<Place>().Setting_place.Place_for != Place_for.Player
                                && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Block
                                && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Enemy
                                && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Player)
                            {
                                item.GetComponent<Place>().Count += 1;
                                item.GetComponent<Place>().Setting_place.Place_for = Place_for.Enemy;
                                item.GetComponent<Place>().Update_place_from_pointers();
                                Setting.Count_farm--;
                            }
                        }
                    }
                    break;
                case Place_for.Player:
                    {
                        foreach (var item in Place_farms)
                        {
                            if (
                                item.GetComponent<Place>().Setting_place.Place_for != Place_for.Enemy
                                && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Block
                                && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Enemy
                                && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Player)
                            {
                                item.GetComponent<Place>().Count += 1;
                                item.GetComponent<Place>().Setting_place.Place_for = Place_for.Player;
                                item.GetComponent<Place>().Update_place_from_pointers();
                                Setting.Count_farm--;
                            }
                        }
                    }
                    break;
            }

            if (Setting.Count_farm <= 0)
            {
                break;
            }
        }


        //anim destroy;
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 0.01f);
            if (transform.localScale == Vector3.zero)
            {
                Destroy(gameObject);

                break;
            }

        }
    }

    public struct Setting_spawner
    {
        public GameObject[,] All_place;
        public Place_for place_For;
        public int Count_farm;
    }

}
