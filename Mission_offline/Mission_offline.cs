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


    Vector2 Start_pos_fild = new Vector2(-2, 2.5f);
    public GameObject[,] All_place;
    public GameObject[] Place_blocks;
    public GameObject[] Place_white;
    [HideInInspector]
    public GameObject Place_player;
    [HideInInspector]
    public GameObject Place_Enemy;


    int lock_move = 0;

    void Start()
    {
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
            Start_pos_fild = new Vector2(-2, Start_pos_fild.y - 0.5f);
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


    private void Update()
    {

        //key control player 
        if (Input.GetKeyDown(KeyCode.W))
        {
            Vector3 Pos_up = new Vector3(pointer_player.transform.position.x, pointer_player.transform.position.y + 0.5f);

            foreach (var item in Places_side_pointer_player)
            {
                if (Pos_up == item.transform.position && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Block)
                {
                    StartCoroutine(Move_pointer(Pos_up, pointer_player, item));
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 pos_down = new Vector3(pointer_player.transform.position.x, pointer_player.transform.position.y - 0.5f);
            foreach (var item in Places_side_pointer_player)
            {
                if (pos_down == item.transform.position && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Block)
                {
                    StartCoroutine(Move_pointer(pos_down, pointer_player, item));
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 pos_right = new Vector3(pointer_player.transform.position.x - 0.5f, pointer_player.transform.position.y);
            foreach (var item in Places_side_pointer_player)
            {
                if (item.transform.position == pos_right && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Block)
                {
                    StartCoroutine(Move_pointer(pos_right, pointer_player, item));
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 pos_left = new Vector3(pointer_player.transform.position.x + 0.5f, pointer_player.transform.position.y);
            foreach (var item in Places_side_pointer_player)
            {
                if (item.transform.position == pos_left && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Block)
                {
                    StartCoroutine(Move_pointer(pos_left, pointer_player, item));
                }
            }
        }
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
                if (Vector2.Distance(item.transform.position, pointer_player.transform.position) <= 0.5f)
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
    IEnumerator Move_pointer(Vector3 postion, GameObject pointer, GameObject place)
    {
        if (lock_move == 0)
        {
            lock_move = 1;
            while (true)
            {
                yield return new WaitForSeconds(0.01f);
                if (pointer.transform.position != postion)
                {
                    pointer.transform.position = Vector3.MoveTowards(pointer.transform.position, postion, 0.07f);

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
