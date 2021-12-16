using KeepCoding;
using KModkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rnd = UnityEngine.Random;

public class notesScript : MonoBehaviour {

    public KMBombInfo bomb;
    public KMAudio audio;
    public KMBombModule bombModule;
    public KMSelectable Yes;
    public KMSelectable No;
    public TextMesh Note;

    private int _moduleId;
    private static int _moduleIdCounter = 1;
    private bool _moduleSolved;

    private string[] numbers = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
    private string message = "";
    private string originalMessage = "";
    private int yes = 0;
    private int time = 0;

	// Use this for initialization
	void Start () {
        Yes.OnInteract += delegate { YesPress(); return false; };
        No.OnInteract += delegate { NoPress(); return false; };
        _moduleId = _moduleIdCounter++;
        MakeMessage();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void MakeMessage()
    {
        message = "";
        originalMessage = "";
        originalMessage += "press ";
        yes = Rnd.Range(0, 2);
        if(yes == 0)
        {
            originalMessage += "yes ";
        }
        else
        {
            originalMessage += "no ";
        }
        originalMessage += "when last digit of timer is ";
        time = Rnd.Range(0, 9);
        originalMessage += numbers[time];
        message = encrypt(originalMessage);
        message = despace(message);
        
        Debug.LogFormat("[Notes #{0}] Original Message: {1}", _moduleId, originalMessage);
        Debug.LogFormat("[Notes #{0}] Encrypted Message: {1}", _moduleId, message);
        Note.text = message;
    }
    private string encrypt(string s)
    {
        string s2 = s.Replace("a", "ⓘ").Replace("b", "◉").Replace("c", "⊜").Replace("d", "(:)").Replace("e", "⊙")
                .Replace("f", "(☆)").Replace("g", "(▢)").Replace("h", "①").Replace("i", "◯").Replace("j", "②").Replace("k", "(|||)")
                .Replace("l", "(△)").Replace("m", "③").Replace("n", "((□))").Replace("o", "◎").Replace("p", "(,)").Replace("q", "(▲)")
                .Replace("r", "((▵))").Replace("s", "⊝").Replace("t", "④").Replace("u", "(::)").Replace("v", "◐").Replace("w", "㊂")
                .Replace("x", "✪").Replace("y", "(⋯)").Replace("z", "▣");

        return s2;
    }
    private string despace(string s)
    {
        string s2 = s.Replace(" ", "\n");
        return s2;
    }
    void YesPress()
    {
        Yes.AddInteractionPunch();
        if (yes == 0 && Mathf.FloorToInt(bomb.GetTime() % 60) % 10 == time)
        {
            bombModule.HandlePass();
            audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
            _moduleSolved = true;
        }
        else
        {
            bombModule.HandleStrike();
            MakeMessage();
        }
    }

    void NoPress()
    {
        No.AddInteractionPunch();
        if (yes == 1 && Mathf.FloorToInt(bomb.GetTime() % 60) % 10 == time)
        {
            bombModule.HandlePass();
            audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
            _moduleSolved = true;
        }
        else
        {
            bombModule.HandleStrike();
            MakeMessage();
        }
    }
}
