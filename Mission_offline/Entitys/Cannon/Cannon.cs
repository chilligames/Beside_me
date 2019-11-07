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
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.1f, 0.1f, 0), 0.01f);
            if (transform.localScale == new Vector3(0.1f, 0.1f, 0))
            {
                break;
            }
        }

        yield return new WaitForSeconds(1);
        print("spawn");
        Instantiate(Raw_bullet, place_cannon.transform.position, transform.rotation).AddComponent<Bullet>().Chnage_value(new Bullet.Setting_bullet { All_place = Setting.All_place, place_cannon = place_cannon, Target_place_Postion = Finde_target() });//change value palce pos

    }





    class Bullet : MonoBehaviour
    {
        Setting_bullet Setting;

        public void Chnage_value(Setting_bullet setting_Bullet)
        {
            Setting = setting_Bullet;
            StartCoroutine(Start_bullet());
        }



        IEnumerator Start_bullet()
        {
            while (true)
            {

                yield return new WaitForSeconds(0.01f);

                Vector3 diff = Setting.place_cannon.transform.position - transform.position;
                diff.Normalize();
                float rot_z = Mathf.Atan2(diff.y,diff.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, rot_z-180);

                transform.position = Vector3.MoveTowards(transform.position, Setting.Target_place_Postion.transform.position, 0.01f);
            }
        }
        public struct Setting_bullet
        {
            public GameObject Target_place_Postion;
            public GameObject place_cannon;

            public GameObject[,] All_place;

        }
    }



    public struct Setting_Cannon
    {
        public GameObject[,] All_place;
        public int Magezin;
        public Place_for Place_for;
    }

}




