using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step_4 : MonoBehaviour
{
    public GameObject Pointer_enemy;

    Vector3 Distiny_enemy;

    private void Start()
    {
        Distiny_enemy = Pointer_enemy.transform.position;

        StartCoroutine(pointer_enemy_move_to_place());
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
}
