using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class step_5 : MonoBehaviour
{
    public Button BTN_Next;
    public GameObject Step_6;

    public Button BTN_Bomb;
    public Button BTN_Castle;
    public Button BTN_Turret;
    public Button BTN_Spawner;
    public Button BTN_Sniper;
    public Button BTN_Cannon;
    public Button BTN_Trap;
    public Button BTN_Teleport;

    public GameObject Content_Bomb;
    public GameObject Content_Castle;
    public GameObject Content_turret;
    public GameObject Content_spawner;
    public GameObject Content_Sniper;
    public GameObject Content_Cannon;
    public GameObject Content_Trap;
    public GameObject Content_teleport;



    GameObject Current_content;

    void Start()
    {
        Current_content = Content_Bomb;

        BTN_Bomb.onClick.AddListener(() =>
        {
            Current_content.SetActive(false);
            Current_content = Content_Bomb;
            Current_content.SetActive(true);
        });

        BTN_Castle.onClick.AddListener(() =>
        {
            Current_content.SetActive(false);
            Current_content = Content_Castle;
            Current_content.SetActive(true);
        });

        BTN_Turret.onClick.AddListener(() =>
        {
            Current_content.SetActive(false);
            Current_content = Content_turret;
            Current_content.SetActive(true);
        });

        BTN_Spawner.onClick.AddListener(() =>
        {
            Current_content.SetActive(false);
            Current_content = Content_spawner;
            Current_content.SetActive(true);
        });
        BTN_Sniper.onClick.AddListener(() =>
        {
            Current_content.SetActive(false);
            Current_content = Content_Sniper;
            Current_content.SetActive(true);
        });
        BTN_Cannon.onClick.AddListener(() =>
        {
            Current_content.SetActive(false);
            Current_content = Content_Cannon;
            Current_content.SetActive(true);
        });
        BTN_Trap.onClick.AddListener(() =>
        {
            Current_content.SetActive(false);
            Current_content = Content_Trap;
            Current_content.SetActive(true);
        });
        BTN_Teleport.onClick.AddListener(() =>
        {
            Current_content.SetActive(false);
            Current_content = Content_teleport;
            Current_content.SetActive(true);
        });

        BTN_Next.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            Step_6.SetActive(true);

        });
    }


}
