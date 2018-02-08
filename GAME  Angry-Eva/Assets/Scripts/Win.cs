using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Win : MonoBehaviour
{
	public void ShowStar()//显示星星
	{
		GameManager.Instance.WinAndShowStar();//调用总控中 胜利方法
	}
}