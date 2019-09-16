using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBoundary
{
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    public ScreenBoundary(GameObject current)
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = current.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
        objectHeight = current.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
    }

    public Vector3 updateTrans(Vector3 pos)
    {
        Vector3 viewPos = pos;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
        pos = viewPos;
        return pos;
    }
    public bool currentPos(Vector3 pos)
    {
        bool ret = false;
        if(pos.x < (screenBounds.x * -1 + objectWidth) || pos.x > screenBounds.x - objectWidth)
        {
            ret = true;
        }
        else if(pos.y < (screenBounds.y * -1 + objectWidth) || pos.y > screenBounds.y - objectWidth)
        {
            ret = true;
        }
        return ret;
    }
}
