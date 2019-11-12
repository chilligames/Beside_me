using System.Collections;
using UnityEngine;

public class Mission_offline : MonoBehaviour
{
    [Header("entity_game")]
    public Color Color_player;
    public Color Color_enemy;
    public GameObject Raw_Place;
    public Transform Place_fild;
    [HideInInspector]
    public GameObject pointer_player;
    [HideInInspector()]
    public GameObject Pointer_enemy;

    [Header("Setting mission")]
    public int Y_size;//dfult 11
    public int X_size;//defult 9
    [Range(3, 10)]
    public int Deficullty;//defult 5


    public Vector2 Start_pos_fild;
    float? Collom_pos;//defult -2
    public GameObject[,] All_place;
    public GameObject[] Place_blocks;
    public GameObject[] Place_white;

    [HideInInspector]
    public GameObject Place_player;
    [HideInInspector]
    public GameObject Place_Enemy;

    void Start()
    {
        //finde place x
        if (Collom_pos == null)
        {
            Collom_pos = Start_pos_fild.x;
        }


        //place creator
        All_place = new GameObject[Y_size, X_size];
        for (int i = 0; i < Y_size; i++)
        {
            for (int a = 0; a < X_size; a++)
            {
                All_place[i, a] = Instantiate(Raw_Place, Place_fild);
                All_place[i, a].name = i.ToString() + a.ToString();
                All_place[i, a].transform.position = Start_pos_fild;
                Start_pos_fild = new Vector2(Start_pos_fild.x + 0.5f, Start_pos_fild.y);
            }
            Start_pos_fild = new Vector2((float)Collom_pos, Start_pos_fild.y - 0.5f);
        }


        //block place
        int count_block = 0;
        ///creat_Place_block
        for (int i = 0; i < Y_size; i++)
        {
            for (int a = 0; a < X_size; a++)
            {
                int rand = Random.Range(1, Deficullty); //if max value >0 mission hard 
                if (rand == 2)
                {
                    All_place[i, a].GetComponent<SpriteRenderer>().color = Color.black;
                    All_place[i, a].GetComponent<Place>().Change_Value_Place_sctipt(
                        new place_setting
                        {
                            All_place = All_place,
                            Color_enemy = Color_enemy,
                            Type_place = Type_place.Block,
                            Place_for = Place_for.Block,
                            Color_player = Color_player,
                            pointer_enemy = Pointer_enemy,
                            pointer_player = pointer_player
                        });


                    count_block++;
                }
            }

        }
        ////pick place_block to new fild
        Place_blocks = new GameObject[count_block];
        for (int i = 0; i < Y_size; i++)
        {
            for (int a = 0; a < X_size; a++)
            {
                if (All_place[i, a].GetComponent<Place>().Setting_place.Type_place == Type_place.Block)
                {
                    for (int b = 0; b < count_block; b++)
                    {
                        if (Place_blocks[b] == null)
                        {
                            Place_blocks[b] = All_place[i, a];
                            break;
                        }
                    }
                }
            }

        }

        //pick place null white
        int count_white = All_place.Length - count_block;
        Place_white = new GameObject[count_white];
        for (int i = 0; i < Y_size; i++)
        {
            for (int a = 0; a < X_size; a++)
            {
                if (All_place[i, a].GetComponent<Place>().Setting_place.Type_place != Type_place.Block)
                {
                    for (int b = 0; b < count_white; b++)
                    {
                        if (Place_white[b] == null)
                        {
                            Place_white[b] = All_place[i, a];
                            break;

                        }
                    }
                }

            }
        }



        //place enemy player with control distance
        Place_player_enemy();


        //rebuild place white
        var new_place_white = new GameObject[count_white - 2];
        for (int i = 0; i < Place_white.Length; i++)
        {
            if (Place_white[i].GetComponent<Place>().Setting_place.Type_place == Type_place.Place)
            {
                for (int a = 0; a < new_place_white.Length; a++)
                {
                    if (new_place_white[a] == null)
                    {
                        new_place_white[a] = Place_white[i];

                        break;
                    }
                }
            }
        }
        for (int i = 0; i < new_place_white.Length; i++)
        {
            new_place_white[i].GetComponent<Place>().Change_Value_Place_sctipt(
                new place_setting
                {
                    All_place = All_place,
                    Type_place = Type_place.Place,
                    pointer_player = pointer_player,
                    pointer_enemy = Pointer_enemy,
                    Color_player = Color_player,
                    Place_for = Place_for.Empity,
                    Color_enemy = Color_enemy
                });
        }
        Place_white = new GameObject[count_white - 2];
        Place_white = new_place_white;


        //pointer setting
        pointer_player.transform.position = Place_player.transform.position;
        Pointer_enemy.transform.position = Place_Enemy.transform.position;
        Pointer_enemy.GetComponent<Pointer_enemy>().Change_value_enemy_pointer(
            new Pointer_enemy.Setting_Enemy
            {
                All_place = All_place,
                Place_player = Place_player,
                place_enemy = Place_Enemy
            });


        //feed to all place
        StartCoroutine(Spawn_turn_for_all_place());
        StartCoroutine(Spawn_turn_to_base());

        //local methods
        void Place_player_enemy()
        {
            int place_player = Random.Range(0, count_white);
            int palce_enemy = Random.Range(0, count_white);

            if (Vector2.Distance(Place_white[place_player].transform.position, Place_white[palce_enemy].transform.position) > 4)
            {
                Place_player = Place_white[place_player];
                Place_Enemy = Place_white[palce_enemy];
                Place_player.GetComponent<Place>().Change_Value_Place_sctipt(
                    new place_setting
                    {
                        All_place = All_place,
                        Type_place = Type_place.Player,
                        Place_for = Place_for.Player,
                        Color_enemy = Color_enemy,
                        Color_player = Color_player,
                        pointer_enemy = Pointer_enemy,
                        pointer_player = pointer_player
                    });
                Place_Enemy.GetComponent<Place>().Change_Value_Place_sctipt(
                    new place_setting { All_place = All_place, Type_place = Type_place.Enemy, Place_for = Place_for.Enemy, pointer_player = pointer_player, pointer_enemy = Pointer_enemy, Color_player = Color_player, Color_enemy = Color_enemy }
                    );
            }
            else
            {
                Place_player_enemy();
            }

        }

    }



    IEnumerator Spawn_turn_for_all_place()
    {
        while (true)
        {
            yield return new WaitForSeconds(20);

            foreach (var item in All_place)
            {

                if (item.GetComponent<Place>().Setting_place.Place_for == Place_for.Player || item.GetComponent<Place>().Setting_place.Place_for == Place_for.Enemy)
                {
                    item.GetComponent<Place>().Count++;
                }
            }
        }
    }

    IEnumerator Spawn_turn_to_base()
    {
        while (true)
        {
            yield return new WaitForSeconds(15);
            Place_player.GetComponent<Place>().Count++;
            Place_Enemy.GetComponent<Place>().Count++;


        }
    }

    public void Reset_pointer_player()
    {
        StopAllCoroutines();

        pointer_player.transform.position = Place_player.transform.position;
    }


}

