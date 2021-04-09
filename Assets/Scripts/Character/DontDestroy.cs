using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    private GameObject[] players;
    private CinemachineConfiner cc;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        cc = gameObject.GetComponentInChildren<CinemachineConfiner>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        try
        {
            cc.m_BoundingShape2D = GameObject.FindWithTag("BoundingBox").GetComponent<PolygonCollider2D>();
        }
        catch (Exception) { }

        players = GameObject.FindGameObjectsWithTag("CharacterObjects");
        if(players.Length > 1)
        {
            Destroy(players[1]);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
