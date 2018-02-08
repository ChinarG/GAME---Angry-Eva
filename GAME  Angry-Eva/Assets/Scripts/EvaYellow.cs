using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 黄Eva重写脚本
/// </summary>
public class EvaYellow : Eva
{
	public override void EvaYellowExpedite()
	{
		base.EvaYellowExpedite();
		EvaRg.velocity *= 2;//速度2倍
	}
}