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

    private void Update()
    {
        if (Count > 0)
        {
            Text_count.text = Count.ToString();
        }
        else
        {
            Text_count.text = "";
        }
        //Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, -10), 0.1f);
    }


}
