using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lexer
{
    private readonly string _input;
    private int _position;

    private static readonly Dictionary<string, TokenType> Keywords = new Dictionary<string, TokenType>
    {
        { "if", TokenType.Keyword },
        { "while", TokenType.Keyword },
        { "var", TokenType.Keyword },
    };

    private static readonly Dictionary<string, TokenType> Operators = new Dictionary<string, TokenType>
    {
        { "=", TokenType.Assignment },
        { "==", TokenType.Comparison },
        { "!=", TokenType.Comparison },
        { "<", TokenType.Comparison },
        { "<=", TokenType.Comparison },
        { ">", TokenType.Comparison },
        { ">=", TokenType.Comparison },
        { "+", TokenType.Operator },
        { "-", TokenType.Operator },
        { "*", TokenType.Operator },
        { "/", TokenType.Operator }
    };

    public Lexer(string input)
    {
        _input = input;
        _position = 0;
    }

    private char CurrentChar => _position < _input.Length ? _input[_position] : '\0';

    private void Advance() => _position++;

    public List<Token> Tokenize()
    {
        var tokens = new List<Token>();

        while (_position < _input.Length)
        {
            if (char.IsWhiteSpace(CurrentChar))
            {
                Advance();
            }
            else if (char.IsLetter(CurrentChar))
            {
                tokens.Add(ReadIdentifierOrKeyword());
            }
            else if (char.IsDigit(CurrentChar))
            {
                tokens.Add(ReadNumber());
            }
            else if (CurrentChar == '(')
            {
                tokens.Add(new Token(TokenType.LeftParen, CurrentChar.ToString()));
                Advance();
            }
            else if (CurrentChar == ')')
            {
                tokens.Add(new Token(TokenType.RightParen, CurrentChar.ToString()));
                Advance();
            }
            else if (CurrentChar == '{')
            {
                tokens.Add(new Token(TokenType.LeftBrace, CurrentChar.ToString()));
                Advance();
            }
            else if (CurrentChar == '}')
            {
                tokens.Add(new Token(TokenType.RightBrace, CurrentChar.ToString()));
                Advance();
            }
            else if (CurrentChar == ';')
            {
                tokens.Add(new Token(TokenType.Semicolon, CurrentChar.ToString()));
                Advance();
            }
            else if (IsOperatorStart(CurrentChar))
            {
                tokens.Add(ReadOperator());
            }
            else
            {
                Debug.LogError($"Unknown character: {CurrentChar}");
                Advance();
            }
        }

        tokens.Add(new Token(TokenType.EndOfFile, ""));
        return tokens;
    }

    private Token ReadIdentifierOrKeyword()
    {
        var start = _position;

        while (char.IsLetterOrDigit(CurrentChar) || CurrentChar == '_')
        {
            Advance();
        }

        var value = _input.Substring(start, _position - start);

        return Keywords.ContainsKey(value)
            ? new Token(Keywords[value], value)
            : new Token(TokenType.Identifier, value);
    }

    private Token ReadNumber()
    {
        var start = _position;

        while (char.IsDigit(CurrentChar))
        {
            Advance();
        }

        var value = _input.Substring(start, _position - start);
        return new Token(TokenType.Number, value);
    }

    private Token ReadOperator()
    {
        var start = _position;

        while (IsOperatorPart(CurrentChar))
        {
            Advance();
        }

        var value = _input.Substring(start, _position - start);

        if (Operators.ContainsKey(value))
        {
            return new Token(Operators[value], value);
        }

        Debug.LogError($"Unknown operator: {value}");
        return new Token(TokenType.Operator, value);
    }

    private bool IsOperatorStart(char ch)
    {
        return "=!<>+-*/".IndexOf(ch) != -1;
    }

    private bool IsOperatorPart(char ch)
    {
        return "=!<>+-*/".IndexOf(ch) != -1;
    }
}
public enum TokenType
{
    Identifier,
    Keyword,
    Operator,
    Number,
    LeftParen, // (
    RightParen, // )
    LeftBrace, // {
    RightBrace, // }
    Semicolon, // ;
    Assignment, // =
    Comparison, // ==, !=, <, <=, >, >=
    EndOfFile
}

public class Token
{
    public TokenType Type { get; private set; }
    public string Value { get; private set; }

    public Token(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }

    public override string ToString()
    {
        return $"{Type}: {Value}";
    }
}