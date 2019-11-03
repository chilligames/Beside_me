using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Mission_offline : MonoBehaviour
{
    [Header("entity_game")]
    public Color Color_player;
    public Color Color_enemy;
    public GameObject Raw_Place;
    public Transform Place_fild;
    public GameObject pointer_player;
    public GameObject Pointer_enemy;



    [Header("Defend Object")]

    [Header("Attack Object")]
    public GameObject Raw_turret;
    public GameObject raw_Castel;



    [Header("Setting mission")]
    public int Y_size;//dfult 11
    public int X_size;//defult 9
    [Range(3, 10)]
    public int Deficullty;//defult 5


    Vector2 Start_pos_fild = new Vector2(-2, 2.5f);
    public GameObject[,] All_place;
    public GameObject[] Place_blocks;
    GameObject[] Place_white;
    GameObject Place_player;
    GameObject Place_Enemy;


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
                All_place[i, a].AddComponent<Raw_Place_script>();
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
                    All_place[i, a].GetComponent<Raw_Place_script>().Change_Value_Place_sctipt(
                        new Raw_Place_script.Raw_place_setting
                        {
                            All_place = All_place,
                            Color_enemy = Color_enemy,
                            Type_place = Raw_Place_script.Type_place.Block,
                            Place_for = Raw_Place_script.Place_for.Block,
                            Color_player = Color_player,
                            pointer_enemy = Pointer_enemy,
                            pointer_player = pointer_player
                        },
                       new Raw_Place_script.Object_attack_defance
                       {
                           Raw_Turet = Raw_turret,
                           Raw_castel = raw_Castel
                       }
                       );


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
                if (All_place[i, a].GetComponent<Raw_Place_script>().Setting_place.Type_place == Raw_Place_script.Type_place.Block)
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
                if (All_place[i, a].GetComponent<Raw_Place_script>().Setting_place.Type_place != Raw_Place_script.Type_place.Block)
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
            if (Place_white[i].GetComponent<Raw_Place_script>().Setting_place.Type_place == Raw_Place_script.Type_place.Place)
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
            new_place_white[i].GetComponent<Raw_Place_script>().Change_Value_Place_sctipt(
                new Raw_Place_script.Raw_place_setting
                {
                    All_place = All_place,
                    Type_place = Raw_Place_script.Type_place.Place,
                    pointer_player = pointer_player,
                    pointer_enemy = Pointer_enemy,
                    Color_player = Color_player,
                    Place_for = Raw_Place_script.Place_for.Empity,
                    Color_enemy = Color_enemy
                },
                new Raw_Place_script.Object_attack_defance
                {
                    Raw_Turet = Raw_turret,
                    Raw_castel = raw_Castel
                }
                );
        }
        Place_white = new GameObject[count_white - 2];
        Place_white = new_place_white;

        //pointer setting
        pointer_player.transform.position = Place_player.transform.position;
        Pointer_enemy.transform.position = Place_Enemy.transform.position;
        pointer_player.AddComponent<Pointer_player>();
        Pointer_enemy.AddComponent<Pointer_Enemy>().Change_value_enemy_pointer(
            new Pointer_Enemy.Setting_Enemy
            {
                All_place = All_place,
                Place_player = Place_player,
                place_enemy = Place_Enemy
            },
            new Raw_Place_script.Object_attack_defance
            {
                Raw_Turet = Raw_turret,
                Raw_castel = raw_Castel,
            }
            );



        //local methods
        void Place_player_enemy()
        {
            int place_player = Random.Range(0, count_white);
            int palce_enemy = Random.Range(0, count_white);

            if (Vector2.Distance(Place_white[place_player].transform.position, Place_white[palce_enemy].transform.position) > 4)
            {
                Place_player = Place_white[place_player];
                Place_Enemy = Place_white[palce_enemy];
                Place_player.GetComponent<Raw_Place_script>().Change_Value_Place_sctipt(
                    new Raw_Place_script.Raw_place_setting { All_place = All_place, Type_place = Raw_Place_script.Type_place.Player, Place_for = Raw_Place_script.Place_for.Player, Color_enemy = Color_enemy, Color_player = Color_player, pointer_enemy = Pointer_enemy, pointer_player = pointer_player },
                   new Raw_Place_script.Object_attack_defance
                   {
                       Raw_Turet = Raw_turret,
                       Raw_castel = raw_Castel
                   }
                    );
                Place_Enemy.GetComponent<Raw_Place_script>().Change_Value_Place_sctipt(new Raw_Place_script.Raw_place_setting { All_place = All_place, Type_place = Raw_Place_script.Type_place.Enemy, Place_for = Raw_Place_script.Place_for.Enemy, pointer_player = pointer_player, pointer_enemy = Pointer_enemy, Color_player = Color_player, Color_enemy = Color_enemy },
                  new Raw_Place_script.Object_attack_defance
                  {
                      Raw_Turet = Raw_turret,
                      Raw_castel = raw_Castel
                  }
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
                if (Pos_up == item.transform.position && item.GetComponent<Raw_Place_script>().Setting_place.Type_place != Raw_Place_script.Type_place.Block)
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
                if (pos_down == item.transform.position && item.GetComponent<Raw_Place_script>().Setting_place.Type_place != Raw_Place_script.Type_place.Block)
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
                if (item.transform.position == pos_right && item.GetComponent<Raw_Place_script>().Setting_place.Type_place != Raw_Place_script.Type_place.Block)
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
                if (item.transform.position == pos_left && item.GetComponent<Raw_Place_script>().Setting_place.Type_place != Raw_Place_script.Type_place.Block)
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



    public class Raw_Place_script : MonoBehaviour
    {
        //entity places
        public Raw_place_setting Setting_place;
        public Object_attack_defance _Attack_Defance;

        public Type_Build Type_Build;

        GameObject[] Place_inside_place;

        TextMeshProUGUI Text_place
        {
            get
            {
                return GetComponentInChildren<TextMeshProUGUI>();
            }
        }


        Button BTN_place
        {
            get
            {
                return GetComponentInChildren<Button>();
            }
        }

        //place setting
        public int Count;
        int Lock_block_player = 0;
        int Lock_block_enemy = 0;


        //anim entity
        public int Anim_boomb;
        public int Anim_builds;


        public void Change_Value_Place_sctipt(Raw_place_setting Setting, Object_attack_defance _Attack_Defance)
        {
            Setting_place = Setting;
            this._Attack_Defance = _Attack_Defance;

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

            //controller instance for build and defance in place
            switch (Setting.Type_place)
            {
                case Type_place.Place:
                    GetComponentInChildren<RawImage>().gameObject.SetActive(false);
                    BTN_place.onClick.AddListener(() =>
                    {
                        switch (Type_Build)
                        {
                            case Type_Build.Turret:
                                {
                                    foreach (var item in Setting.All_place)
                                    {
                                        if (
                                        item.GetComponent<Raw_Place_script>().Setting_place.Type_place == Type_place.Place
                                        && GetComponentInParent<Mission_offline>().Place_player.GetComponent<Raw_Place_script>().Count >= 2
                                        )
                                        {
                                            Instantiate(this._Attack_Defance.Raw_Turet, transform).GetComponent<Turret>().Change_valus_turret(
                                                new Turret.Setting_turet
                                                {
                                                    magezin = 10,
                                                    Fire_to_ = Place_for.Enemy,
                                                    All_place = Setting.All_place
                                                }
                                                );

                                            //minuse from base
                                            GetComponentInParent<Mission_offline>().Place_player.GetComponent<Raw_Place_script>().Count -= 2;

                                            //cotrol place
                                            Setting_place.Place_for = Place_for.Player;

                                            break;
                                        }
                                        else if (item.GetComponent<Raw_Place_script>().Count <= 0)
                                        {
                                            print("code err no turen");
                                        }
                                    }
                                }
                                break;
                            case Type_Build.Castel:
                                {
                                    Instantiate(_Attack_Defance.Raw_castel, transform).GetComponent<Castel>().Change_value_castel(new Castel.Castel_setting { All_place = Setting_place.All_place, Place_for = Place_for.Player });
                                }
                                break;
                        }
                    });
                    break;
                case Type_place.Enemy:
                    GetComponentInChildren<RawImage>().gameObject.SetActive(true);
                    GetComponent<SpriteRenderer>().color = Setting.Color_enemy;
                    break;
                case Type_place.Player:
                    GetComponent<SpriteRenderer>().color = Setting.Color_player;
                    GetComponentInChildren<RawImage>().gameObject.SetActive(true);
                    break;
                case Type_place.Block:
                    GetComponentInChildren<RawImage>().gameObject.SetActive(false);
                    BTN_place.onClick.AddListener(() =>
                    {
                        if (Anim_boomb == 1 || Anim_boomb == 2)
                        {

                            //disable all animation bomb
                            foreach (var item in GetComponentInParent<Mission_offline>().All_place)
                            {
                                if (item.GetComponent<Raw_Place_script>().Setting_place.Type_place == Type_place.Block)
                                {
                                    item.GetComponentInParent<Raw_Place_script>().Anim_boomb = 3;

                                }
                            }

                            //work
                            Setting_place.Type_place = Type_place.Place;
                            Setting_place.Place_for = Place_for.Empity;
                            Anim_boomb = 3;
                        }
                    });
                    break;
            }
        }

        private void Start()
        {
            //spaw turns
            StartCoroutine(Spawn_turn_tobase());
            StartCoroutine(Spawn_turn_forall_turn());


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
            //text place null if count 0 
            if (Count > 0)
            {
                Text_place.text = Count.ToString();
            }
            else
            {
                Text_place.text = "";
            }

            //color change with place for
            switch (Setting_place.Place_for)
            {
                case Place_for.Empity:
                    GetComponent<SpriteRenderer>().color = Color.white;
                    break;
                case Place_for.Enemy:
                    GetComponent<SpriteRenderer>().color = Setting_place.Color_enemy;
                    break;
                case Place_for.Player:
                    GetComponent<SpriteRenderer>().color = Setting_place.Color_player;
                    break;
                case Place_for.Block:
                    GetComponent<SpriteRenderer>().color = Color.black;
                    break;
            }



            //change place for with 0
            if (Count == 0 && Setting_place.Type_place != Type_place.Block && Setting_place.Type_place != Type_place.Enemy && Setting_place.Type_place != Type_place.Player) //change and cheack for player enemy
            {
                Setting_place.Place_for = Place_for.Empity;
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
                            if (Setting_place.pointer_enemy.GetComponent<Pointer_Enemy>().Count >= 1)
                            {
                                Setting_place.Place_for = Place_for.Enemy;

                                Setting_place.pointer_enemy.GetComponent<Pointer_Enemy>().Count -= 1;
                                Count = 1;
                            }
                            break;
                        case Place_for.Enemy:
                            if (Count > 1)
                            {
                                Setting_place.pointer_enemy.GetComponent<Pointer_Enemy>().Count += Count;
                                Count = 1;
                            }
                            break;
                        case Place_for.Player:
                            Lock_block_enemy = 1;
                            if (Count - Setting_place.pointer_enemy.GetComponent<Pointer_Enemy>().Count >= 0)
                            {
                                Count -= Setting_place.pointer_enemy.GetComponent<Pointer_Enemy>().Count;
                                if (Count == 0)
                                {
                                    Count = 1;
                                    Setting_place.Place_for = Place_for.Enemy;
                                }
                                Setting_place.pointer_enemy.GetComponent<Pointer_Enemy>().Count = 0;
                            }
                            else
                            {
                                var count = Count;
                                Count -= Setting_place.pointer_enemy.GetComponent<Pointer_Enemy>().Count;
                                Count = 1;
                                Setting_place.pointer_enemy.GetComponent<Pointer_Enemy>().Count -= count;
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
                Setting_place.pointer_enemy.GetComponent<Pointer_Enemy>().Count += Count;
                Count = 0;
            }
            else if (Setting_place.pointer_player.transform.position == gameObject.transform.position && Setting_place.Type_place == Type_place.Player)
            {
                Setting_place.pointer_player.GetComponent<Pointer_player>().Count += Count;
                Count = 0;
            }

            //pointer in one place
            if (Setting_place.pointer_player.transform.position == Setting_place.pointer_enemy.transform.position)
            {
                print("minuse pointers");
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
                if ((Count - Setting_place.pointer_enemy.GetComponent<Pointer_Enemy>().Count) < 0)
                {
                    print("losse");
                }
            }


            //anim controll

            //anim Bobms
            if (Setting_place.Type_place == Type_place.Block)
            {
                if (Anim_boomb == 1)
                {
                    if (gameObject.transform.localScale != new Vector3(3.5f, 3.5f, 0))
                    {
                        gameObject.transform.localScale = Vector3.MoveTowards(gameObject.transform.localScale, new Vector3(3.5f, 3.5f, 0), 0.05f);
                    }
                    else
                    {
                        Anim_boomb = 2;
                    }

                }
                else if (Anim_boomb == 2)
                {
                    if (gameObject.transform.localScale != new Vector3(3, 3, 0))
                    {
                        gameObject.transform.localScale = Vector3.MoveTowards(gameObject.transform.localScale, new Vector3(3, 3, 0), 0.05f);
                    }
                    else
                    {
                        Anim_boomb = 1;
                    }
                }
                else if (Anim_boomb == 3)
                {
                    if (gameObject.transform.localScale != new Vector3(3, 3, 0))
                    {

                        gameObject.transform.localScale = Vector3.MoveTowards(gameObject.transform.localScale, new Vector3(3, 3, 0), 0.05f);
                    }
                    else
                    {
                        Anim_boomb = 0;
                        Type_Build = Type_Build.Null;
                    }
                }
            }

            //anim_builds
            if (Setting_place.Type_place == Type_place.Place)
            {
                if (Anim_builds == 1)
                {
                    if (gameObject.transform.localScale != new Vector3(3.5f, 3.5f, 0))
                    {
                        gameObject.transform.localScale = Vector3.MoveTowards(gameObject.transform.localScale, new Vector3(3.5f, 3.5f, 0), 0.01f);

                    }
                    else
                    {
                        Anim_builds = 2;
                    }
                }
                else if (Anim_builds == 2)
                {
                    if (gameObject.transform.localScale != new Vector3(3, 3, 0))
                    {
                        gameObject.transform.localScale = Vector3.MoveTowards(gameObject.transform.localScale, new Vector3(3, 3, 0), 0.01f);
                    }
                    else
                    {
                        Anim_builds = 1;

                    }
                }
                else if (Anim_builds == 3)
                {
                    if (gameObject.transform.localScale != new Vector3(3, 3, 0))
                    {
                        gameObject.transform.localScale = Vector3.MoveTowards(gameObject.transform.localScale, new Vector3(3, 3, 0), 0.01f);
                    }
                    else
                    {
                        Anim_builds = 0;
                        Type_Build = Type_Build.Null;
                    }
                }
            }
        }


        IEnumerator Spawn_turn_tobase()
        {
            while (true)
            {
                yield return new WaitForSeconds(20f);
                if (Setting_place.Type_place == Type_place.Player || Setting_place.Type_place == Type_place.Enemy)
                {
                    Count++;
                }

            }
        }

        IEnumerator Spawn_turn_forall_turn()
        {
            while (true)
            {
                yield return new WaitForSeconds(10);
                if (Setting_place.Place_for == Place_for.Enemy || Setting_place.Place_for == Place_for.Player)
                {
                    Count++;
                }
            }
        }


        public enum Type_place
        {
            Place, Player, Block, Enemy
        }

        public enum Place_for
        {
            Empity, Enemy, Player, Block
        }

        public struct Raw_place_setting
        {
            public Type_place Type_place;
            public Place_for Place_for;
            public Color Color_player;
            public Color Color_enemy;
            public GameObject pointer_player;
            public GameObject pointer_enemy;
            public GameObject[,] All_place;
        }


        public struct Object_attack_defance
        {
            public GameObject Raw_Turet;
            public GameObject Raw_castel;
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


    public class Pointer_Enemy : MonoBehaviour
    {
        //Entity enemy
        Setting_Enemy Enemy_setting;
        Raw_Place_script.Object_attack_defance Attack_Defance;

        GameObject[] Place_Can_move_bot;

        //setting ai
        TextMeshProUGUI Text_Turn
        {
            get
            {
                return GetComponentInChildren<TextMeshProUGUI>();
            }
        }
        GameObject Distiny_pointer;
        GameObject Last_postion;

        public int Count;
        int Atack_time;
        int count_last_postion;
        int lock_move;

        public void Change_value_enemy_pointer(Setting_Enemy setting, Raw_Place_script.Object_attack_defance attack_Defance)
        {
            //cange values
            Enemy_setting = setting;
            Attack_Defance = attack_Defance;

            //convert to arry place;
            Place_Can_move_bot = new GameObject[9];

            int count = 0;
            foreach (var item in Enemy_setting.All_place)
            {
                if (item.GetComponent<Raw_Place_script>().Setting_place.Type_place != Raw_Place_script.Type_place.Block && Vector3.Distance(gameObject.transform.position, item.transform.position) <= 0.7f)
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
                    if (item.GetComponent<Raw_Place_script>().Setting_place.Type_place != Raw_Place_script.Type_place.Enemy)
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
                            if (item.GetComponent<Raw_Place_script>().Setting_place.Place_for == Raw_Place_script.Place_for.Block && Vector3.Distance(item.transform.position, gameObject.transform.position) <= 1)
                            {
                                item.GetComponent<Raw_Place_script>().Setting_place.Type_place = Raw_Place_script.Type_place.Place;
                                item.GetComponent<Raw_Place_script>().Setting_place.Place_for = Raw_Place_script.Place_for.Empity;
                                Enemy_setting.place_enemy.GetComponent<Raw_Place_script>().Count -= 10;
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
                            if (item.GetComponent<Raw_Place_script>().Type_Build == Type_Build.Null && item.GetComponent<Raw_Place_script>().Setting_place.Place_for != Raw_Place_script.Place_for.Player && item.GetComponent<Raw_Place_script>().Setting_place.Place_for == Raw_Place_script.Place_for.Empity && Vector3.Distance(item.transform.position, gameObject.transform.position) <= 1)
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
                                    Fire_to_ = Raw_Place_script.Place_for.Player,
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
                            if (item.GetComponent<Raw_Place_script>().Type_Build == Type_Build.Null && item.GetComponent<Raw_Place_script>().Setting_place.Place_for != Raw_Place_script.Place_for.Player && item.GetComponent<Raw_Place_script>().Setting_place.Place_for == Raw_Place_script.Place_for.Empity && Vector3.Distance(item.transform.position, gameObject.transform.position) <= 1)
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
                            Instantiate(Attack_Defance.Raw_castel, place_build[rand].transform).GetComponent<Castel>().Change_value_castel(new Castel.Castel_setting { All_place = Enemy_setting.All_place, Place_for = Raw_Place_script.Place_for.Enemy });
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
                        if (item.GetComponent<Raw_Place_script>().Setting_place.Place_for == Raw_Place_script.Place_for.Player)
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
                                    if (white.GetComponent<Raw_Place_script>().Setting_place.Place_for == Raw_Place_script.Place_for.Empity)
                                    {
                                        Distiny_pointer = white;
                                    }
                                    else
                                    {
                                        if (Distiny_pointer.GetComponent<Raw_Place_script>().Setting_place.Place_for != Raw_Place_script.Place_for.Empity)
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
                            item.GetComponent<Raw_Place_script>().Setting_place.Place_for == Raw_Place_script.Place_for.Enemy
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
                if (Enemy_setting.place_enemy.GetComponent<Raw_Place_script>().Count >= 0)
                {
                    Builder(Type_Build.Bomb);
                }

                yield return new WaitForSeconds(0.1f);
                if (Enemy_setting.place_enemy.GetComponent<Raw_Place_script>().Count >= 15)
                {
                    Builder(Type_Build.Turret);
                }
                yield return new WaitForSeconds(0.1f);
                if (Enemy_setting.place_enemy.GetComponent<Raw_Place_script>().Count >= 0)
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
                                if (Enemy_setting.place_enemy.GetComponent<Raw_Place_script>().Count >= 2)
                                {
                                    Count += 3;
                                    Enemy_setting.place_enemy.GetComponent<Raw_Place_script>().Count -= 2;
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


    public enum Type_Build
    {
        Null, Bomb, Turret, Castel

    }
}
