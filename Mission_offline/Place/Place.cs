﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Place : MonoBehaviour
{
    //entity places
    public place_setting Setting_place;
    public SpriteRenderer Image_cron;
    public Color Color_player;
    public Color Color_enemy;
    public Color Color_Block;
    public Color Color_Empity;

    public Color Color_Active;

    [Header("Envorment")]

    GameObject[] Place_inside_place;

    public TextMeshProUGUI Text_place;

    //place setting
    public int Count;
    int Lock_block_player = 0;
    int Lock_block_enemy = 0;


    public void Change_Value_Place_sctipt(place_setting Setting)
    {
        //change values
        Setting_place = Setting;

        //finde place inside
        Place_inside_place = new GameObject[8];
        int count = 0;
        foreach (var item in Setting_place.All_place)
        {
            if (Vector3.Distance(item.transform.position, gameObject.transform.position) <= 0.9)
            {
                for (int i = 0; i < Place_inside_place.Length; i++)
                {
                    if (Place_inside_place[i] == null)
                    {
                        Place_inside_place[i] = item;
                        count++;
                        break;
                    }
                }
            }
        }
        var fix_place = new GameObject[count];
        for (int i = 0; i < Place_inside_place.Length; i++)
        {
            if (Place_inside_place[i] != null)
            {
                for (int a = 0; a < fix_place.Length; a++)
                {
                    if (fix_place[a] == null)
                    {
                        fix_place[a] = Place_inside_place[i];
                        break;
                    }

                }
            }
        }
        Place_inside_place = fix_place;
    }

    private void Start()
    {
        if (Count > 0)
        {
            Text_place.text = Count.ToString();
        }
        else
        {
            Text_place.text = "";
        }

    }


    public void Update_place_from_pointers()
    {
        //control show for player
        switch (Setting_place.Place_for)
        {
            case Place_for.Player:
                {
                    foreach (var item in Setting_place.All_place)
                    {
                        if (Vector3.Distance(item.transform.position, transform.position) <= 1 && item.GetComponent<Place>().Setting_place.Place_for != Place_for.Block && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Block)
                        {
                            switch (item.GetComponent<Place>().Setting_place.Place_for)
                            {
                                case Place_for.Empity:
                                    {
                                        item.GetComponent<SpriteRenderer>().color = Color_Active;
                                    }
                                    break;
                                case Place_for.Enemy:
                                    {
                                        item.GetComponent<SpriteRenderer>().color = Color_enemy;
                                        item.GetComponent<Place>().Text_place.gameObject.SetActive(true);
                                    }
                                    break;
                                case Place_for.Player:
                                    {
                                        item.GetComponent<SpriteRenderer>().color = Color_player;

                                        item.GetComponent<Place>().Text_place.gameObject.SetActive(true);

                                    }
                                    break;
                            }
                        }
                    }
                }
                break;
        }


      

        //controller instance for build and defance in place
        switch (Setting_place.Type_place)
        {
            case Type_place.Place:
                Image_cron.gameObject.SetActive(false);
                break;
            case Type_place.Enemy:
                Image_cron.gameObject.SetActive(true);
                GetComponent<SpriteRenderer>().color = Color_enemy;
                break;
            case Type_place.Player:
                GetComponent<SpriteRenderer>().color = Setting_place.Color_player;
                Image_cron.gameObject.SetActive(true);
                break;
            case Type_place.Block:
                break;
        }


        //change place for with 0
        if (Count == 0 && Setting_place.Type_place != Type_place.Block && Setting_place.Type_place != Type_place.Enemy && Setting_place.Type_place != Type_place.Player) //change and cheack for player enemy
        {
            Setting_place.Place_for = Place_for.Empity;
            Text_place.text = "";

        }
        else if (Count > 0)
        {
            Text_place.text = Count.ToString();
        }


        //control pointer turn
        if (Setting_place.pointer_player.transform.position == gameObject.transform.position && Setting_place.Type_place != Type_place.Player)
        {
            if (Lock_block_player == 0)
            {

                switch (Setting_place.Place_for)
                {
                    case Place_for.Empity:
                        if (Setting_place.pointer_player.GetComponent<Pointer_player>().Count >= 1)
                        {
                            Setting_place.Place_for = Place_for.Player;
                            Setting_place.pointer_player.GetComponent<Pointer_player>().Count -= 1;
                            Count = 1;
                        }
                        break;
                    case Place_for.Enemy:
                        {
                            Lock_block_player = 1;
                            if ((Count - Setting_place.pointer_player.GetComponent<Pointer_player>().Count) >= 0)
                            {
                                Count -= Setting_place.pointer_player.GetComponent<Pointer_player>().Count;
                                if (Count == 0)
                                {
                                    Count = 1;
                                    Setting_place.Place_for = Place_for.Player;
                                }

                                Setting_place.pointer_player.GetComponent<Pointer_player>().Count = 0;

                            }
                            else
                            {
                                var count = Count;
                                Count -= Setting_place.pointer_player.GetComponent<Pointer_player>().Count;
                                Count = 1;
                                Setting_place.pointer_player.GetComponent<Pointer_player>().Count -= count;
                                Setting_place.Place_for = Place_for.Player;
                            }
                        }
                        break;
                    case Place_for.Player:
                        if (Count > 1)
                        {
                            Setting_place.pointer_player.GetComponent<Pointer_player>().Count += Count;
                            Count = 1;
                        }
                        break;
                }
            }
        }
        else
        {
            Lock_block_player = 0;
        }

        if (Setting_place.pointer_enemy.transform.position == gameObject.transform.position)
        {
            if (Lock_block_enemy == 0)
            {
                switch (Setting_place.Place_for)
                {
                    case Place_for.Empity:
                        if (Setting_place.pointer_enemy.GetComponent<Pointer_enemy>().Count >= 1)
                        {
                            Setting_place.Place_for = Place_for.Enemy;

                            Setting_place.pointer_enemy.GetComponent<Pointer_enemy>().Count -= 1;
                            Count = 1;
                        }
                        break;
                    case Place_for.Enemy:
                        if (Count > 1)
                        {
                            Setting_place.pointer_enemy.GetComponent<Pointer_enemy>().Count += Count;
                            Count = 1;
                        }
                        break;
                    case Place_for.Player:
                        Lock_block_enemy = 1;
                        if (Count - Setting_place.pointer_enemy.GetComponent<Pointer_enemy>().Count >= 0)
                        {
                            Count -= Setting_place.pointer_enemy.GetComponent<Pointer_enemy>().Count;
                            if (Count == 0)
                            {
                                Count = 1;
                                Setting_place.Place_for = Place_for.Enemy;
                            }
                            Setting_place.pointer_enemy.GetComponent<Pointer_enemy>().Count = 0;
                        }
                        else
                        {
                            var count = Count;
                            Count -= Setting_place.pointer_enemy.GetComponent<Pointer_enemy>().Count;
                            Count = 1;
                            Setting_place.pointer_enemy.GetComponent<Pointer_enemy>().Count -= count;
                            Setting_place.Place_for = Place_for.Enemy;
                        }
                        break;
                }
            }
        }
        else
        {
            Lock_block_enemy = 0;
        }

        //give turn from base
        if (Setting_place.pointer_enemy.transform.position == gameObject.transform.position && Setting_place.Type_place == Type_place.Enemy)
        {
            Setting_place.pointer_enemy.GetComponent<Pointer_enemy>().Count += Count;
            Count = 0;
        }
        else if (Setting_place.pointer_player.transform.position == gameObject.transform.position && Setting_place.Type_place == Type_place.Player)
        {
            Setting_place.pointer_player.GetComponent<Pointer_player>().Count += Count;
            Count = 0;
        }

        //win losse Controll
        if (Setting_place.Type_place == Type_place.Enemy && Setting_place.pointer_player.transform.position == gameObject.transform.position)
        {
            if ((Count - Setting_place.pointer_player.GetComponent<Pointer_player>().Count) < 0)
            {
                print(" game win code here  ");
            }
        }

        if (Setting_place.Type_place == Type_place.Player && Setting_place.pointer_enemy.transform.position == gameObject.transform.position)
        {
            if ((Count - Setting_place.pointer_enemy.GetComponent<Pointer_enemy>().Count) < 0)
            {
                print("losse");
            }
        }

    }
}




/*.............place setting............*/
public enum Type_place
{
    Place, Player, Block, Enemy
}

public enum Place_for
{
    Empity, Enemy, Player, Block
}

public struct place_setting
{
    public Type_place Type_place;
    public Place_for Place_for;
    public Color Color_player;
    public Color Color_enemy;
    public GameObject pointer_player;
    public GameObject pointer_enemy;
    public GameObject[,] All_place;
}