using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject Mission_offline;

    public Button BTN_singel_play;
    public Button BTN_multi_play;
    public GameObject Dialog;


    int Anim;
    int anim_dialog_multiplay;
    public TMPro.TMP_FontAsset fonts;

    void Start()
    {
        BTN_singel_play.onClick.AddListener(() =>
        {
            Instantiate(Mission_offline);
            Anim = 1;
        });
        BTN_multi_play.onClick.AddListener(() =>
        {
            if (anim_dialog_multiplay != 1)
            {
                anim_dialog_multiplay = 1;
            }
            else if (anim_dialog_multiplay == 1)
            {
                anim_dialog_multiplay = 2;
            }
        });
    }

    public void Update()
    {

        if (Anim == 1)
        {

            foreach (var item in GetComponentsInChildren<Transform>())
            {
                if (item.transform.localScale != Vector3.zero)
                {
                    item.transform.localScale = Vector3.MoveTowards(item.transform.localScale, Vector3.zero, 0.1f);
                }
            }
        }

        if (anim_dialog_multiplay == 1)
        {
            Dialog.transform.localScale = Vector3.MoveTowards(Dialog.transform.localScale, Vector3.one, 0.1f);
        }
        else if (anim_dialog_multiplay == 2)
        {
            Dialog.transform.localScale = Vector3.MoveTowards(Dialog.transform.localScale, Vector3.zero, 0.1f);
        }


        //shadow Image
        foreach (var item in GetComponentsInChildren<Image>())
        {
            if (item.GetComponent<Shadow>())
            {
                item.GetComponent<Shadow>().effectDistance = new Vector2(Input.acceleration.x / 5, Input.acceleration.y / 5);
            }
        }
        foreach (var item in GetComponentsInChildren<RawImage>())
        {
            if (item.GetComponent<Shadow>())
            {
                item.GetComponent<Shadow>().effectDistance = new Vector2(Input.acceleration.x / 5, Input.acceleration.y / 5);
            }
        }
    }

}

