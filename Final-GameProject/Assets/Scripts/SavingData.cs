using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class SavingData
{
    public float[] position;
    public int Exp;
    public int Level;
    public SavingData(BasicScript Player) {
        position = new float[2];
        position[0] = Player.transform.position.x;
        position[1] = Player.transform.position.y;
    }
}
