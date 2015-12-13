using UnityEngine;
using System.Collections;

public class LevelData {
	public int levelLength = 50;
	public int levelWidth = 9;
	public int navigableAreaWidth = 7;
	public int levelTop = 0;
	public ISectionBuilder activeSectionBuilder = null;
	public int newSectionStart = 0;
	public int newSectionEnd = 0;
}
