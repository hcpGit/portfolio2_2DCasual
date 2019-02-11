using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ds : SingletonMono<ds> {
    public bool clientBorn;
    public Sprite charaSprite;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
