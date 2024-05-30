using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LexerTest : MonoBehaviour
{
    void Start()
    {
        string code = @"
            var x = 10;
            if (x > 5) {
                while (x < 20) {
                    x = x + 1;
                }
            }
        ";

        Lexer lexer = new Lexer(code);
        List<Token> tokens = lexer.Tokenize();

        foreach (var token in tokens)
        {
            Debug.Log(token);
        }
    }
}