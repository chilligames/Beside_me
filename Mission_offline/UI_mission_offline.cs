using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Mission_offline;

public class UI_mission_offline : MonoBehaviour
{
    [Header("entity game")]
    public GameObject[,] All_place;

    public GameObject Pointer_player;
    public GameObject Pointer_enemy;
    public Color Color_enemy;
    public Color Color_player;


    [Header("BTNS")]
    public Button BTN_Bomb;
    public Button BTN_turret;
    public Button BTN_Castel;
    public Button BTN_Spawner;
    public Button BTN_sniper;
    public Button BTN_Cannon;
    public Button BTN_Trap;
    public Button BTN_teleport;

    [Header("Entity_touch")]
    public Button BTN_Up_right;
    public Button BTN_Up_left;
    public Button BTN_right;
    public Button BTN_down_left;
    public Button BTN_down_right;
    public Button BTN_left;


    [Header("Turn_object")]
    public TextMeshProUGUI Text_build_your_number;
    public TextMeshProUGUI Text_build_enemy_number;



    [Header("Raw_trap")]
    public GameObject Raw_bomb;
    public GameObject Raw_Turret;
    public GameObject Raw_Castel;
    public GameObject Raw_Spawner;
    public GameObject Raw_Sniper;
    public GameObject Raw_Cannon;
    public GameObject Raw_trap;
    public GameObject Raw_teleport;

    [Header("Slider")]
    public Slider Slider_player;
    public Image Slider_background;
    public Image Slider_fill;


    int lock_move = 0;

    int Total_turn;

    void Start()
    {
        All_place = GetComponentInParent<Mission_offline>().All_place;

        //action builds
        BTN_Bomb.onClick.AddListener(() =>
        {
            if (Pointer_player.GetComponent<Pointer_player>().Count >= 0)
            {
                print("cost_bomb");
                Instantiate(Raw_bomb, Pointer_player.transform.position, transform.rotation).GetComponent<Bomb>().Change_value_bomb(new Bomb.Bomb_setting { All_Place = All_place });
            }

        });

        BTN_Castel.onClick.AddListener(() =>
        {
            print("cost castle");

            Instantiate(Raw_Castel, Pointer_player.transform.position, Pointer_player.transform.rotation).GetComponent<Castel>().Change_value_castel(new Castel.Castel_setting { All_place = All_place, Place_for = Place_for.Player });
        });

        BTN_turret.onClick.AddListener(() =>
        {
            print("cost_turret");
            Instantiate(Raw_Turret, Pointer_player.transform.position, transform.rotation).GetComponent<Turret>().Change_valus_turret(new Turret.Setting_turet { All_place = All_place, Fire_to_ = Place_for.Enemy, magezin = 10 });
        });

        BTN_Spawner.onClick.AddListener(() =>
        {
            print("cost spawner here");
            Instantiate(Raw_Spawner, Pointer_player.transform.position, transform.rotation).GetComponent<Spawner>().Change_values_spawner(new Spawner.Setting_spawner { place_For = Place_for.Player, All_place = All_place, Count_farm = 50 });
        });

        BTN_sniper.onClick.AddListener(() =>
        {
            print("cost sniper");
            Instantiate(Raw_Sniper, Pointer_player.transform.position, transform.rotation).GetComponent<Sniper>().Change_value_sniper(new Sniper.Setting_sniper { All_place = All_place, Magezin = 25, place_For = Place_for.Player });
        });

        BTN_Cannon.onClick.AddListener(() =>
        {
            print("cost cannoc");
            Instantiate(Raw_Cannon, Pointer_player.transform.position, transform.rotation).GetComponent<Cannon>().Change_value_cannon(new Cannon.Setting_Cannon { All_place = All_place, Magezin = 10, Place_for = Place_for.Player });

        });

        BTN_Trap.onClick.AddListener(() =>
        {
            print("cost trap");
            Instantiate(Raw_trap, Pointer_player.transform.position, transform.rotation).GetComponent<Trap>().Change_value(new Trap.Setting_Trap { Place_for = Place_for.Player, Pointer_enemy = Pointer_enemy, Pointer_player = Pointer_player, All_place = All_place });
        });

        BTN_teleport.onClick.AddListener(() =>
        {
            print("costteleport");
            Instantiate(Raw_teleport, Pointer_player.transform.position, transform.rotation).GetComponent<Teleport>().Chnage_value_teleport(new Teleport.Setting_teleport { All_place = All_place, Place_for = Place_for.Player, Pointer_enemy = Pointer_enemy, Pointer_player = Pointer_player }); ;

        });

        //btn_touch
        BTN_Up_left.onClick.AddListener(Up);
        BTN_Up_right.onClick.AddListener(Up);

        BTN_down_left.onClick.AddListener(Down);
        BTN_down_right.onClick.AddListener(Down);

        BTN_left.onClick.AddListener(Left);
        BTN_right.onClick.AddListener(Right);



        void Up()
        {
            Vector3 Pos_up = new Vector3(Pointer_player.transform.position.x, Pointer_player.transform.position.y + 0.5f);

            foreach (var item in Places_side_pointer_player)
            {
                if (Pos_up == item.transform.position && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Block)
                {
                    StartCoroutine(Move_pointer(Pos_up, Pointer_player));
                }
            }


        }
        void Down()
        {
            Vector3 pos_down = new Vector3(Pointer_player.transform.position.x, Pointer_player.transform.position.y - 0.5f);
            foreach (var item in Places_side_pointer_player)
            {
                if (pos_down == item.transform.position && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Block)
                {
                    StartCoroutine(Move_pointer(pos_down, Pointer_player));
                }
            }

        }

        void Left()
        {
            Vector3 pos_left = new Vector3(Pointer_player.transform.position.x - 0.5f, Pointer_player.transform.position.y);
            foreach (var item in Places_side_pointer_player)
            {
                if (item.transform.position == pos_left && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Block)
                {
                    StartCoroutine(Move_pointer(pos_left, Pointer_player));
                }
            }
        }
        void Right()
        {
            Vector3 pos_right = new Vector3(Pointer_player.transform.position.x + 0.5f, Pointer_player.transform.position.y);
            foreach (var item in Places_side_pointer_player)
            {
                if (item.transform.position == pos_right && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Block)
                {
                    StartCoroutine(Move_pointer(pos_right, Pointer_player));
                }
            }

        }

    }

