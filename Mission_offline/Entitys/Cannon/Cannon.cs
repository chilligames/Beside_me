using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject Raw_bullet;

    Setting_Cannon Setting;

    GameObject place_cannon;


    public void Change_value_cannon(Setting_Cannon setting_Cannon)
    {
        //change value
        Setting = setting_Cannon;

        //finde place canon
        foreach (var item in Setting.All_place)
        {
            if (Vector3.Distance(item.transform.position, gameObject.transform.position) == 0)
            {
                place_cannon = item;
                break;
            }
        }
        if (place_cannon == null)
        {
            Destroy(gameObject);
        }
        //start cannon shot
        StartCoroutine(Start_cannon_fire());
    }

    /// <summary>
    /// finde target
    /// </summary>
    public GameObject Finde_target()
    {
        var place_for_shot = new GameObject[30];
        var count_place = 0;
        foreach (var item in Setting.All_place)
        {
            if (Vector3.Distance(item.transform.position, transform.position) <= 4)
            {
                for (int i = 0; i < place_for_shot.Length; i++)
                {
                    if (place_for_shot[i] == null)
                    {
                        place_for_shot[i] = item;
                        count_place++;

                        break;
                    }
                }
            }
        }
        var new_pos = new GameObject[count_place];
        foreach (var item in place_for_shot)
        {
            if (item != null)
            {

                for (int i = 0; i < new_pos.Length; i++)
                {
                    if (new_pos[i] == null)
                    {
                        new_pos[i] = item;
                        break;
                    }
                }
            }
        }


        var rand = Random.Range(0, count_place);

        return new_pos[rand];
    }



    IEnumerator Start_cannon_fire()
    {
        //anim start
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.1f, 0.1f, 0), 0.01f);
            if (transform.localScale == new Vector3(0.1f, 0.1f, 0))
            {
                break;
            }
        }

        //job
        while (true)
        {
            if (Setting.Magezin >= 1)
            {
                Instantiate(Raw_bullet, place_cannon.transform.position, transform.rotation).AddComponent<Bullet>().Chnage_value(new Bullet.Setting_bullet { All_place = Setting.All_place, place_cannon = place_cannon, Target_place_Postion = Finde_target(), place_For = Setting.Place_for });//change value palce pos
                Setting.Magezin -= 1;
            }
            else if (Setting.Magezin <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(10);
        }

        //anim destory;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 0.01f);
            if (transform.localScale == Vector3.zero)
            {
                Destroy(gameObject);
            }

        }

    }





    class Bullet : MonoBehaviour
    {

        Setting_bullet Setting;


        GameObject[] place_for_destory;


        public void Chnage_value(Setting_bullet setting_Bullet)
        {
            Setting = setting_Bullet;


            //finde place for destroy
            place_for_destory = new GameObject[5];
            var Count_destroy = 0;
            foreach (var item in Setting.All_place)
            {
                if (Vector3.Distance(item.transform.position, setting_Bullet.Target_place_Postion.transform.position) <= 0.8f)
                {
                    for (int i = 0; i < place_for_destory.Length; i++)
                    {
                        if (place_for_destory[i] == null)
                        {
                            place_for_destory[i] = item;
                            Count_destroy++;
                            break;
                        }
                    }
                }
            }

            var new_place = new GameObject[Count_destroy];
            foreach (var item in place_for_destory)
            {
                if (item != null)
                {
                    for (int i = 0; i < new_place.Length; i++)
                    {
                        if (new_place[i] == null)
                        {
                            new_place[i] = item;
                            break;
                        }
                    }

                }

            }

            place_for_destory = new_place;


            //start job
            StartCoroutine(Start_bullet());
        }

        IEnumerator Start_bullet()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.01f);

                Vector3 diff = Setting.place_cannon.transform.position - transform.position;
                diff.Normalize();
                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, rot_z - 180);

                transform.position = Vector3.MoveTowards(transform.position, Setting.Target_place_Postion.transform.position, 0.01f);

                if (transform.position == Setting.Target_place_Postion.transform.position)
                {
                    break;
                }
            }

            while (true)
            {
                yield return new WaitForSeconds(0.01f);

                transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 0.01f);

                if (transform.localScale == Vector3.zero)
                {
                    foreach (var item in place_for_destory)
                    {
                        switch (Setting.place_For)
                        {
                            case Place_for.Enemy:
                                {
                                    if (item.GetComponent<Place>().Setting_place.Type_place != Type_place.Enemy && item.GetComponent<Place>().Setting_place.Type_place != Type_place.Player && item.GetComponent<Place>().Setting_place.Place_for != Place_for.Enemy)
                                    {
                                        item.GetComponent<Place>().Count = 0;
                                        item.GetComponent<Place>().Setting_place.Place_for = Place_for.Empity;
                                        item.GetComponent<Place>().Setting_place.Type_place = Type_place.Place;
                                    }
                                }
                                break;
                            case Place_for.Player:
                                {
                                    if (item.GetComponent<Place>().Setting_place.Type_place != Type_place.Enemy && item.GetComponent<Place>().Setting_place.Place_for != Place_for.Player && item.GetComponent<Place>().Setting_place.Place_for != Place_for.Player)
                                    {
                                        item.GetComponent<Place>().Count = 0;
                                        item.GetComponent<Place>().Setting_place.Place_for = Place_for.Empity;
                                        item.GetComponent<Place>().Setting_place.Type_place = Type_place.Place;
                                    }
                                }
                                break;
                        }

                    }
                    Destroy(gameObject);
                }
            }

        }
        public struct Setting_bullet
        {
            public GameObject Target_place_Postion;
            public GameObject place_cannon;

            public GameObject[,] All_place;
            public Place_for place_For;

        }
    }



    public struct Setting_Cannon
    {
        public GameObject[,] All_place;
        public int Magezin;
        public Place_for Place_for;
    }

}





