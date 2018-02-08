using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 专门管理暂停游戏
/// </summary>
public class PauseGame : MonoBehaviour {

	public void PauseStart()
	{
		Time.timeScale = 0;
	}


	public void PauseEnd()
	{
		GameManager.Instance.PausePanel.SetActive(false);
	}
}
