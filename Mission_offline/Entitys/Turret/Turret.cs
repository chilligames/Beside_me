using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Raw_place = Mission_offline;

public class Turret : MonoBehaviour
{

    public Color Color_player;
    public Color Color_enemy;

    Setting_turet Setting;
    GameObject[] Can_shot_to;
    GameObject Place_turret;

    int Anim_turet;

    public void Change_valus_turret(Setting_turet Turet_setting)
    {
        //frist change value
        Setting = Turet_setting;

        //finde place for shot
        Can_shot_to = new GameObject[9];
        int count_place = 0;
        foreach (var item in Setting.All_place)
        {
            if (Vector3.Distance(item.transform.position, gameObject.transform.position) <= 0.8f)
            {
                for (int i = 0; i < Can_shot_to.Length; i++)
                {
                    if (Can_shot_to[i] == null)
                    {
                        Can_shot_to[i] = item;
                        count_place++;
                        break;
                    }
                }
            }
        }

        var new_place_shot = new GameObject[count_place];

        foreach (var item in Can_shot_to)
        {
            if (item != null)
            {
                for (int i = 0; i < new_place_shot.Length; i++)
                {
                    if (new_place_shot[i] == null)
                    {
                        new_place_shot[i] = item;
                        break;
                    }
                }
            }
        }

        Can_shot_to = new_place_shot;

        //find_place turret
        foreach (var item in Setting.All_place)
        {
            if (Vector3.Distance(transform.position, item.transform.position) == 0)
            {
                Place_turret = item;
                break;
            }
        }

        //distroy turret if target
        foreach (var item in Can_shot_to)
        {
            if (item == null)
            {
                Destroy(gameObject);
            }

        }

        //destroy turret if place null  
        if (Place_turret == null)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        switch (Setting.Fire_to_)
        {
            case Place_for.Enemy:
                {
                    //change color turret
                    gameObject.GetComponent<SpriteRenderer>().color = Color_player;

                    //give place for  enemy
                    Place_turret.GetComponent<Place>().Count += 1;
                    Place_turret.GetComponent<Place>().Setting_place.Place_for = Place_for.Player;
                }
                break;
            case Place_for.Player:
                {
                    //change color
                    gameObject.GetComponent<SpriteRenderer>().color = Color_enemy;

                    //give place fro player
                    Place_turret.GetComponent<Place>().Count += 1;
                    Place_turret.GetComponent<Place>().Setting_place.Place_for = Place_for.Enemy;
                }
                break;
        }
        StartCoroutine(Active_turet());
    }

    void Update()
    {
        if (Anim_turet == 0)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.1f, 0.1f, 0), 0.01f);
        }
        else if (Anim_turet == 1)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 0.01f);
            if (transform.localScale == Vector3.zero)
            {
                Destroy(gameObject);
            }
        }

        if (Place_turret.GetComponent<Place>().Count <= 0)
        {
            Anim_turet = 1;
        }
    }

    IEnumerator Active_turet()
    {
        while (true)
        {
            Change_valus_turret(Setting);
            foreach (var placess in Can_shot_to)
            {
                switch (Setting.Fire_to_)
                {

                    case Place_for.Enemy:
                        {
                            //fire to place
                            if (
                                placess.GetComponent<Place>().Count >= 1
                                && Setting.magezin >= 1
                                && placess.GetComponent<Place>().Setting_place.Type_place != Type_place.Enemy
                               && placess.GetComponent<Place>().Setting_place.Type_place != Type_place.Player
                              && placess.GetComponent<Place>().Setting_place.Place_for == Place_for.Enemy
                              )
                            {
                                Setting.magezin--;
                                placess.GetComponent<Place>().Count -= 1;
                                break;
                            }
                            else if (Setting.magezin <= 0)
                            {
                                StopCoroutine(Active_turet());
                                Anim_turet = 1;
                            }
                        }
                        break;
                    case Place_for.Player:
                        {
                            //fire to place
                            if (
                              placess.GetComponent<Place>().Count >= 1
                              && Setting.magezin >= 1
                              && placess.GetComponent<Place>().Setting_place.Type_place != Type_place.Enemy
                             && placess.GetComponent<Place>().Setting_place.Type_place != Type_place.Player
                            && placess.GetComponent<Place>().Setting_place.Place_for == Place_for.Player
                             )
                            {
                                Setting.magezin--;
                                placess.GetComponent<Place>().Count -= 1;
                                break;
                            }
                            else if (Setting.magezin <= 0)
                            {
                                StopCoroutine(Active_turet());
                                Anim_turet = 1;
                            }
                        }
                        break;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public struct Setting_turet
    {
        public int magezin;

        /// <summary>
        /// Placess can fire
        /// </summary>
        public GameObject[,] All_place;

        /// <summary>
        /// turret fire enemy or player
        /// </summary>
        public Place_for Fire_to_;
    }

}