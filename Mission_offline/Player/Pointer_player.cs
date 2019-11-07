using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pointer_player : MonoBehaviour
{

    public GameObject UI;
    public int Count;
    public TextMeshProUGUI Text_count;

    public GameObject[,] All_place;
    private void Start()
    {
        All_place = GetComponentInParent<Mission_offline>().All_place;
    }

    private void Update()
    {
        foreach (var item in All_place)
        {
            if (item.transform.position == gameObject.transform.position)
            {
                item.GetComponent<Place>().Up_from_pointers();
            }
        }
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
