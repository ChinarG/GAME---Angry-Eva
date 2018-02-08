using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// EVA妈妈脚本
/// </summary>
public class EvaMum : MonoBehaviour
{
	public  float          MaxSpeed = 8;//默认最大速度
	public  float          MinSpeed = 3;//默认最小速度
	private SpriteRenderer Render;      //图片
	public  Sprite         HurtSprite;  //受伤图片
	protected GameObject     Boom;  //爆炸特效
	public  GameObject     EvaMumScore; //分数图片
	public  bool           isEvaMum;    //是不是Eva妈妈
	public  AudioClip      EvaHurtClip; //Eva受伤音效
	public  AudioClip      DeadClip;    //销毁音效
	public  AudioClip      HurtClip;    //受伤音效


	private void Awake()
	{
		Render = GetComponent<SpriteRenderer>(); //获取图片渲染组件
	}


	// Use this for initialization
	void Start()
	{
		Boom = Resources.Load<GameObject>("Prefabs/EvaMumBoom");
	}


	/// <summary>
	/// 触发检测
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag=="Eva")
		{
			AudioPlay(EvaHurtClip);
			collision.transform.GetComponent<Eva>().Hurt();//受伤
		}

		if (collision.relativeVelocity.magnitude > MaxSpeed) //如果相对速度.大小>最大速度
		{
			Dead(); //调用死亡消除方法
		}
		else if (collision.relativeVelocity.magnitude > MinSpeed && collision.relativeVelocity.magnitude < MaxSpeed) //相对速度在4-8之间
		{
			Render.sprite = HurtSprite; //更换图片，受伤
			AudioPlay(HurtClip);
		}
	}


	/// <summary>
	/// 死亡消除
	/// </summary>
	public void Dead()
	{
		if (isEvaMum)
		{
			GameManager.Instance.EvaMumList.Remove(this); //移除一个EvaMum
		}
		Destroy(gameObject);                                                                                               //直接死亡
		Instantiate(Boom, transform.position, Quaternion.identity);                                                  //实例化特效
		GameObject scoreobj = Instantiate(EvaMumScore, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity); //实例化分数
		Destroy(scoreobj, 1.5f);
		AudioPlay(DeadClip); //死亡音效
	}


	/// <summary>
	/// 播放音效
	/// </summary>
	/// <param name="clip"></param>
	public void AudioPlay(AudioClip clip)
	{
		AudioSource.PlayClipAtPoint(clip, transform.position); //静态方法：播放音效
	}
}