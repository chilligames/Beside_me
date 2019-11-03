using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Place : MonoBehaviour
{
    //entity places
    public place_setting Setting_place;
    public Object_attack_defance _Attack_Defance;
    public RawImage Image_cron;
    public Color color_player;
    public Color Color_enemy;

    public Type_Build Type_Build;

    GameObject[] Place_inside_place;

    public TextMeshProUGUI Text_place;


    public Button BTN_place;

    //place setting
    public int Count;
    int Lock_block_player = 0;
    int Lock_block_enemy = 0;


    //anim entity
    public int Anim_boomb;
    public int Anim_builds;


    public void Change_Value_Place_sctipt(place_setting Setting, Object_attack_defance _Attack_Defance)
    {
        //change values
        Setting_place = Setting;
        this._Attack_Defance = _Attack_Defance;

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

        //controller instance for build and defance in place
        BTN_place.onClick.RemoveAllListeners();
        switch (Setting_place.Type_place)
        {
            case Type_place.Place:
                Image_cron.gameObject.SetActive(false);
                BTN_place.onClick.AddListener(() =>
                {
                    switch (Type_Build)
                    {
                        case Type_Build.Turret:
                            {
                                foreach (var item in Setting_place.All_place)
                                {
                                    if (
                                    item.GetComponent<Place>().Setting_place.Type_place == Type_place.Place
                                    && GetComponentInParent<Mission_offline>().Place_player.GetComponent<Place>().Count >= 2
                                    )
                                    {
                                        Instantiate(this._Attack_Defance.Raw_Turet, transform).GetComponent<Turret>().Change_valus_turret(
                                            new Turret.Setting_turet
                                            {
                                                magezin = 10,
                                                Fire_to_ = Place_for.Enemy,
                                                All_place = Setting_place.All_place
                                            }
                                            );

                                        //minuse from base
                                        GetComponentInParent<Mission_offline>().Place_player.GetComponent<Place>().Count -= 2;

                                        //cotrol place
                                        Setting_place.Place_for = Place_for.Player;

                                        break;
                                    }
                                    else if (item.GetComponent<Place>().Count <= 0)
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
                Image_cron.gameObject.SetActive(true);
                GetComponent<SpriteRenderer>().color = Color_enemy;
                break;
            case Type_place.Player:
                GetComponent<SpriteRenderer>().color = Setting_place.Color_player;
                Image_cron.gameObject.SetActive(true);
                break;
            case Type_place.Block:
                BTN_place.onClick.AddListener(() =>
                {
                    if (Anim_boomb == 1 || Anim_boomb == 2)
                    {

                        //disable all animation bomb
                        foreach (var item in GetComponentInParent<Mission_offline>().All_place)
                        {
                            if (item.GetComponent<Place>().Setting_place.Type_place == Type_place.Block)
                            {
                                item.GetComponentInParent<Place>().Anim_boomb = 3;

                            }
                        }

                        //work
                        Setting_place.Place_for = Place_for.Empity;
                        Setting_place.Type_place = Type_place.Place;
                        Anim_boomb = 3;
                    }
                });
                break;
        }


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
            if ((Count - Setting_place.pointer_enemy.GetComponent<Pointer_enemy>().Count) < 0)
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


public struct Object_attack_defance
{
    public GameObject Raw_Turet;
    public GameObject Raw_castel;
}

public enum Type_Build
{
    Null, Bomb, Turret, Castel

}
