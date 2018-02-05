using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCountControl : MonoBehaviour {

	public int LeftOffset = 5;
	public int TopOffset = 5;
	public int SpriteOffset = 5;
	public Sprite KillSprite;
	private int _killCount;
	private int _screenW;
	private int _screenH;
	private int _left;
	private int _top;
	private Vector3 _next;
	private float _scale;
	// Use this for initialization
	void Start () {
		_scale = .09f;
		_killCount = 0;
		RectTransform t = GetComponent<RectTransform>(); 
		_screenW = (int)t.rect.width;
		_screenH = (int)t.rect.height;
		_left = -_screenW / 2 + LeftOffset;
		_top = _screenH / 2 - TopOffset;
		_next = new Vector3 (_left, _top, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.L)) {
			AddKill (1);
		}
	}

	public void AddKill(int kill){
		for (int i = 0; i < kill; i++) {
			_killCount++;
			GameObject NewObj = new GameObject(); //Create the GameObject
			NewObj.transform.SetParent(transform);
			Image NewImage = NewObj.AddComponent<Image>(); //Add the Image Component script
			NewImage.sprite = KillSprite; //Set the Sprite of the Image Component on the new GameObject
			RectTransform t = NewObj.GetComponent<RectTransform>(); 
			Debug.Log(_killCount);
			t.anchoredPosition = new Vector3 (_next.x, _next.y, 0);
			t.localScale = new Vector3 (_scale, _scale, _scale);
			NewObj.SetActive(true); //Activate the GameObject

			_next = new Vector3(_next.x + t.rect.width * _scale + SpriteOffset, _next.y, 0);
			if (_next.x + LeftOffset > -_left + SpriteOffset + t.rect.width * _scale) {
				_next = new Vector3 (_left, _next.y - t.rect.height * _scale - SpriteOffset, 0);
			}
		}
	}
}
