using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterBar : MonoBehaviour
{
    private Transform _bar;

    // Start is called before the first frame update
    void Start()
    {
        _bar = transform.Find("Bar");
    }

    public void SetSizeDown(float currentPos)
    {
        if(currentPos >= 0)
        {
            _bar.localScale = new Vector3(currentPos, 1f);
        }
    }

    public void SetSizeUp(float currentPos)
    {

        if (currentPos < 1f)
        {
            _bar.localScale = new Vector3(currentPos, 1f);
        }
    }
}
