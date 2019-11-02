using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    Setting_turet Setting;
    int Anim_turet;


    public void Change_valus(Setting_turet Turet_setting)
    {
        Setting = Turet_setting;

    }
    private void Start()
    {
        StartCoroutine(Active_turet());
    }

    void Update()
    {
        if (Anim_turet == 0)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.02f, 0.02f, 0), 0.001f);
        }
        else if (Anim_turet == 1)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 0.001f);
            if (transform.localScale == Vector3.zero)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator Active_turet()
    {
        while (true)
        {
            Change_valus(Setting); //cheak 

            foreach (var placess in Setting.fire_on_placess)
            {
                if (
                    placess.GetComponent<Mission_offline.Raw_Place_script>().Count >= 1 && Setting.magezin >= 1
                    
                    )
                {
                    Setting.magezin--;
                    placess.GetComponent<Mission_offline.Raw_Place_script>().Count -= 1;
                    break;
                }
                else if (Setting.magezin <= 0)
                {
                    StopCoroutine(Active_turet());
                    placess.GetComponent<Mission_offline.Raw_Place_script>().Type_Build = Mission_offline.Type_Build.Null;
                    placess.GetComponent<Mission_offline.Raw_Place_script>().Setting_place.Place_for = Mission_offline.Raw_Place_script.Place_for.Empity;
                    Anim_turet = 1;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public struct Setting_turet
    {

        public int magezin;

        /// <summary>
        /// turret on place
        /// </summary>
        public GameObject Place;

        /// <summary>
        /// Placess can fire
        /// </summary>
        public GameObject[] fire_on_placess;

        /// <summary>
        /// turret fire enemy or player
        /// </summary>
        public Mission_offline.Raw_Place_script.Place_for Fire_to_;


    }

}