﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrickDestroyer
{
	public class ObjectPlacement : MonoBehaviour
	{

		public void PlaceNewObjectsOnTheScene()
		{
			// 기준 좌표 정의
			Vector3 basePosition = new Vector3(125.93f, 1.17f, 0.3999996f);

			float positionX = 0;
			int numberOfBricks = 0;
			if (Vars.level < 10)
			{
				numberOfBricks = Random.Range(1, 3);
			}
			else
			{
				numberOfBricks = Random.Range(2, 6);
			}
			HashSet<float> set = new HashSet<float>();
			while (set.Count < numberOfBricks)
			{
				int x = Random.Range(0, 8);
				if (x == 0)
				{
					positionX = -2.55f;
				}
				else if (x == 1)
				{
					positionX = -1.7f;
				}
				else if (x == 2)
				{
					positionX = -0.85f;
				}
				else if (x == 3)
				{
					positionX = 0;
				}
				else if (x == 4)
				{
					positionX = 0.85f;
				}
				else if (x == 5)
				{
					positionX = 1.7f;
				}
				else if (x == 6)
				{
					positionX = 2.55f;
				}
				set.Add(positionX);
			}

			float[] bricksXPosition = new float[numberOfBricks];
			set.CopyTo(bricksXPosition);
			for (int i = 0; i < bricksXPosition.Length; i++)
			{
				int randBonusBall = Random.Range(0, 15); //Probability to spawn bonus ball
				int randStarBonus = Random.Range(0, 25); //Probability to spawn star
				// 계산된 상대 위치
				Vector3 relativePosition = new Vector3(bricksXPosition[i], 5.4f, 0f);
				// 최종 위치 = 기준 좌표 + 상대 위치
				Vector3 finalPosition = basePosition + relativePosition;

				if (randBonusBall == 1)
				{
					Instantiate(Resources.Load<GameObject>("bonusBall"), finalPosition, Quaternion.identity);
				}
				else if (randStarBonus == 1)
				{
					Instantiate(Resources.Load<GameObject>("star"), finalPosition, Quaternion.identity);
				}
				else
				{
					Instantiate(Resources.Load<GameObject>("brick"), finalPosition, Quaternion.identity);
				}
			}
		}
	}
}