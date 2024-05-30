using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Language : MonoBehaviour
{
    static readonly Regex Trimmer = new Regex(@"\s\s+");
    public TMP_InputField inputField;


    private float timer;
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > 0.05)
        {
            OnCodeSubmit();
            timer = 0;
        }
    }

    private string TextNormalizer(string s) => Trimmer.Replace(s, " ").Trim();

    public void OnCodeSubmit()
    {
        print("submit");
        var lines = inputField.text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            LexicalAnalyzer(TextNormalizer(line).Split(' ').ToList());
        }
    }


    private void LexicalAnalyzer(List<string> tokens)
    {
        if (tokens.Count < 1) return;
        print("LexicalAnalyzer");
        switch (tokens[0])
        {
            case "int":
                print("int");
                break;
            case "float":
                print("float");
                break;
            case "string":
                print("string");
                break;
            case "move":
                tokens.RemoveAt(0);
                Move(tokens);
                break;
            case "rotate":
                tokens.RemoveAt(0);
                Rotate(tokens);
                break;
            case "scale":
                tokens.RemoveAt(0);
                Scale(tokens);
                break;
            case "if":
                print("if");
                break;
        }
    }

    private void Move(List<string> list)
    {
        Vector3 v = new Vector3();
        switch (list[0])
        {
            case "forward":
                v = transform.forward;
                break;
            case "backward":
                v = -transform.forward;
                break;
            case "left":
                v = transform.right;
                break;
            case "right":
                v = transform.right;
                break;
            case "up":
                v = transform.up;
                break;
            case "down":
                v = -transform.up;
                break;
        }

        if (list.Count > 1) v *= (float)Convert.ToDouble(list[1]);
        transform.position += v;
    }

    private void Rotate(List<string> list)
    {
        Vector3 v = new Vector3();
        switch (list[0])
        {
            case "forward":
                v = transform.forward;
                break;
            case "backward":
                v = -transform.forward;
                break;
            case "left":
                v = -transform.right;
                break;
            case "right":
                v = transform.right;
                break;
            case "up":
                v = transform.up;
                break;
            case "down":
                v = -transform.up;
                break;
        }
        if (list.Count > 1) v *= (float)Convert.ToDouble(list[1]);
        transform.Rotate(v , Space.Self);
    }
    
    private void Scale(List<string> list)
    {
        float v = 1;
        if (list.Count > 0) v *= (float)Convert.ToDouble(list[0]);
        transform.localScale *= v;
    }
}