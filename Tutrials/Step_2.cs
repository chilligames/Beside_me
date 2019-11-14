using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Step_2 : MonoBehaviour
{
    public GameObject Pointer;
    public TextMeshProUGUI Text_pointer_number;

    public GameObject @base;
    public Button BTN_Right;
    public Button BTN_left;
    public Button BTN_next;

    Vector3 Frist_location;

    int Count_turn;

    private void Start()
    {
        Frist_location = Pointer.transform.position;
        BTN_Right.onClick.AddListener(() =>
        {
            Pointer.transform.position = new Vector3(Pointer.transform.position.x + 1, Pointer.transform.position.y, 0);
        });


        BTN_left.onClick.AddListener(() =>
        {
            Pointer.transform.position = new Vector3(Pointer.transform.position.x - 1, Pointer.transform.position.y, 0);
        });

        StartCoroutine(Spawn_turn_tu_base());
    }
    void Update()
    {
        //cheack pointer less frist location
        if (Pointer.transform.position.x <= Frist_location.x)
        {
            Pointer.transform.position = Frist_location;
        }

        //cheack active misison and next mission
        if (Pointer.transform.position.x >= @base.transform.position.x)
        {
            Pointer.transform.position = @base.transform.position;
            StopAllCoroutines();

            Text_pointer_number.text = Count_turn.ToString();
            @base.GetComponentInChildren<TextMeshProUGUI>().text = "";


            BTN_next.gameObject.SetActive(true);
        }
    }
    IEnumerator Spawn_turn_tu_base()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            Count_turn++;
            @base.GetComponentInChildren<TextMeshProUGUI>().text = Count_turn.ToString();
        }
    }

}
