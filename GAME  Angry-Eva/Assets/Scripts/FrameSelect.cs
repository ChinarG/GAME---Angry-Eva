using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FrameSelect : MonoBehaviour
{
	public  int        StarNum;           //行星数量
	public  bool       isSelect;          //是否可选
	public  GameObject LocksGameObject;   //枷锁
	public  GameObject StarsGameObject;   //星星
	public  GameObject AllFramePanel;     //所有选关界面
	public  GameObject MapLevelPanel;     //关卡界面
	private Button     SelectButton;      //按钮组件
	public  Text       LevelStarsText;    //关卡星星数量
	public  int        LevelStarNum  = 0; //关卡开始数量
	public  int        LevelEndCount = 2; //关卡结束数量



	void Start()
	{
		//PlayerPrefs.DeleteAll();

		SelectButton = GetComponent<Button>();
		SelectButton.onClick.AddListener(ToMapLevel); //绑定选关按钮事件

		SelectButton = GameObject.Find("ExitGameButton").GetComponent<Button>();
		SelectButton.onClick.AddListener(ExitGame); //绑定退出游戏按钮事件

		SelectButton = GameObject.Find("BackStartPanelButton").GetComponent<Button>();
		SelectButton.onClick.AddListener(BackStarPanel); //绑定按钮事件

		if (PlayerPrefs.GetInt("AllStarNum", 0) >= StarNum) //声明一个所有星星数量>0
		{
			isSelect = true; //可以选择
		}
		if (isSelect)
		{
			LocksGameObject.SetActive(false); //关闭枷锁
			StarsGameObject.SetActive(true);  //开启星星计数
			//显示星星
			int counts = 0; //用来计数关卡的总星星个数
			for (int i = LevelStarNum; i <= LevelEndCount; i++)
			{
				counts += PlayerPrefs.GetInt("Level (" + i + ")", 0);
			}
			LevelStarsText.text = counts + "/9";
		}
	}


	/// <summary>
	/// 跳转到关卡界面
	/// </summary>
	public void ToMapLevel()
	{
		if (isSelect)
		{
			AllFramePanel.SetActive(false); //关闭选关界面
			MapLevelPanel.SetActive(true);  //开启关卡界面
		}
	}


	/// <summary>
	/// 退出游戏
	/// </summary>
	public void ExitGame()
	{
		Application.Quit();
	}


	/// <summary>
	/// 返回开始界面
	/// </summary>
	public void BackStarPanel()
	{
		SceneManager.LoadScene(0);
	}
}