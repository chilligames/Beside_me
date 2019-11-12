using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pointer_enemy : MonoBehaviour
{
    [Header("Traps")]
    public GameObject Raw_Bomb;
    public GameObject Raw_turret;
    public GameObject Raw_castle;
    public GameObject Raw_spawner;
    public GameObject Raw_Sniper;
    public GameObject Raw_Cannon;
    public GameObject Raw_Trap;
    public GameObject Raw_teleport;

    [Header("Entitys")]
    public GameObject Pointer_player;


    //Entity enemy
    Setting_Enemy Enemy_setting;

    GameObject[] Place_Can_move_bot;

    //setting ai
    public TextMeshProUGUI Text_Turn;


    GameObject Distiny_pointer;
    GameObject Last_postion;

    public int Count;
    int Atack_time;
    int count_last_postion;
    int lock_move;

    public void Change_value_enemy_pointer(Setting_Enemy setting)
    {
        //cange values
        Enemy_setting = setting;

        //convert to arry place;
        Place_Can_move_bot = new GameObject[9];

        int count = 0;
        foreach (var item in Enemy_setting.All_place)
        {
            if (item.GetComponent<Place>().Setting_place.Type_place != Type_place.Block && Vector3.Distance(gameObject.transform.position, item.transform.position) <= 0.7f)
            {
                for (int i = 0; i < Place_Can_move_bot.Length; i++)
                {
                    if (Place_Can_move_bot[i] == null)
                    {
                        Place_Can_move_bot[i] = item;
                        count++;
                        break;
                    }
                }
            }
        }

        var new_pos = new GameObject[count];

        for (int i = 0; i < Place_Can_move_bot.Length; i++)
        {
            if (Place_Can_move_bot[i] != null)
            {
                for (int a = 0; a < new_pos.Length; a++)
                {
                    if (new_pos[a] == null)
                    {
                        new_pos[a] = Place_Can_move_bot[i];
                        break;
                    }
                }
            }

        }

        Place_Can_move_bot = new_pos;

        //frist distiny_finde
        if (Distiny_pointer == null)
        {
            foreach (var item in Place_Can_move_bot)
            {
                if (item.GetComponent<Place>().Setting_place.Type_place != Type_place.Enemy)
                {
                    Distiny_pointer = item;
                    break;
                }
            }
        }

    }


    private void Start()
    {
        StartCoroutine(Start_bot());
        StartCoroutine(Control_build());
        StartCoroutine(Control_bug_go_to_distiny());
    }

    private void Update()
    {
        //control text
        if (Count >= 1)
        {
            Text_Turn.text = Count.ToString();
        }
        else
        {
            Text_Turn.text = "";
        }


        //update place
        foreach (var item in Enemy_setting.All_place)
        {
            if (item.transform.position == item.transform.position)
            {
                item.GetComponent<Place>().Update_place_from_pointers();
            }
        }

        //show pointer to player
        if (Vector3.Distance(Pointer_player.transform.position, gameObject.transform.position) < 0.8f)
        {
            Text_Turn.gameObject.SetActive(true);
            GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            Text_Turn.gameObject.SetActive(false);
            GetComponent<SpriteRenderer>().color = Color.clear;
        }

    }


    public void Reset_boat()
    {
        Last_postion = Enemy_setting.place_enemy;
        Distiny_pointer = Enemy_setting.place_enemy;
    }


    public void Builder(Type_Build type_build)
    {
        switch (type_build)
        {
            case Type_Build.Bomb:
                {
                    Instantiate(Raw_Bomb, transform.position, transform.rotation).GetComponent<Bomb>().Change_value_bomb(new Bomb.Bomb_setting { All_Place = Enemy_setting.All_place });
                }
                break;
            case Type_Build.Castle:
                {
                    Instantiate(Raw_castle, transform.position, transform.rotation).GetComponent<Castel>().Change_value_castel(new Castel.Castel_setting { All_place = Enemy_setting.All_place, Place_for = Place_for.Enemy });
                }
                break;
            case Type_Build.Turret:
                {
                    Instantiate(Raw_turret, gameObject.transform.position, transform.rotation).GetComponent<Turret>().Change_valus_turret(new Turret.Setting_turet { All_place = Enemy_setting.All_place, Fire_to_ = Place_for.Player, magezin = 10 });
                }
                break;
            case Type_Build.Spawner:
                {
                    Instantiate(Raw_spawner, gameObject.transform.position, transform.rotation).GetComponent<Spawner>().Change_values_spawner(new Spawner.Setting_spawner { All_place = Enemy_setting.All_place, Count_farm = 50, place_For = Place_for.Enemy }); ;
                }
                break;
            case Type_Build.Sniper:
                {
                    Instantiate(Raw_Sniper, gameObject.transform.position, transform.rotation).GetComponent<Sniper>().Change_value_sniper(new Sniper.Setting_sniper { All_place = Enemy_setting.All_place, Magezin = 25, place_For = Place_for.Enemy });
                }
                break;
            case Type_Build.Cannon:
                {
                    Instantiate(Raw_Cannon, gameObject.transform.position, transform.rotation).GetComponent<Cannon>().Change_value_cannon(new Cannon.Setting_Cannon { All_place = Enemy_setting.All_place, Magezin = 10, Place_for = Place_for.Enemy });
                }
                break;
            case Type_Build.Trap:
                {
                    Instantiate(Raw_Trap, gameObject.transform.position, transform.rotation).GetComponent<Trap>().Change_value(new Trap.Setting_Trap { All_place = Enemy_setting.All_place, Place_for = Place_for.Enemy, Pointer_enemy = gameObject, Pointer_player = Pointer_player });
                }
                break;
            case Type_Build.Teleport:
                {
                    Instantiate(Raw_teleport, gameObject.transform.position, transform.rotation).GetComponent<Teleport>().Chnage_value_teleport(new Teleport.Setting_teleport { All_place = Enemy_setting.All_place, Place_for = Place_for.Enemy, Pointer_enemy = gameObject, Pointer_player = Pointer_player, });
                }
                break;
        }
    }

    IEnumerator Start_bot()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (Count >= 1)
            {
                //find distiny
                foreach (var item in Place_Can_move_bot)
                {
                    if (item.GetComponent<Place>().Setting_place.Place_for == Place_for.Player)
                    {
                        Distiny_pointer = item;
                        break;
                    }
                    else
                    {
                        if (Atack_time >= 2)
                        {
                            Atack_time = 0;
                            foreach (var place_attack in Place_Can_move_bot)
                            {
                                if (Vector3.Distance(place_attack.transform.position, Enemy_setting.Place_player.transform.position) <= Vector3.Distance(Distiny_pointer.transform.position, Enemy_setting.Place_player.transform.position))
                                {
                                    Distiny_pointer = place_attack;
                                }
                            }

                        }
                        else
                        {
                            foreach (var white in Place_Can_move_bot)
                            {
                                if (white.GetComponent<Place>().Setting_place.Place_for == Place_for.Empity)
                                {
                                    Distiny_pointer = white;
                                }
                                else
                                {
                                    if (Distiny_pointer.GetComponent<Place>().Setting_place.Place_for != Place_for.Empity)
                                    {
                                        int rand_step = Random.Range(0, Place_Can_move_bot.Length);

                                        Distiny_pointer = Place_Can_move_bot[rand_step];
                                    }

                                }


                            }

                        }
                    }
                }

                //move enemy to distiny
                while (true)
                {
                    yield return new WaitForSeconds(0.01f);
                    lock_move = 0;
                    if (gameObject.transform.position != Distiny_pointer.transform.position)
                    {
                        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Distiny_pointer.transform.position, 0.1f);
                    }
                    else
                    {
                        Change_value_enemy_pointer(Enemy_setting);
                        lock_move = 1;
                        Atack_time++;
                        break;
                    }

                }
            }
            else
            {

                foreach (var item in Place_Can_move_bot)
                {
                    if (
                        item.GetComponent<Place>().Setting_place.Place_for == Place_for.Enemy
                        && Vector3.Distance(item.transform.position, Enemy_setting.place_enemy.transform.position) <= Vector3.Distance(Distiny_pointer.transform.position, Enemy_setting.place_enemy.transform.position)
                        && Last_postion != item
                        )
                    {
                        Distiny_pointer = item;
                        Last_postion = item;
                        break;
                    }
                    else if (Vector3.Distance(item.transform.position, Enemy_setting.place_enemy.transform.position) <= Vector3.Distance(Distiny_pointer.transform.position, Enemy_setting.place_enemy.transform.position))
                    {
                        Distiny_pointer = item;
                        Last_postion = item;
                    }
                }

                if (Last_postion.transform.position == gameObject.transform.position && count_last_postion == 3)
                {
                    var rand_pos = Random.Range(0, Place_Can_move_bot.Length);
                    Distiny_pointer = Place_Can_move_bot[rand_pos];
                    count_last_postion = 0;
                }
                else
                {
                    count_last_postion++;
                }

                //back to home
                while (true)
                {
                    yield return new WaitForSeconds(0.01f);
                    lock_move = 0;
                    if (gameObject.transform.position != Distiny_pointer.transform.position)
                    {
                        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Distiny_pointer.transform.position, 0.1f);
                    }
                    else
                    {
                        Change_value_enemy_pointer(Enemy_setting);
                        lock_move = 1;
                        break;
                    }

                }

            }

        }
    }


    IEnumerator Control_build()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (Enemy_setting.place_enemy.GetComponent<Place>().Count >= 10)//10
            {
                Builder(Type_Build.Bomb);
            }

            yield return new WaitForSeconds(0.1f);
            if (Enemy_setting.place_enemy.GetComponent<Place>().Count >= 25)//25
            {
                Builder(Type_Build.Castle);
            }

            yield return new WaitForSeconds(0.1f);
            if (Enemy_setting.place_enemy.GetComponent<Place>().Count >= 20)//20
            {
                Builder(Type_Build.Turret);
            }

            yield return new WaitForSeconds(0.1f);
            if (Enemy_setting.place_enemy.GetComponent<Place>().Count >= 35)//35
            {
                Builder(Type_Build.Spawner);
            }

            yield return new WaitForSeconds(0.1f);
            if (Enemy_setting.place_enemy.GetComponent<Place>().Count >= 40)
            {
                Builder(Type_Build.Sniper);
            }

            yield return new WaitForSeconds(0.1f);
            if (Enemy_setting.place_enemy.GetComponent<Place>().Count >= 50)
            {
                Builder(Type_Build.Cannon);
            }

            yield return new WaitForSeconds(0.1f);
            if (Enemy_setting.place_enemy.GetComponent<Place>().Count >= 60)
            {
                Builder(Type_Build.Trap);
            }


            yield return new WaitForSeconds(0.1f);
            if (Enemy_setting.place_enemy.GetComponent<Place>().Count >= 70)
            {
                Builder(Type_Build.Teleport);
            }
        }
    }


    IEnumerator Control_bug_go_to_distiny()
    {
        while (true)
        {
            var last_postion = Distiny_pointer;
            yield return new WaitForSeconds(5);

            if (lock_move == 1)
            {

                if (last_postion == Distiny_pointer)
                {
                    foreach (var item in Place_Can_move_bot)
                    {
                        if (Vector3.Distance(item.transform.position, Enemy_setting.Place_player.transform.position) <= Vector3.Distance(gameObject.transform.position, Enemy_setting.Place_player.transform.position))
                        {
                            Distiny_pointer = item;
                            print("find path");
                        }
                        else
                        {
                            print("dipos");
                            if (Enemy_setting.place_enemy.GetComponent<Place>().Count >= 2)
                            {
                                Count += 3;
                                Enemy_setting.place_enemy.GetComponent<Place>().Count -= 2;
                            }
                        }
                    }
                }

            }
        }
    }

    public struct Setting_Enemy
    {
        public GameObject[,] All_place;
        public GameObject Place_player;
        public GameObject place_enemy;
    }
}
