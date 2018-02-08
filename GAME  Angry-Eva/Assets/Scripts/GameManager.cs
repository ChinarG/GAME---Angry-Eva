using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
	public static GameManager Instance
	{
		get { return instance; }

		set { instance = value; }
	}
	private static GameManager  instance;      //单例
	public         List<Eva>    EvaList;       //Eva数组
	public         List<EvaMum> EvaMumList;    //EvaMum数组
	private        Vector3      OriginPos;     //Eva初始化位置
	public         GameObject   WinPanel;      //胜利游戏面板
	public         GameObject   LosePanel;     //输了游戏面板
	public         GameObject   PausePanel;    //输了游戏面板
	public         GameObject[] Stars;         //星星数组
	private        Button       ListenButton;  //按钮
	private        Animator     PauseAnimator; //暂停动画
	private        int          StarNum;       //星星数量
	private        bool         isPause;       //是否暂停
	public         int          AllStarsNum=3;   //所有星星数量


	void Awake()
	{
		instance = this;
		if (EvaList.Count > 0)
		{
			OriginPos = EvaList[0].transform.position;
		} //如果存在Eva,记录初始化位置
	}


	void Start()
	{
		Initialize(); //调用初始化函数
		StarNum = 0;
	}


	/// <summary>
	/// 初始化函数
	/// </summary>
	private void Initialize()
	{
		for (int i = 0; i < EvaList.Count; i++)
		{
			if (i == 0)
			{
				EvaList[i].transform.position = OriginPos; //给第一个小鸟初始化位置
				EvaList[i].enabled            = true;      //激活第一个Eva脚本
				EvaList[i].EvaSP.enabled      = true;      //激活第一个弹簧链接组件
			}
			else
			{
				EvaList[i].enabled       = false; //关闭所有Eva脚本
				EvaList[i].EvaSP.enabled = false; //关闭所有弹簧链接组件
			}
		}
	}


	/// <summary>
	/// 判断是否启用下一个Eva
	/// </summary>
	public void NextEva()
	{
		if (EvaMumList.Count <= 0) //如果敌人依旧存在
		{
			WinPanel.SetActive(true); //胜利游戏面板
			AddButtonListen("WinRePlay");
			ListenButton.onClick.AddListener(RePlay);
			AddButtonListen("WinMainMenu");
			ListenButton.onClick.AddListener(Home);
		}
		else
		{
			if (EvaList.Count > 0) //如果Eva存在
			{
				Initialize(); //初始化Eva
			}
			else
			{
				LosePanel.SetActive(true); //结束游戏面板
				AddButtonListen("LoseRePlay");
				ListenButton.onClick.AddListener(RePlay);
				AddButtonListen("LoseMainMenu");
				ListenButton.onClick.AddListener(Home);
			}
		}
	}


	/// <summary>
	/// 赢了显示星星
	/// </summary>
	public void WinAndShowStar()
	{
		StartCoroutine(ShowStars()); //开启协成，一个个显示
	}


	/// <summary>
	/// 一个个显示星星协成
	/// </summary>
	/// <returns></returns>
	private IEnumerator ShowStars()
	{
		for (; StarNum < EvaList.Count + 1; StarNum++)
		{
			if (StarNum >= Stars.Length) break; //如果小鸟数量大于星星数量，就跳出：防止越界

			yield return new WaitForSeconds(0.5f);

			Stars[StarNum].SetActive(true); //开启星星
		}
	}


	/// <summary>
	/// 添加按钮事件
	/// </summary>
	private void AddButtonListen(string str)
	{
		ListenButton = GameObject.Find(str).GetComponent<Button>();
	}


	/// <summary>
	/// 重新开始
	/// </summary>
	public void RePlay()
	{
		if (isPause)
		{
			SceneManager.LoadScene(2);
			Time.timeScale = 1;
		}
		else
		{
			SceneManager.LoadScene(2);
			SaveData(); //储存数据
		}
	}


	/// <summary>
	/// 回到主页
	/// </summary>
	public void Home()
	{
		if (isPause)
		{
			SceneManager.LoadScene(1);
		}
		else
		{
			SceneManager.LoadScene(1);
			SaveData(); //储存数据
			Time.timeScale = 1;
		}
	}


	/// <summary>
	/// 暂停游戏
	/// </summary>
	public void PauseGame()
	{
		PausePanel.SetActive(true);
		PauseAnimator = PausePanel.GetComponent<Animator>(); //获取暂停动画机
		PauseAnimator.SetBool("isPause", true);
		AddButtonListen("RePlayButton");
		ListenButton.onClick.AddListener(RePlay);
		AddButtonListen("HomeButton");
		ListenButton.onClick.AddListener(Home);
		AddButtonListen("ContinueButton");
		ListenButton.onClick.AddListener(PauseResume);
		isPause = true; //暂停游戏了
	}


	/// <summary>
	/// 继续游戏
	/// </summary>
	public void PauseResume()
	{
		Time.timeScale = 1;
		PauseAnimator.SetBool("isPause", false);
		isPause = false; //关闭暂停
	}


	/// <summary>
	/// 存储数据
	/// </summary>
	private void SaveData()
	{
		if (StarNum > PlayerPrefs.GetInt(PlayerPrefs.GetString("NowLevel"))) //判断
		{
			PlayerPrefs.SetInt(PlayerPrefs.GetString("NowLevel"), StarNum); //分别设置每个关卡的星星个数
		}
		//所有星星数量相加
		int num = 0;
		for (int i = 0; i <= AllStarsNum; i++)
		{
			num+=PlayerPrefs.GetInt("Level ("+ i + ")");
		}
		PlayerPrefs.SetInt("AllStarNum", num);//在“AllStarNum”中存储总星星数量
	}
}