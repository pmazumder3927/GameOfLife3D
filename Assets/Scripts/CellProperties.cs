using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellProperties {
    public CellProperties(bool alive)
    {
        this.alive = alive;
    }
    public CellProperties()
    {
        this.alive = false;
    }
    public int age;
    public bool alive;
    public int neighbors;
}
