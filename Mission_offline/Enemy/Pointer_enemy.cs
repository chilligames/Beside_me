using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pointer_enemy : MonoBehaviour
{
    //Entity enemy
    Setting_Enemy Enemy_setting;
    Object_attack_defance Attack_Defance;

    GameObject[] Place_Can_move_bot;

    //setting ai
    public TextMeshProUGUI Text_Turn;


    GameObject Distiny_pointer;
    GameObject Last_postion;

    public int Count;
    int Atack_time;
    int count_last_postion;
    int lock_move;

    public void Change_value_enemy_pointer(Setting_Enemy setting, Object_attack_defance attack_Defance)
    {
        //cange values
        Enemy_setting = setting;
        Attack_Defance = attack_Defance;

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
        if (Count >= 1)
        {
            Text_Turn.text = Count.ToString();
        }
        else
        {
            Text_Turn.text = "";
        }
    }


    public void Builder(Type_Build type_build)
    {
        switch (type_build)
        {
            case Type_Build.Bomb:
                {
                    foreach (var item in Enemy_setting.All_place)
                    {
                        if (item.GetComponent<Place>().Setting_place.Place_for == Place_for.Block && Vector3.Distance(item.transform.position, gameObject.transform.position) <= 1)
                        {
                            item.GetComponent<Place>().Setting_place.Type_place = Type_place.Place;
                            item.GetComponent<Place>().Setting_place.Place_for = Place_for.Empity;
                            Enemy_setting.place_enemy.GetComponent<Place>().Count -= 10;
                            break;
                        }
                    }
                }
                break;
            case Type_Build.Turret:
                {
                    var count_can_build = 0;
                    var place_build = new GameObject[9];

                    foreach (var item in Enemy_setting.All_place)
                    {
                        if (item.GetComponent<Place>().Type_Build == Type_Build.Null && item.GetComponent<Place>().Setting_place.Place_for != Place_for.Player && item.GetComponent<Place>().Setting_place.Place_for == Place_for.Empity && Vector3.Distance(item.transform.position, gameObject.transform.position) <= 1)
                        {
                            for (int i = 0; i < place_build.Length; i++)
                            {
                                if (place_build[i] == null)
                                {
                                    place_build[i] = item;
                                    count_can_build++;
                                    break;
                                }
                            }
                        }
                    }

                    var new_place = new GameObject[count_can_build];
                    for (int i = 0; i < place_build.Length; i++)
                    {
                        if (place_build[i] != null)
                        {
                            for (int a = 0; a < new_place.Length; a++)
                            {
                                if (new_place[a] == null)
                                {
                                    new_place[a] = place_build[i];
                                    break;
                                }
                            }
                        }
                    }

                    place_build = new_place;

                    var rand = Random.Range(0, place_build.Length);
                    //if place build 0 bashe randm 0 bashe err mishe
                    if (place_build.Length != 0)
                    {
                        Instantiate(Attack_Defance.Raw_Turet, place_build[rand].transform).GetComponent<Turret>().Change_valus_turret(
                            new Turret.Setting_turet
                            {
                                All_place = Enemy_setting.All_place,
                                Fire_to_ = Place_for.Player,
                                magezin = 10,
                            });
                    }
                }
                break;
            case Type_Build.Castel:
                {
                    var count_can_build = 0;
                    var place_build = new GameObject[9];

                    foreach (var item in Enemy_setting.All_place)
                    {
                        if (item.GetComponent<Place>().Type_Build == Type_Build.Null && item.GetComponent<Place>().Setting_place.Place_for != Place_for.Player && item.GetComponent<Place>().Setting_place.Place_for == Place_for.Empity && Vector3.Distance(item.transform.position, gameObject.transform.position) <= 1)
                        {
                            for (int i = 0; i < place_build.Length; i++)
                            {
                                if (place_build[i] == null)
                                {
                                    place_build[i] = item;
                                    count_can_build++;
                                    break;
                                }
                            }
                        }
                    }

                    var new_place = new GameObject[count_can_build];
                    for (int i = 0; i < place_build.Length; i++)
                    {
                        if (place_build[i] != null)
                        {
                            for (int a = 0; a < new_place.Length; a++)
                            {
                                if (new_place[a] == null)
                                {
                                    new_place[a] = place_build[i];
                                    break;
                                }
                            }
                        }
                    }

                    place_build = new_place;

                    var rand = Random.Range(0, place_build.Length);
                    //if place build 0 bashe randm 0 bashe err mishe
                    if (place_build.Length != 0)
                    {
                        Instantiate(Attack_Defance.Raw_castel, place_build[rand].transform).GetComponent<Castel>().Change_value_castel(new Castel.Castel_setting { All_place = Enemy_setting.All_place, Place_for = Place_for.Enemy });
                    }
                }
                break;
        }
    }

    IEnumerator Start_bot()
    {


        while (true)
        {
            yield return new WaitForSeconds(0.003f);

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
                        Change_value_enemy_pointer(Enemy_setting, Attack_Defance);
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
                        Change_value_enemy_pointer(Enemy_setting, Attack_Defance);
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
            if (Enemy_setting.place_enemy.GetComponent<Place>().Count >= 0)
            {
                Builder(Type_Build.Bomb);
            }

            yield return new WaitForSeconds(0.1f);
            if (Enemy_setting.place_enemy.GetComponent<Place>().Count >= 15)
            {
                Builder(Type_Build.Turret);
            }
            yield return new WaitForSeconds(0.1f);
            if (Enemy_setting.place_enemy.GetComponent<Place>().Count >= 0)
            {
                Builder(Type_Build.Castel);
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
