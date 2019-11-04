using UnityEngine;
using UnityEngine.UI;
using static Mission_offline;

public class UI_mission_offline : MonoBehaviour
{
    public GameObject[,] All_place;

    public GameObject Pointer_player;

    public Button BTN_Bomb;
    public Button BTN_turret;
    public Button BTN_Castel;

    public GameObject Raw_bomb;
    public GameObject Raw_Turret;
    public GameObject Raw_Castel;



    void Start()
    {
        All_place = GetComponentInParent<Mission_offline>().All_place;

        //action builds
        BTN_Bomb.onClick.AddListener(() =>
        {
            if (Pointer_player.GetComponent<Pointer_player>().Count >= 0)
            {
                print("cost_bomb");
                Instantiate(Raw_bomb, Pointer_player.transform.position, transform.rotation).GetComponent<Bomb>().Change_value_bomb(new Bomb.Bomb_setting { All_Place = All_place });
            }

        });

        BTN_Castel.onClick.AddListener(() =>
        {
            print("cost castle");

            Instantiate(Raw_Castel, Pointer_player.transform.position, Pointer_player.transform.rotation).GetComponent<Castel>().Change_value_castel(new Castel.Castel_setting { All_place = All_place, Place_for = Place_for.Player });
        });

        BTN_turret.onClick.AddListener(() =>
        {
            print("cost_turret");
            Instantiate(Raw_Turret, Pointer_player.transform.position, transform.rotation).GetComponent<Turret>().Change_valus_turret(new Turret.Setting_turet { All_place = All_place, Fire_to_ = Place_for.Enemy, magezin = 10 });
        });


    }

}

public enum Type_Build
{
    Bomb, Castle, Turret
}