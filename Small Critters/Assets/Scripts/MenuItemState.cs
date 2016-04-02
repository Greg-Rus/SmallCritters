using UnityEngine;
using System.Collections;
using System;

public class MenuItemState {

    public GameObject MenuItem;
    public int menuLevel;
    public Action returnMenu;

    public MenuItemState(GameObject menuItem, int menuLevel, MenuItemState returnMenu)
    {
        
    }
}
