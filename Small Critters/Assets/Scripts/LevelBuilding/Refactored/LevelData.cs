using UnityEngine;
using System.Collections;

public class LevelData {

	public int levelWidth = 9;
	public int levelTop = 0;
	public ISectionBuilder activeSectionBuilder = null;
	public int newSectionStart = 0;
	public int newSectionEnd = 0;
}
