using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrongAttackGuage : MonoBehaviour
{
    [SerializeField] Attack player;
    public Slider Guage_Bar;
    float max = 2f;

    // Start is called before the first frame update
    void Start()
    {
        Guage_Bar = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        Guage_Bar_Management();
    }

    void Guage_Bar_Management() {
        float Charging_Guage = player.Strong_Attack_Hold_Time;
        Guage_Bar.value = Charging_Guage / max;
    }
}
