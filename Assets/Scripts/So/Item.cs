using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Game/Item")]
public class Item : ScriptableObject
{
    [SerializeField] Sprite _sprite;
    [SerializeField] GameObject _prefab;
    [SerializeField] string _name;

    public Sprite Sprite { get => _sprite; }
    public GameObject Prefab { get => _prefab;}
    public string Name { get => _name;}
}
