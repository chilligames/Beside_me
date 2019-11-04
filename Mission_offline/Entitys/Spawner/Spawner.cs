using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Setting_spawner Setting;

    public GameObject[] Place_farms;

    public void Change_values_spawner(Setting_spawner Setting_spawner)
    {
        //change value
        Setting = Setting_spawner;


        //finde place farm
        Place_farms = new GameObject[5];
        int Count_place = 0;

        foreach (var item in Setting.All_place)
        {
            if (Vector3.Distance(item.transform.position, gameObject.transform.position) <= 0.7)
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

        var New_place_farm = new GameObject[Count_place - 1];

        foreach (var item in Place_farms)
        {
            if (item != null)
            {
                for (int i = 0; i < New_place_farm.Length; i++)
                {
                    if (New_place_farm == null)
                    {
                        New_place_farm[i] = item;
                        break;
                    }
                }
            }
        }

        Place_farms = New_place_farm;
        foreach (var item in Place_farms)
        {
            print(item);
        }
    }


    public struct Setting_spawner
    {
        public GameObject[,] All_place;
        public Place_for place_For;

    }

}
