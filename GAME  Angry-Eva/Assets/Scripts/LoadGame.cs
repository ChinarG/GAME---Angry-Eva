using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 开始页面加载关卡
/// </summary>
public class LoadGame : MonoBehaviour
{
	private Button     StartButton;     //开始按钮
	private GameObject StartGameObject; //开始按钮OBJ
	public AudioClip MouseUpClip;


	void Start()
	{
		//Screen.SetResolution(1920, 1080, false);
		StartGameObject = GameObject.Find("Star_Game_Button"); //获取按钮组件
		StartButton     = StartGameObject.GetComponent<Button>();
		StartButton.onClick.AddListener(StartGame); //绑定按钮事件
	}


	/// <summary>
	/// 开始游戏
	/// </summary>
	public void StartGame()
	{
		Invoke("LoadLevelScene", 2); //2秒后调用
		AudioSource.PlayClipAtPoint(MouseUpClip, transform.position);
	}


	/// <summary>
	/// 加载Level场景
	/// </summary>
	private void LoadLevelScene()
	{
		SceneManager.LoadSceneAsync(1); //异步加载关卡
	}


	
}