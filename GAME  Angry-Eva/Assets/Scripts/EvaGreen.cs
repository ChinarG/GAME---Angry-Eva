using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 绿色Eva
/// </summary>
public class EvaGreen : Eva
{
	public override void EvaYellowExpedite()
	{
		base.EvaYellowExpedite();
		Vector3 Speed  = EvaRg.velocity; //给Speed赋值
		Speed.x        *= -1;            //把X向量反向
		EvaRg.velocity =  Speed;         //重新设定方向
	}
}