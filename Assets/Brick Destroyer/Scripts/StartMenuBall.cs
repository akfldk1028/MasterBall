using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrickDestroyer
{
	public class StartMenuBall : MonoBehaviour
	{

		public Material[] ballMaterials;
		public GameObject ballModel;

		void Start()
		{
			ShootTheBall();
		}
		void OnEnable()
		{
			ShootTheBall();
		}

		private void ShootTheBall()
		{
			BallColorAndSprite();
			Time.timeScale = 1;
			int xForce = Random.Range(100, 200);
			int yForce = Random.Range(100, 200);
			GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce, yForce));
		}
		public void BallColorAndSprite()
		{
			ballModel.GetComponent<MeshRenderer>().material = ballMaterials[0];
			if (PlayerPrefs.GetString("selectedBall").Equals("white"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("green"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(0, 255, 44, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("blue"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(0, 128, 255, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("pink"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(251, 0, 255, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("red"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("yellow"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(255, 255, 0, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("brown"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(136, 84, 11, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("silver"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(192, 192, 192, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("aqua"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(0, 255, 255, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("purple"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(128, 0, 128, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("olive"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(128, 128, 0, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("violet"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(138, 43, 226, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("brick"))
			{
				ballModel.GetComponent<MeshRenderer>().material = ballMaterials[1];
				ballModel.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("tiles"))
			{
				ballModel.GetComponent<MeshRenderer>().material = ballMaterials[2];
				ballModel.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("metal"))
			{
				ballModel.GetComponent<MeshRenderer>().material = ballMaterials[3];
				ballModel.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);
			}
		}
	}
}