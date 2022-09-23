using UnityEngine;
using System.Collections;

public class switchSprite : MonoBehaviour {

	[SerializeField]
	private Sprite angel;
	[SerializeField]
	private Sprite demon;
	private SpriteRenderer rend;

	private void Start()
    {

		rend = GetComponent<SpriteRenderer>();
		rend.sprite = angel;

    }

	// Update is called once per frame
	private void Update () 
	{

        if (Input.GetKeyDown(KeyCode.LeftControl) == true)
		{
			Debug.Log("got in");
			if (rend.sprite == angel)
            {
				rend.sprite = demon;
			}
				
			else
            {
				rend.sprite = angel;
			}
				
		}
	
	}
}
