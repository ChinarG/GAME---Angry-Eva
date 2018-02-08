using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 黑Eva类型
/// </summary>
public class EvaBlack : Eva
{
	public List<EvaMum> EvaMumList = new List<EvaMum>(); //敌人集合


	public override void EvaYellowExpedite()
	{
		base.EvaYellowExpedite();
		if (EvaMumList.Count > 0 && EvaMumList != null)
		{
			for (int i = 0; i < EvaMumList.Count; i++)
			{
				EvaMumList[i].Dead(); //死亡
			}
		}
		ClearAction();
	}


	/// <summary>
	/// 碰撞检测
	/// </summary>
	/// <param name="col"></param>
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Enemy")
		{
			EvaMumList.Add(col.GetComponent<EvaMum>());
		}
	}


	/// <summary>
	/// 退出检测
	/// </summary>
	/// <param name="col"></param>
	private void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == "Enemy")
		{
			EvaMumList.Remove(col.GetComponent<EvaMum>());
		}
		
	}


	/// <summary>
	/// 处理爆炸动作
	/// </summary>
	private void ClearAction()
	{
		transform.localScale=new Vector3(5,5,0);
		EvaRg.velocity = Vector3.zero;                                 //速度归零
		myTrail.ClearTrail();                                          //清除轨迹
	}

	
}