using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_tmp : MonoBehaviour
{

    List<Item> _items;

    private void Awake()
    {
        _items = new List<Item>();
    }


    public void AddItem(Item i)
    {
        _items.Add(i);

    }



}