    private void Update()
    {
        var Count_Build_Player = 0;
        var Count_build_enemy = 0;
        foreach (var item in All_place)
        {
            if (item.GetComponent<Place>().Setting_place.Place_for == Place_for.Enemy)
            {
                Count_build_enemy += 1;
            }
            else if (item.GetComponent<Place>().Setting_place.Place_for == Place_for.Player)
            {
                Count_Build_Player += 1;
            }
        }

        Text_build_enemy_number.text = Count_build_enemy.ToString();
        Text_build_your_number.text = Count_Build_Player.ToString();

        Slider_player.maxValue = Count_Build_Player;

            Slider_player.value = Count_build_enemy;
       
    }
    /// <summary>
    /// place inside player pointer
    /// </summary>
    GameObject[] Places_side_pointer_player
    {
        get
        {
            GameObject[] placess = new GameObject[5];
            int count = 0;
            foreach (var item in All_place)
            {
                if (Vector2.Distance(item.transform.position, Pointer_player.transform.position) <= 0.5f)
                {
                    for (int i = 0; i < placess.Length; i++)
                    {
                        if (placess[i] == null)
                        {
                            placess[i] = item;
                            count++;
                            break;
                        }
                    }
                }
            }

            var new_pos_place = new GameObject[count];

            for (int i = 0; i < placess.Length; i++)
            {
                if (placess[i] != null)
                {
                    for (int a = 0; a < new_pos_place.Length; a++)
                    {
                        if (new_pos_place[a] == null)
                        {
                            new_pos_place[a] = placess[i];
                            break;
                        }
                    }
                }
            }
            return new_pos_place;
        }
    }



    //move pointer to postion
    public IEnumerator Move_pointer(Vector3 postion, GameObject pointer)
    {
        if (lock_move == 0)
        {
            lock_move = 1;
            while (true)
            {
                yield return new WaitForSeconds(0.01f);
                if (pointer.transform.position != postion)
                {
                    pointer.transform.position = Vector3.MoveTowards(pointer.transform.position, postion, 0.2f);

                }
                else
                {
                    pointer.transform.position = postion;
                    lock_move = 0;
                    break;
                }

            }
        }
    }
}

public enum Type_Build
{
    Bomb,
    Castle,
    Turret,
    Spawner,
    Sniper,
    Cannon,
    Trap,
    Teleport

}
