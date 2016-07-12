using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class LevelData
{
	public int levelLength = 50;
	public int levelWidth = 9;
	public int navigableAreaWidth = 7;
	public int levelTop = 0;
	public ISectionBuilder activeSectionBuilder = null;
	public int newSectionStart = 0;
	public int newSectionEnd = 0;
    public bool emptyRow = false;
    public float coldFogStartRow = -20f;
    public float leftWallX = 0.5f;
    public float rightWallX = 8.5f;
    public float wallWidth = 1f;
}
