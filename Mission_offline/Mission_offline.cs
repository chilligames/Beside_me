using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Mission_offline : MonoBehaviour
{
    public GameObject Raw_Place;
    public Transform Place_fild;
    public GameObject pointer_player;
    public GameObject Pointer_enemy;

    public int Y_size;//dfult 11
    public int X_size;//defult 9
    [Range(3, 10)]
    public int Deficullty;//defult 5


    Vector2 Start_pos_fild = new Vector2(-2, 2.5f);
    public GameObject[,] Places;
    GameObject[] Place_blocks;
    GameObject[] Place_white; //rebuild after player enemy pick 
    GameObject Place_player;
    GameObject Place_Enemy;

    Vector3 Pointer_pos_next;


    int lock_move = 0;

    void Start()
    {

        //place creator
        Places = new GameObject[Y_size, X_size];
        for (int i = 0; i < Y_size; i++)
        {
            for (int a = 0; a < X_size; a++)
            {
                Places[i, a] = Instantiate(Raw_Place, Place_fild);
                Places[i, a].name = i.ToString() + a.ToString();
                Places[i, a].transform.position = Start_pos_fild;
                Places[i, a].AddComponent<Raw_Place_script>();
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
                    Places[i, a].GetComponent<SpriteRenderer>().color = Color.black;
                    Places[i, a].GetComponent<Raw_Place_script>().Change_Value(Raw_Place_script.Type_place.Block);
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
                if (Places[i, a].GetComponent<Raw_Place_script>().Type == Raw_Place_script.Type_place.Block)
                {
                    for (int b = 0; b < count_block; b++)
                    {
                        if (Place_blocks[b] == null)
                        {
                            Place_blocks[b] = Places[i, a];
                            break;
                        }
                    }
                }
            }

        }


        //pick place null white
        int count_white = Places.Length - count_block;
        Place_white = new GameObject[count_white];
        for (int i = 0; i < Y_size; i++)
        {
            for (int a = 0; a < X_size; a++)
            {
                if (Places[i, a].GetComponent<Raw_Place_script>().Type != Raw_Place_script.Type_place.Block)
                {
                    for (int b = 0; b < count_white; b++)
                    {
                        if (Place_white[b] == null)
                        {
                            Place_white[b] = Places[i, a];
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
            if (Place_white[i].GetComponent<Raw_Place_script>().Type == Raw_Place_script.Type_place.Place)
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
            new_place_white[i].GetComponent<Raw_Place_script>().Change_Value(Raw_Place_script.Type_place.Place);
        }
        Place_white = new GameObject[count_white - 2];
        Place_white = new_place_white;

        //pointer setting
        pointer_player.transform.position = Place_player.transform.position;
        Pointer_enemy.transform.position = Place_Enemy.transform.position;
        pointer_player.AddComponent<Pointer_player>();


        //start turn
        StartCoroutine(Spawn_turn());
        //start turn for place 
        StartCoroutine(Spaw_turn_forall_turn());

        //local method
        void Place_player_enemy()
        {
            int place_player = Random.Range(0, count_white);
            int palce_enemy = Random.Range(0, count_white);

            if (Vector2.Distance(Place_white[place_player].transform.position, Place_white[palce_enemy].transform.position) > 4)
            {
                Place_player = Place_white[place_player];
                Place_Enemy = Place_white[palce_enemy];
                Place_player.GetComponent<Raw_Place_script>().Change_Value(Raw_Place_script.Type_place.Player);
                Place_Enemy.GetComponent<Raw_Place_script>().Change_Value(Raw_Place_script.Type_place.Enemy);
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

            foreach (var item in Places_side_pointer)
            {
                if (Pos_up == item.transform.position && item.GetComponent<Raw_Place_script>().Type != Raw_Place_script.Type_place.Block)
                {
                    StartCoroutine(Move_pointer(Pos_up, pointer_player, item));

                    // give turn to placess
                    if (pointer_player.GetComponent<Pointer_player>().Count > 0)
                    {
                        item.GetComponent<Raw_Place_script>().Pluse_count(1, Raw_Place_script.Type_place.Player);
                        pointer_player.GetComponent<Pointer_player>().Count -= 1;
                    }
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 pos_down = new Vector3(pointer_player.transform.position.x, pointer_player.transform.position.y - 0.5f);
            foreach (var item in Places_side_pointer)
            {
                if (pos_down == item.transform.position && item.GetComponent<Raw_Place_script>().Type != Raw_Place_script.Type_place.Block)
                {
                    StartCoroutine(Move_pointer(pos_down, pointer_player, item));
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 pos_right = new Vector3(pointer_player.transform.position.x - 0.5f, pointer_player.transform.position.y);
            foreach (var item in Places_side_pointer)
            {
                if (item.transform.position == pos_right && item.GetComponent<Raw_Place_script>().Type != Raw_Place_script.Type_place.Block)
                {
                    StartCoroutine(Move_pointer(pos_right, pointer_player, item));
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 pos_left = new Vector3(pointer_player.transform.position.x + 0.5f, pointer_player.transform.position.y);
            foreach (var item in Places_side_pointer)
            {
                if (item.transform.position == pos_left && item.GetComponent<Raw_Place_script>().Type != Raw_Place_script.Type_place.Block)
                {
                    StartCoroutine(Move_pointer(pos_left, pointer_player, item));
                }
            }
        }

        //give turn in base
        if (pointer_player.transform.position == Place_player.transform.position)
        {
            int turn_base_player = Place_player.GetComponent<Raw_Place_script>().Count;
            Place_player.GetComponent<Raw_Place_script>().Count = 0;
            pointer_player.GetComponent<Pointer_player>().Count += turn_base_player;
        }

        //give turn from place
        foreach (var item in Places)
        {
            if (item.GetComponent<Raw_Place_script>().Count >= 2 && pointer_player.transform.position == item.transform.position && item.GetComponent<Raw_Place_script>().Type != Raw_Place_script.Type_place.Enemy)
            {
                var count_place = item.GetComponent<Raw_Place_script>().Count;
                item.GetComponent<Raw_Place_script>().Count -= 1;
                pointer_player.GetComponent<Pointer_player>().Count += count_place - 1;
            }
        }

    }



    GameObject[] Places_side_pointer
    {
        get
        {
            GameObject[] placess = new GameObject[5];
            int count = 0;
            foreach (var item in Places)
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

    IEnumerator Spawn_turn()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f);
            Place_player.GetComponent<Raw_Place_script>().Count++;
            Place_Enemy.GetComponent<Raw_Place_script>().Count++;
        }
    }

    IEnumerator Spaw_turn_forall_turn()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            foreach (var item in Places)
            {
                if (item.GetComponent<Raw_Place_script>().Place_for_ ==Raw_Place_script.Place_for.Player )
                {
                    item.GetComponent<Raw_Place_script>().Count += 1;
                }
            }
        }
    }

    public class Raw_Place_script : MonoBehaviour
    {
        public Type_place Type;
        public Place_for Place_for_;

        TextMeshProUGUI Text_place
        {
            get
            {
                return GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        public int Count;

        public void Change_Value(Type_place Type_block)
        {
            switch (Type_block)
            {
                case Type_place.Place:
                    GetComponentInChildren<RawImage>().gameObject.SetActive(false);
                    Type = Type_block;
                    break;
                case Type_place.Enemy:
                    GetComponentInChildren<RawImage>().gameObject.SetActive(true);
                    GetComponent<SpriteRenderer>().color = Color.red;
                    Type = Type_block;
                    break;
                case Type_place.Player:
                    GetComponent<SpriteRenderer>().color = Color.blue;
                    GetComponentInChildren<RawImage>().gameObject.SetActive(true);
                    Type = Type_block;
                    break;
                case Type_place.Block:
                    GetComponentInChildren<TextMeshProUGUI>().text = "B";
                    GetComponentInChildren<RawImage>().gameObject.SetActive(false);
                    Type = Type_block;
                    break;
            }
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

        private void Update()
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

        public void Pluse_count(int count, Type_place change_for)
        {
            Count += count;

            switch (change_for)
            {
                case Type_place.Player:
                    GetComponent<SpriteRenderer>().color = Color.blue;
                    Place_for_ = Place_for.Player;
                    print("Change color");
                    break;
                case Type_place.Enemy:
                    print("change color");
                    break;
            }

        }


        public enum Type_place
        {
            Place, Player, Block, Enemy
        }

        public enum Place_for
        {
           Empity,Enemy,Player
        }
    }


    public class Pointer_player : MonoBehaviour
    {
        public int Count;
        TextMeshProUGUI Text_count
        {
            get
            {
                return GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        private void Update()
        {
            if (Count > 0)
            {
                Text_count.text = Count.ToString();
            }
            else
            {
                Text_count.text = "";
            }
        }

    }



}
