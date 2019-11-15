using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Step_4 : MonoBehaviour
{
    public GameObject Step_5;
    public Button BTN_Next;

    public Color Color_enemy;
    public GameObject Pointer_enemy;
    public GameObject[] Place_can_move_enemy;
    public GameObject[] Place_player;

    public GameObject Base_enemey;
    public GameObject Base_player;

    int[] Count_turn_place;
    public int[] Count_turn_player;

    int count_base_player;
    int count_base_enemy;


    Vector3 Distiny_enemy;
    int Count_pointer_enemy = 20;

    private void Start()
    {
        BTN_Next.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            Step_5.SetActive(true);
        });

        //frist place nemey;
        Distiny_enemy = Pointer_enemy.transform.position;

        //start work bot;
        StartCoroutine(pointer_enemy_move_to_place());
        StartCoroutine(Spawn_turn());


        //frist_count_turn
        Count_turn_place = new int[Place_can_move_enemy.Length];
    }


    private void Update()
    {
        //count to text
        Pointer_enemy.GetComponentsInChildren<TextMeshProUGUI>()[1].text = Count_pointer_enemy.ToString();

        //give turn to place
        for (int i = 0; i < Place_can_move_enemy.Length; i++)
        {
            if (Place_can_move_enemy[i].transform.position == Pointer_enemy.transform.position && Count_turn_place[i] <= 0)
            {
                Place_can_move_enemy[i].GetComponent<RawImage>().color = Color_enemy;
                Count_pointer_enemy -= 1;
                Count_turn_place[i] = 1;
            }
        }
        //cheack for show count to palace
        for (int i = 0; i < Place_can_move_enemy.Length; i++)
        {

            Place_can_move_enemy[i].GetComponentInChildren<TextMeshProUGUI>().text = Count_turn_place[i] >= 1 ? Count_turn_place[i].ToString() : "";
        }

        //control place and pointer nenemyh for hide 
        for (int i = 0; i < Place_can_move_enemy.Length; i++)
        {
            Place_can_move_enemy[i].GetComponentInChildren<TextMeshProUGUI>().color = Pointer_enemy.transform.position == Place_can_move_enemy[i].transform.position ? Color_enemy : Color.black;

        }

        //control show text_and count
        for (int i = 0; i < Place_player.Length; i++)
        {
            Place_player[i].GetComponentInChildren<TextMeshProUGUI>().text = Count_turn_player[i].ToString(); ;
        }

        //rqual base player enemy
        Base_player.GetComponentInChildren<TextMeshProUGUI>().text = count_base_player.ToString();
        Base_enemey.GetComponentInChildren<TextMeshProUGUI>().text = count_base_enemy.ToString();


        //control enemy pointer
        if (Pointer_enemy.transform.position.x >= Base_enemey.transform.position.x)
        {
            Pointer_enemy.transform.position = Base_enemey.transform.position;
            Count_pointer_enemy += count_base_enemy;
            count_base_enemy = 0;
            Base_enemey.GetComponentInChildren<TextMeshProUGUI>().color = Color_enemy;
        }
    }

    IEnumerator pointer_enemy_move_to_place()
    {
        while (true)
        {
            Distiny_enemy = new Vector3(Distiny_enemy.x + 1, Distiny_enemy.y, 0);
            yield return new WaitForSeconds(2);

            while (true)
            {
                yield return new WaitForSeconds(0.01f);
                if (Pointer_enemy.transform.position != Distiny_enemy)
                {
                    Pointer_enemy.transform.position = Vector3.MoveTowards(Pointer_enemy.transform.position, Distiny_enemy, 0.2f);

                }
                else
                {
                    break;
                }
            }

        }
    }
    IEnumerator Spawn_turn()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);

            for (int i = 0; i < Count_turn_place.Length; i++)
            {
                Count_turn_place[i] += Count_turn_place[i] >= 1 ? 1 : 0;
            }

            for (int i = 0; i < Count_turn_player.Length; i++)
            {
                Count_turn_player[i] += 1;

            }

            count_base_enemy++;
            count_base_player++;
        }

    }
}
