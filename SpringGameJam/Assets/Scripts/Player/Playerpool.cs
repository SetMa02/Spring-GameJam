using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerCorpsePool : MonoBehaviour 
{
    [SerializeField] private int _maxPlayerCorpses = 5;
    [SerializeField] private GameObject _playerCorpseTemplate;

    private List<GameObject> _playerCorpses;
    [SerializeField]private Player _player;
    private int _activeCorpseCount = 0;

    private void Start()
    {
        _player = gameObject.GetComponent<Player>();
        _playerCorpses = new List<GameObject>();
        PoolInit();
    }

    private void OnEnable()
    {
        _player.DieInit += SpawnCorpse;
    }

    private void OnDisable()
    {
        _player.DieInit -= SpawnCorpse;
    }

    private void PoolInit()
    {
        for (int i = 0; i < _maxPlayerCorpses; i++)
        {
            GameObject corpse = Instantiate(_playerCorpseTemplate);
            corpse.SetActive(false);
            _playerCorpses.Add(corpse);
        }
    }

    private void SpawnCorpse()
    {
        GameObject corpseToSpawn = null;
        
        for (int i = 0; i < _playerCorpses.Count; i++)
        {
            if (!_playerCorpses[i].activeInHierarchy)
            {
                corpseToSpawn = _playerCorpses[i];
                break; 
            }
        }
        
        if (corpseToSpawn == null)
        {
            corpseToSpawn = _playerCorpses[0];
            Debug.Log("Пул трупов полон, возвращаем самый старый труп в пул и спавним новый.");
        }
        
        if (corpseToSpawn != null)
        {
            corpseToSpawn.transform.position = transform.position; 
            corpseToSpawn.transform.rotation = transform.rotation;   
            corpseToSpawn.SetActive(true); 
            
            _playerCorpses.Remove(corpseToSpawn);
            _playerCorpses.Add(corpseToSpawn);
        }
    }
}
