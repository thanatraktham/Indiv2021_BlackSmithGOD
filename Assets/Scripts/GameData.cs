using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int material_1_total;
    public int material_2_total;
    public int material_3_total;

    public GameData(GameManager gm) {
        material_1_total = gm.material_1_total;
        material_2_total = gm.material_2_total;
        material_3_total = gm.material_3_total;
    }
}
