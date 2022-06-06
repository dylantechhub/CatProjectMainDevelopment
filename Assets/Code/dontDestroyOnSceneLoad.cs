using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class dontDestroyOnSceneLoad : MonoBehaviour
{
	[Header("Object to keep alive")]
	public GameObject notDestroyedGameObject;

    private void Start()
    {
        keepAlive();
    }
    public void keepAlive()
	{
		DontDestroyOnLoad( notDestroyedGameObject.gameObject );
	}
}
