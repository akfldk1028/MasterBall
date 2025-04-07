using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrickDestroyer
{
	public class ChangeLanguage : MonoBehaviour
	{

		public GameObject langButton1;
		public GameObject langButton2;
		public GameObject langButton3;

		public Sprite americanFlag;
		public Sprite italicanFlag;
		public Sprite serbianFlag;

		public GameObject playButton;
		public GameObject achevementsButton;
		public GameObject ballShopButton;

		public GameObject continueButtonPauseMenu;
		public GameObject restartButtonPauseMenu;
		public GameObject exitButtonPauseMenu;

		public GameObject continueButtonGameOverMenu;
		public GameObject restartButtonGameOverMenu;
		public GameObject exitButtonGameOverMenu;

		public GameObject achievementsMenuTittle;
		public GameObject ballShopTittle;

		public GameObject pauseMenuText;
		public GameObject gameoverMenuText;

		public GameObject mainMenuBestScoreText;

		//achevements
		public GameObject destroy100bricks;
		public GameObject destroy1000bricks;
		public GameObject destroy10000bricks;

		public GameObject play10games;
		public GameObject play100games;
		public GameObject play1000games;

		public GameObject collect1000stars;
		public GameObject collect1000balls;
		public GameObject score100points;

		public GameObject unlockFootballBall;
		public GameObject unlockGolfBall;
		public GameObject unlockAllBalls;

		public GameObject whiteBall;
		public GameObject greenBall;
		public GameObject blueBall;

		public GameObject pinkBall;
		public GameObject redBall;
		public GameObject yellowBall;

		public GameObject brownBall;
		public GameObject silverBall;
		public GameObject aquaBall;

		public GameObject purpleBall;
		public GameObject oliveBall;
		public GameObject violetBall;

		public GameObject footballBall;
		public GameObject tennisBall;
		public GameObject baskteballBall;

		private string[] playButtonTranslate = { "PLAY", "GIOCARE", "NOVA IGRA" };
		private string[] achievementsButtonTranslate = { "ACHIEVEMENTS", "REALIZZAZIONI", "MEDALJE" };
		private string[] ballShopButtonTranslate = { "SHOP", "NEGOZIO", "PRODAVNICA" };

		private string[] continueButtonPauseMenuTranslate = { "CONTINUE", "CONTINUA", "NASTAVI" };
		private string[] restartButtonPauseMenuTranslate = { "RESTART", "RICOMINCIA", "RESTARTUJ" };
		private string[] exitButtonPauseMenuTranslate = { "EXIT", "USCITA", "IZLAZ" };

		private string[] continueButtonGameOverMenuTranslate = { "CONTINUE", "CONTINUA", "NASTAVI" };
		private string[] restartButtonGameOverMenuTranslate = { "RESTART", "RICOMINCIA", "RESTARTUJ" };
		private string[] exitButtonGameOverMenuTranslate = { "EXIT", "USCITA", "IZLAZ" };

		private string[] achievementsMenuTittleTranslate = { "ACHIEVEMENTS", "REALIZZAZIONI", "MEDALJE" };
		private string[] ballShopTittleTranslate = { "SHOP", "NEGOZIO", "PRODAVNICA" };

		private string[] pauseMenuTextTranslate = { "PAUSE", "PAUSA", "PAUZA" };
		private string[] gameoverMenuTextTranslate = { "GAME OVER", "GIOCO FINITO", "KRAJ IGRE" };

		private string[] mainMenuBestScoreTextTransalate;

		//achievements 

		private string[] destroy100bricksTranslate = { "Destroy 100 bricks", "Distruggere 100 mattoni", "Uništi 100 cigala" };
		private string[] destroy1000bricksTranslate = { "Destroy 1000 bricks", "Distruggere 1000 mattoni", "Uništi 1000 cigala" };
		private string[] destroy10000bricksTranslate = { "Destroy 10000 bricks", "Distruggere 10000 mattoni", "Uništi 10000 cigala" };

		private string[] play10gamesTranslate = { "Play 10 games", "Gioca 10 partite", "Odigraj 10 partija" };
		private string[] play100gamesTranslate = { "Play 100 games", "Gioca 100 partite", "Odigraj 100 partija" };
		private string[] play1000gamesTranslate = { "Play 1000 games", "Gioca 1000 partite", "Odigraj 1000 partija" };

		private string[] collect1000starsTranslate = { "Collect 1000 stars", "Raccogliere 1000 stelle", "Sakupi 1000 zvezda" };
		private string[] collect1000ballsTranslate = { "Collect 1000 balls", "Raccogliere 1000 palline", "Sakupi 1000 lopti" };
		private string[] score100pointsTranslate = { "Score 100 points", "Segna 100 punti", "Osvoji 100 bodova" };

		private string[] unlockFootballBallTranslate = { "Unlock football ball", "Sbloccare pallone da calcio", "Otključaj fudbalsku loptu" };
		private string[] unlockGolfBallTranslate = { "Unlock golf ball", "Sbloccare la pallina da golf", "Otključaj lopticu za golf" };
		private string[] unlockAllBallsTranslate = { "Unlock all balls", "Sblocca tutte le palle", "Otključaj sve lopte" };

		//ballz shop

		private string[] whiteTranslate = { "white", "bianca", "bela" };
		private string[] greenTranslate = { "green", "verde", "zelena" };
		private string[] blueTranslate = { "blue", "blu", "plava" };

		private string[] pinkTranslate = { "pink", "rosa", "roze" };
		private string[] redTranslate = { "red", "rossa", "crvena" };
		private string[] yellowTranslate = { "yellow", "giallo", "žuta" };

		private string[] brownTranslate = { "brown", "marrone", "braon" };
		private string[] silverTransalte = { "silver", "argento", "srebrna" };
		private string[] aquaTransalte = { "aqua", "acqua", "akva" };

		private string[] purpleTranslate = { "purple", "viola scura", "lila" };
		private string[] oliveTranslate = { "olive", "oliva", "maslinasta" };
		private string[] violetTranslate = { "violet", "viola", "ljubičasta" };

		private string[] footballBallTranslate = { "brick", "mattoni", "cigla" };
		private string[] tennisBallTranslate = { "tiles", "piastrelle", "ploče" };
		private string[] basketballBallTranslate = { "metal", "metallo", "metalna" };

		private AudioSource buttonClickSound;

		void Start()
		{
			string bestUS = "BEST " + PlayerPrefs.GetInt("bestScore").ToString();
			string bestIT = "MIGLIORE " + PlayerPrefs.GetInt("bestScore").ToString();
			string bestSRB = "REKORD " + PlayerPrefs.GetInt("bestScore").ToString();
			mainMenuBestScoreTextTransalate = new string[] { bestUS, bestIT, bestSRB };
			buttonClickSound = GameObject.Find("buttonClickSound").GetComponent<AudioSource>();
			if (PlayerPrefs.GetInt("currentLanguage") == 0)
			{
				Translate(0);
				langButton1.GetComponent<Image>().sprite = americanFlag;
				langButton2.GetComponent<Image>().sprite = italicanFlag;
				langButton3.GetComponent<Image>().sprite = serbianFlag;
			}
			else if (PlayerPrefs.GetInt("currentLanguage") == 1)
			{
				Translate(1);
				langButton1.GetComponent<Image>().sprite = italicanFlag;
				langButton2.GetComponent<Image>().sprite = americanFlag;
				langButton3.GetComponent<Image>().sprite = serbianFlag;
			}
			else if (PlayerPrefs.GetInt("currentLanguage") == 2)
			{
				Translate(2);
				langButton1.GetComponent<Image>().sprite = serbianFlag;
				langButton2.GetComponent<Image>().sprite = italicanFlag;
				langButton3.GetComponent<Image>().sprite = americanFlag;
			}
		}

		//PlayerPrefs.GetInt("currentLanguage") // 1 - American, 2 - Italian, 3 - Serbian
		private void SetAmericanLanguage()
		{
			PlayerPrefs.SetInt("currentLanguage", 0);
			Translate(0);
		}
		private void SetItalianLanguage()
		{
			PlayerPrefs.SetInt("currentLanguage", 1);
			Translate(1);
		}
		private void SetSerbianLanguage()
		{
			PlayerPrefs.SetInt("currentLanguage", 2);
			Translate(2);
		}
		private void Translate(int language)
		{
			playButton.GetComponent<Text>().text = playButtonTranslate[language];
			achevementsButton.GetComponent<Text>().text = achievementsButtonTranslate[language];
			ballShopButton.GetComponent<Text>().text = ballShopButtonTranslate[language];

			continueButtonPauseMenu.GetComponent<Text>().text = continueButtonPauseMenuTranslate[language];
			restartButtonPauseMenu.GetComponent<Text>().text = restartButtonPauseMenuTranslate[language];
			exitButtonPauseMenu.GetComponent<Text>().text = exitButtonPauseMenuTranslate[language];

			continueButtonGameOverMenu.GetComponent<Text>().text = continueButtonGameOverMenuTranslate[language];
			restartButtonGameOverMenu.GetComponent<Text>().text = restartButtonGameOverMenuTranslate[language];
			exitButtonGameOverMenu.GetComponent<Text>().text = exitButtonGameOverMenuTranslate[language];

			achievementsMenuTittle.GetComponent<Text>().text = achievementsMenuTittleTranslate[language];
			ballShopTittle.GetComponent<Text>().text = ballShopTittleTranslate[language];

			pauseMenuText.GetComponent<Text>().text = pauseMenuTextTranslate[language];
			gameoverMenuText.GetComponent<Text>().text = gameoverMenuTextTranslate[language];

			mainMenuBestScoreText.GetComponent<Text>().text = mainMenuBestScoreTextTransalate[language];

			//achievements menu

			destroy100bricks.GetComponent<Text>().text = destroy100bricksTranslate[language];
			destroy1000bricks.GetComponent<Text>().text = destroy1000bricksTranslate[language];
			destroy10000bricks.GetComponent<Text>().text = destroy10000bricksTranslate[language];

			play10games.GetComponent<Text>().text = play10gamesTranslate[language];
			play100games.GetComponent<Text>().text = play100gamesTranslate[language];
			play1000games.GetComponent<Text>().text = play1000gamesTranslate[language];

			collect1000balls.GetComponent<Text>().text = collect1000ballsTranslate[language];
			collect1000stars.GetComponent<Text>().text = collect1000starsTranslate[language];
			score100points.GetComponent<Text>().text = score100pointsTranslate[language];

			unlockFootballBall.GetComponent<Text>().text = unlockFootballBallTranslate[language];
			unlockGolfBall.GetComponent<Text>().text = unlockGolfBallTranslate[language];
			unlockAllBalls.GetComponent<Text>().text = unlockAllBallsTranslate[language];

			//ballz shop menu

			whiteBall.GetComponent<Text>().text = whiteTranslate[language];
			greenBall.GetComponent<Text>().text = greenTranslate[language];
			blueBall.GetComponent<Text>().text = blueTranslate[language];

			pinkBall.GetComponent<Text>().text = pinkTranslate[language];
			redBall.GetComponent<Text>().text = redTranslate[language];
			yellowBall.GetComponent<Text>().text = yellowTranslate[language];

			brownBall.GetComponent<Text>().text = brownTranslate[language];
			silverBall.GetComponent<Text>().text = silverTransalte[language];
			aquaBall.GetComponent<Text>().text = aquaTransalte[language];

			purpleBall.GetComponent<Text>().text = purpleTranslate[language];
			oliveBall.GetComponent<Text>().text = oliveTranslate[language];
			violetBall.GetComponent<Text>().text = violetTranslate[language];

			footballBall.GetComponent<Text>().text = footballBallTranslate[language];
			tennisBall.GetComponent<Text>().text = tennisBallTranslate[language];
			baskteballBall.GetComponent<Text>().text = basketballBallTranslate[language];

		}


		public void ShowOrHideLanguageSprites()
		{
			if (!buttonClickSound.isPlaying)
			{
				buttonClickSound.Play();
			}
			if (langButton2.activeInHierarchy)
			{
				langButton2.SetActive(false);
				langButton3.SetActive(false);
			}
			else
			{
				langButton2.SetActive(true);
				langButton3.SetActive(true);
			}
		}
		public void LanguageButton2()
		{
			ShowOrHideLanguageSprites();
			string flagName = langButton2.GetComponent<Image>().sprite.name;
			if (flagName.Equals("flags_1"))
			{
				buttonClickSound.Play();
				SetItalianLanguage();
				langButton1.GetComponent<Image>().sprite = italicanFlag;
				langButton2.GetComponent<Image>().sprite = americanFlag;
				langButton3.GetComponent<Image>().sprite = serbianFlag;
			}
			else
			{
				buttonClickSound.Play();
				SetAmericanLanguage();
				langButton1.GetComponent<Image>().sprite = americanFlag;
				langButton2.GetComponent<Image>().sprite = italicanFlag;
				langButton3.GetComponent<Image>().sprite = serbianFlag;
			}
		}
		public void LanguageButton3()
		{
			ShowOrHideLanguageSprites();
			string flagName = langButton3.GetComponent<Image>().sprite.name;
			if (flagName.Equals("flags_2"))
			{
				buttonClickSound.Play();
				SetSerbianLanguage();
				langButton1.GetComponent<Image>().sprite = serbianFlag;
				langButton2.GetComponent<Image>().sprite = italicanFlag;
				langButton3.GetComponent<Image>().sprite = americanFlag;
			}
			if (flagName.Equals("flags_0"))
			{
				buttonClickSound.Play();
				SetAmericanLanguage();
				langButton1.GetComponent<Image>().sprite = americanFlag;
				langButton2.GetComponent<Image>().sprite = italicanFlag;
				langButton3.GetComponent<Image>().sprite = serbianFlag;

			}

		}
	}
}