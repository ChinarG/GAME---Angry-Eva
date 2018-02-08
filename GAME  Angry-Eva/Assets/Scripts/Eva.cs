using UnityEngine;
using System.Collections;


/// <summary>
/// Eva类脚本
/// </summary>
public class Eva : MonoBehaviour
{
	private                  bool           isClick;           //是否点击
	public                   float          MaxDis = 1.8f;     //Eva可拖动最远距离
	[HideInInspector] public SpringJoint2D  EvaSP;             //弹簧链接组件
	protected                Rigidbody2D    EvaRg;             //刚体组件
	public                   LineRenderer   LeftLineRenderer;  //左线组件
	public                   Transform      LeftPos;           //弹弓左定点
	public                   LineRenderer   RightLineRenderer; //右线组件
	public                   Transform      RightPos;          //弹弓右定点
	protected                  GameObject     EvaBoom;           //Eva爆炸特效
	protected                  MyTrail        myTrail;           //定义拖尾脚本对象
	private                  bool           isCanTrail = true; //是否能拖拽
	public                   float          SmoothFlo  = 3;    //平滑度
	public                   AudioClip      SelectEvaClip;     //选中Eva音效
	public                   AudioClip      FlyEvaClip;        //Eva飞出音效
	private                  bool           isFly;             //是否正在飞
	protected                  SpriteRenderer EvaRender;         //Eva渲染组件
	public                   Sprite         HurtSprite;        //受伤图


	private void Awake()
	{
		EvaSP     = GetComponent<SpringJoint2D>(); //获取组件
		EvaRg     = GetComponent<Rigidbody2D>();
		myTrail   = GetComponent<MyTrail>();
		EvaRender = GetComponent<SpriteRenderer>();
	}


	void Start()
	{
		EvaBoom = Resources.Load<GameObject>("Prefabs/EvaMumBoom");
	}


	// Update is called once per frame
	void Update()
	{
		if (isClick)
		{
			transform.position =  Camera.main.ScreenToWorldPoint(Input.mousePosition);  //屏幕坐标转世界
			transform.position += new Vector3(0, 0, -Camera.main.transform.position.z); //第二种方法：同理，加上摄像机的Z轴偏移量 --得正哦
			//transform.position += new Vector3(0,0,10);//第一种方法：既然摄像机在-10方向上，那么Eva就+10

			if (Vector3.Distance(transform.position, RightPos.position) > MaxDis) //如果大于设定距离MaxDis
			{
				Vector3 pos        = (transform.position - RightPos.position).normalized; //单位化向量,求得方向
				pos                *= MaxDis;                                             //赋值最大长度 给向量Pos
				transform.position =  pos + RightPos.position;                            //Eva当前位置赋值：最大距离+起点坐标点的位置
			}
			SlingShort();
		}


		CameraFollow(); //相机跟随

		if (isFly) //如果在飞出的过程中
		{
			if (Input.GetMouseButtonDown(0)) //按下鼠标左键
			{
				EvaYellowExpedite(); //启用黄Eva加速函数
			}
		}
	}


	/// <summary>
	/// 鼠标按下
	/// </summary>
	private void OnMouseDown()
	{
		if (isCanTrail)
		{
			AudioPlay(SelectEvaClip);
			isClick           = true; //点击了
			EvaRg.isKinematic = true; //启动力学
		}
	}


	/// <summary>
	/// 鼠标抬起
	/// </summary>
	private void OnMouseUp()
	{
		if (isCanTrail)
		{
			isClick                   = false; //没点击
			RightLineRenderer.enabled = false; //关闭右划线
			LeftLineRenderer.enabled  = false; //关闭左划线
			EvaRg.isKinematic         = false; //关闭力学
			Invoke("Fly", 0.1f);               //调用函数，（“函数名”，延迟时间）
			isCanTrail = false;
		}
	}


	/// <summary>
	/// 飞出后的处理
	/// </summary>
	private void Fly()
	{
		isFly = true; //正在飞，开始
		AudioPlay(FlyEvaClip);
		EvaSP.enabled = false; //禁用弹簧链接
		Invoke("NextEva", 4);  //2秒后调用 下一个Eva函数
		myTrail.StartTrail();  //开启拖尾
	}


	/// <summary>
	/// 弹弓
	/// </summary>
	private void SlingShort()
	{
		//给弹弓划线
		RightLineRenderer.enabled = true;
		LeftLineRenderer.enabled  = true;
		RightLineRenderer.SetPosition(0, RightPos.position);
		RightLineRenderer.SetPosition(1, transform.position);
		LeftLineRenderer.SetPosition(0, LeftPos.position);
		LeftLineRenderer.SetPosition(1, transform.position);
	}


	/// <summary>
	/// 下一只Eva
	/// </summary>
	protected virtual void NextEva()
	{
		GameManager.Instance.EvaList.Remove(this); //从Eva数组中移除当前Eva
		Destroy(gameObject);
		Instantiate(EvaBoom, transform.position, Quaternion.identity); //实例化特效
		GameManager.Instance.NextEva();                                //调用总控里的下一个判断
	}


	/// <summary>
	/// 碰撞检测
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionEnter2D(Collision2D collision)
	{
		myTrail.ClearTrail(); //清除拖尾
		isFly = false;
		Time.timeScale = 1;
	}


	/// <summary>
	/// 相机在指定范围跟随
	/// </summary>
	private void CameraFollow()
	{
		//记录Eva的横坐标
		float posX = transform.position.x;
		//相机当前位置 = 插值（当前相机位置，目标位置（Mathf.Clamp-限定范围：（限定posX，0，18之间））
		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(Mathf.Clamp(posX, 0, 18), Camera.main.transform.position.y, Camera.main.transform.position.z), SmoothFlo * Time.deltaTime);
	}


	/// <summary>
	/// 播放音效
	/// </summary>
	/// <param name="clip"></param>
	public void AudioPlay(AudioClip clip)
	{
		AudioSource.PlayClipAtPoint(clip, transform.position); //静态方法：播放音效
	}


	/// <summary>
	/// 黄色Eva加速方法
	/// </summary>
	public virtual void EvaYellowExpedite()
	{
		isFly = false;
		AudioPlay(FlyEvaClip);
	}

	/// <summary>
	/// 受伤函数
	/// </summary>
	public void Hurt()
	{
		EvaRender.sprite = HurtSprite;
	}
}