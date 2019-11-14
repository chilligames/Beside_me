using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Step_3 : MonoBehaviour
{
    public GameObject Pointer;
    public GameObject[] Placess;
    public GameObject Place_Base;
    public Color Color_player;

    public Button BTN_Up;
    public Button BTN_Down;
    public Button BTN_next;

    int Count_base;
    int Count_pointer;

    public int[] count_pos;

    void Start()
    {
        BTN_Up.onClick.AddListener(() =>
        {
            Pointer.transform.position = new Vector3(Pointer.transform.position.x, Pointer.transform.position.y + 1, 0);
        });
        BTN_Down.onClick.AddListener(() =>
        {
            Pointer.transform.position = new Vector3(Pointer.transform.position.x, Pointer.transform.position.y - 1, 0);

        });
        StartCoroutine(Spawn_turn_to_base());

        //countpos
        count_pos = new int[Placess.Length];
    }

    void Update()
    {
        //controll pointer y
        if (Placess[0].transform.position.y <= Pointer.transform.position.y)
        {
            Pointer.transform.position = Placess[0].transform.position;
        }

        //pointer text_count pointer
        Pointer.GetComponentInChildren<TextMeshProUGUI>().text = Count_pointer >= 1 ? Count_pointer.ToString() : "";

        //equal pointer and base
        if (Pointer.transform.position == Place_Base.transform.position)
        {
            Count_pointer += Count_base;
            Count_base = 0;
            Place_Base.GetComponentInChildren<TextMeshProUGUI>().text = Count_base.ToString();
        }

        //work
        for (int i = 0; i < Placess.Length; i++)
        {
            if (Placess[i].transform.position == Pointer.transform.position && Count_pointer >= 1 && count_pos[i] == 0)
            {
                count_pos[i] = 1;
                Count_pointer -= 1;
                Placess[i].GetComponentInChildren<TextMeshProUGUI>().text = count_pos[i].ToString();
                Placess[i].GetComponent<RawImage>().color = Color_player;
                BTN_next.gameObject.SetActive(true);

                print("next");

            }
        }

    }

    IEnumerator Spawn_turn_to_base()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            //turn to base
            Count_base++;
            Place_Base.GetComponentInChildren<TextMeshProUGUI>().text = Count_base.ToString();

            //spawnturn to place player
            for (int i = 0; i < count_pos.Length; i++)
            {
                count_pos[i] += count_pos[i] >= 1 ? 1 : 0;
                Placess[i].GetComponentInChildren<TextMeshProUGUI>().text = count_pos[i] >= 1 ? count_pos[i].ToString() : "";
            }

        }
    }
}
