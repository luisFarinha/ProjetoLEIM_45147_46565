using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectOnEnable : MonoBehaviour
{
    public Button btn;
    public Slider sldr;
    public Toggle tgl;

    public bool canSelect;

    void OnEnable()
    {
        canSelect = true;
    }

    public void Update()
    {
        if (isActiveAndEnabled && canSelect)
        {
            if (btn != null)
            {
                btn.Select();
            }
            else if (sldr != null)
            {
                sldr.Select();
            }
            else if (tgl != null)
            {
                tgl.Select();
            }
            canSelect = false;
        }
    }
}
