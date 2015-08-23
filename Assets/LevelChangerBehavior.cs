using UnityEngine;
using System.Collections;

public class LevelChangerBehavior : MonoBehaviour {

	public void GotoMainMenu()
	{
		Goto ("MainMenu");
	}

	public void GotoGame()
	{
		Goto ("Game");
	}

	public void GotoGameOver()
	{
		Goto ("GameOver");
	}

	public void GotoVictory()
	{
		Goto ("Victory");
	}

	public void Goto(string levelName)
	{
		Application.LoadLevel(levelName);
	}
}
