using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Color Color_player;
    public Color Color_enemy;

    Setting_turet Setting;
    GameObject[,] All_place;
    GameObject[] Can_shot_to;

    int Anim_turet;

    public void Change_valus_turret(Setting_turet Turet_setting)
    {
        //frist change value
        Setting = Turet_setting;
        this.All_place = Turet_setting.All_place;

        //finde place for shot
        Can_shot_to = new GameObject[9];
        int count_place = 0;
        foreach (var item in All_place)
        {
            if (Vector3.Distance(item.transform.position, gameObject.transform.position) <= 0.7f)
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

    }
    private void Start()
    {
        StartCoroutine(Active_turet());
    }

    void Update()
    {
        if (Anim_turet == 0)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.02f, 0.02f, 0), 0.001f);
        }
        else if (Anim_turet == 1)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 0.001f);
            if (transform.localScale == Vector3.zero)
            {
                Destroy(gameObject);
                
            }
        }
    }

    IEnumerator Active_turet()
    {
        while (true)
        {
            Change_valus_turret(Setting);//cheack
            foreach (var placess in Can_shot_to)
            {
                switch (Setting.Fire_to_)
                {
                    case Mission_offline.Raw_Place_script.Place_for.Enemy:
                        {
                            //change color turret
                            gameObject.GetComponent<SpriteRenderer>().color = Color_player;

                            //fire to place
                            if (
                                placess.GetComponent<Mission_offline.Raw_Place_script>().Count >= 1
                                && Setting.magezin >= 1
                                && placess.GetComponent<Mission_offline.Raw_Place_script>().Setting_place.Type_place != Mission_offline.Raw_Place_script.Type_place.Enemy
                               && placess.GetComponent<Mission_offline.Raw_Place_script>().Setting_place.Type_place != Mission_offline.Raw_Place_script.Type_place.Player
                              && placess.GetComponent<Mission_offline.Raw_Place_script>().Setting_place.Place_for == Mission_offline.Raw_Place_script.Place_for.Enemy
                              )
                            {
                                Setting.magezin--;
                                placess.GetComponent<Mission_offline.Raw_Place_script>().Count -= 1;
                                break;
                            }
                            else if (Setting.magezin <= 0)
                            {
                                StopCoroutine(Active_turet());
                                placess.GetComponent<Mission_offline.Raw_Place_script>().Type_Build = Mission_offline.Type_Build.Null;
                                placess.GetComponent<Mission_offline.Raw_Place_script>().Setting_place.Place_for = Mission_offline.Raw_Place_script.Place_for.Empity;
                                Anim_turet = 1;
                            }

                        }
                        break;
                    case Mission_offline.Raw_Place_script.Place_for.Player:
                        {
                            //change color
                            gameObject.GetComponent<SpriteRenderer>().color = Color_enemy;

                            //fire to place
                            if (
                              placess.GetComponent<Mission_offline.Raw_Place_script>().Count >= 1
                              && Setting.magezin >= 1
                              && placess.GetComponent<Mission_offline.Raw_Place_script>().Setting_place.Type_place != Mission_offline.Raw_Place_script.Type_place.Enemy
                             && placess.GetComponent<Mission_offline.Raw_Place_script>().Setting_place.Type_place != Mission_offline.Raw_Place_script.Type_place.Player
                            && placess.GetComponent<Mission_offline.Raw_Place_script>().Setting_place.Place_for == Mission_offline.Raw_Place_script.Place_for.Player
                             )
                            {
                                Setting.magezin--;
                                placess.GetComponent<Mission_offline.Raw_Place_script>().Count -= 1;
                                break;
                            }
                            else if (Setting.magezin <= 0)
                            {
                                StopCoroutine(Active_turet());
                                placess.GetComponent<Mission_offline.Raw_Place_script>().Type_Build = Mission_offline.Type_Build.Null;
                                placess.GetComponent<Mission_offline.Raw_Place_script>().Setting_place.Place_for = Mission_offline.Raw_Place_script.Place_for.Empity;
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
        public Mission_offline.Raw_Place_script.Place_for Fire_to_;
    }

}