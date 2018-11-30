using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour {
    [SerializeField]
    private float scrollSpeed;
    [SerializeField]
    private Pauser pauser;

    [SerializeField]
    private TouchManager.Side side;

    private const float sizeOffset = 0.1f;

    private float bgHeight;
    private TouchManager touchManager;
    private int index;

    // Use this for initialization
    void Start () {
        bgHeight = GetComponent<SpriteRenderer>().bounds.size.y - sizeOffset;
        touchManager = pauser.GetTouchManager;
	}
	
	// Update is called once per frame
	void Update () {
        if (touchManager == null)
            return;

		if(touchManager.HasTouch)
        {
            for (int i = 0; i < touchManager.TouchCount; i++) 
            {
                if (touchManager.side[i] == side)
                {
                    index = i;
                    break;
                }
                if (i == touchManager.TouchCount - 1)
                    return;
            }

            transform.position += new Vector3(0, touchManager.Direction[index].y * scrollSpeed * Time.fixedDeltaTime, 0);
        }
        ChangePos();
    }

    private void ChangePos()
    {
        if (touchManager.Direction[index].y == 0)
            return;

        if(touchManager.Direction[index].y > 0)
        {
            if (transform.position.y >= bgHeight)
                transform.position = new Vector3(transform.position.x, -bgHeight, transform.position.z);
        }
        else
        {
            if (transform.position.y <= -bgHeight)
                transform.position = new Vector3(transform.position.x, bgHeight, transform.position.z);
        }
    }
}
