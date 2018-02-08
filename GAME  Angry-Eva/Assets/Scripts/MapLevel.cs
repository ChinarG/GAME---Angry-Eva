using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// 关卡选择脚本
/// </summary>
public class MapLevel : MonoBehaviour
{
	public  bool         isSelect;      //可选状态
	public  Sprite       LevelSprite;   //替换关卡图
	private Image        LeveLImage;    //UI关卡图组件
	public  GameObject   AllFramePanel; //选关场景界面
	private Button       BackButton;    //返回按钮
	public  GameObject[] Stars;         //星星数组
	public  string       BiaoShiString; //标示
	public  bool         isDevelop;     //是否开发


	private void Awake()
	{
		LeveLImage = GetComponent<Image>();
	}


	void Start()
	{
		BackButton = transform.parent.parent.transform.Find("Map1_BackButton").GetComponent<Button>();
		BackButton.onClick.AddListener(ToFrame); //绑定按钮事件
		BackButton = GetComponent<Button>();
		BackButton.onClick.AddListener(Selected);                 //绑定按钮事件
		if (transform.parent.GetChild(0).name == gameObject.name) //判断是否是第一关
		{
			isSelect = true;
		}
		else //判断前一关的分数
		{
			if (BiaoShiString == "0")
			{
				int beforeNum = transform.GetSiblingIndex() - 1;
				if (PlayerPrefs.GetInt("Level ("            + beforeNum + ")") > 0)
				{
					isSelect = true;
				}
			}
			else if (BiaoShiString != null&& isDevelop)
			{
				int beforeNum = transform.GetSiblingIndex() - 1;
				if (beforeNum < 2) //限定前三个
				{
					if (PlayerPrefs.GetInt("Level (" + BiaoShiString + beforeNum + ")") > 0) //判断并解开后一关关卡
					{
						isSelect = true;
					}
				}
			}
		}


		if (isSelect) //处理关卡显示效果
		{
			LeveLImage.overrideSprite = LevelSprite;               //替换图片
			transform.Find("LevelNum").gameObject.SetActive(true); //开启关卡名
			int num = PlayerPrefs.GetInt(gameObject.name);         //关卡名：对应行星数量
			if (num > 0)                                           //显示几颗星
			{
				for (int i = 0; i < num; i++)
				{
					Stars[i].SetActive(true);
				}
			}
		}
	}


	/// <summary>
	/// 跳转到选关场景界面
	/// </summary>
	public void ToFrame()
	{
		AllFramePanel.SetActive(true);                       //关闭选关界面
		transform.parent.parent.gameObject.SetActive(false); //开启关卡界面
	}


	/// <summary>
	/// 进入关卡
	/// </summary>
	public void Selected()
	{
		if (isSelect)
		{
			PlayerPrefs.SetString("NowLevel", gameObject.name);
			SceneManager.LoadScene(2);
		}
		else
		{
			if (isDevelop==false)//未曾开发
			{
				GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/HintPanel")); //提示页面
				obj.transform.SetParent(GameObject.Find("Canvas").transform);
				obj.transform.localPosition = Vector3.zero;
				Destroy(obj, 2);
			}
		}
	}
}