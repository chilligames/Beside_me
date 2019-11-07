using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    Setting_Trap Setting;
    GameObject Place_trap;

    int anim_trap;

    public void Change_value(Setting_Trap Setting_trap)
    {
        //change vvalue;
        Setting = Setting_trap;



        //finde place trap
        foreach (var item in Setting.All_place)
        {

            if (Vector3.Distance(item.transform.position, transform.position) == 0)
            {
                Place_trap = item;
                break;
            }
        }

        //destroyu if null  
        if (Place_trap == null)
        {
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        switch (Setting.Place_for)
        {
            case Place_for.Enemy:
                {
                    if (Place_trap.transform.position == Setting.Pointer_player.transform.position)
                    {
                        Setting.Pointer_player.GetComponent<Pointer_player>().Count = 0;
                        anim_trap = 1;
                    }
                }
                break;
            case Place_for.Player:
                {

                    if (Place_trap.transform.position == Setting.Pointer_enemy.transform.position)
                    {
                        Setting.Pointer_enemy.GetComponent<Pointer_enemy>().Count = 0;
                        anim_trap = 1;
                    }
                }
                break;
        }



        if (anim_trap == 0)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.1f, 0.1f, 0), 0.01f);
        }
        else if (anim_trap == 1)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 0.01f);
            if (transform.localScale == Vector3.zero)
            {
                Destroy(gameObject);
            }
        }

    }


    public struct Setting_Trap
    {
        public GameObject[,] All_place;
        public GameObject Pointer_player;
        public GameObject Pointer_enemy;
        public Place_for Place_for;
    }

}
