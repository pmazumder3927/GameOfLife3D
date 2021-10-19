using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private CellProperties props;
    public MeshRenderer render;

    private void Awake()
    {
        props = new CellProperties(false);
        render = GetComponent<MeshRenderer>();
        render.enabled = false;
    }

    public void SetProps(CellProperties newProps)
    {
        this.props = newProps;
        render.enabled = newProps.alive;
    }
    public CellProperties GetProps()
    {
        return this.props;
    }
}
