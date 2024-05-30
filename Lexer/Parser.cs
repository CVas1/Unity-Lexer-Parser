using System;
using System.Collections.Generic;

public class Parser
{
    private readonly List<Token> _tokens;
    private int _position;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
        _position = 0;
    }

    private Token CurrentToken => _position < _tokens.Count ? _tokens[_position] : null;

    private void Advance() => _position++;

    public ProgramNode Parse()
    {
        var program = new ProgramNode();

        while (CurrentToken.Type != TokenType.EndOfFile)
        {
            program.Statements.Add(ParseStatement());
        }

        return program;
    }

    private AstNode ParseStatement()
    {
        if (CurrentToken.Type == TokenType.Keyword && CurrentToken.Value == "var")
        {
            return ParseVarDecl();
        }
        else if (CurrentToken.Type == TokenType.Identifier)
        {
            return ParseAssignment();
        }
        else if (CurrentToken.Type == TokenType.Keyword && CurrentToken.Value == "if")
        {
            return ParseIfStmt();
        }
        else if (CurrentToken.Type == TokenType.Keyword && CurrentToken.Value == "while")
        {
            return ParseWhileStmt();
        }
        else if (CurrentToken.Type == TokenType.LeftBrace)
        {
            return ParseBlock();
        }
        else
        {
            throw new Exception($"Unexpected token: {CurrentToken}");
        }
    }

    private AstNode ParseVarDecl()
    {
        Expect(TokenType.Keyword, "var");
        var identifier = Expect(TokenType.Identifier).Value;
        Expect(TokenType.Assignment, "=");
        var expression = ParseExpression();
        Expect(TokenType.Semicolon, ";");

        return new VarDeclNode(identifier, expression);
    }

    private AstNode ParseAssignment()
    {
        var identifier = Expect(TokenType.Identifier).Value;
        Expect(TokenType.Assignment, "=");
        var expression = ParseExpression();
        Expect(TokenType.Semicolon, ";");

        return new AssignmentNode(identifier, expression);
    }

    private AstNode ParseIfStmt()
    {
        Expect(TokenType.Keyword, "if");
        Expect(TokenType.LeftParen, "(");
        var condition = ParseExpression();
        Expect(TokenType.RightParen, ")");
        var statement = ParseStatement();

        return new IfNode(condition, statement);
    }

    private AstNode ParseWhileStmt()
    {
        Expect(TokenType.Keyword, "while");
        Expect(TokenType.LeftParen, "(");
        var condition = ParseExpression();
        Expect(TokenType.RightParen, ")");
        var statement = ParseStatement();

        return new WhileNode(condition, statement);
    }

    private BlockNode ParseBlock()
    {
        var block = new BlockNode();
        Expect(TokenType.LeftBrace, "{");

        while (CurrentToken.Type != TokenType.RightBrace)
        {
            block.Statements.Add(ParseStatement());
        }

        Expect(TokenType.RightBrace, "}");

        return block;
    }

    private AstNode ParseExpression()
    {
        return ParseTerm();
    }

    private AstNode ParseTerm()
    {
        var node = ParseFactor();

        while (CurrentToken.Type == TokenType.Operator && (CurrentToken.Value == "+" || CurrentToken.Value == "-"))
        {
            var op = CurrentToken.Value;
            Advance();
            var right = ParseFactor();
            node = new BinaryOpNode(node, op, right);
        }

        return node;
    }

    private AstNode ParseFactor()
    {
        var node = ParsePrimary();

        while (CurrentToken.Type == TokenType.Operator && (CurrentToken.Value == "*" || CurrentToken.Value == "/"))
        {
            var op = CurrentToken.Value;
            Advance();
            var right = ParsePrimary();
            node = new BinaryOpNode(node, op, right);
        }

        return node;
    }

    private AstNode ParsePrimary()
    {
        if (CurrentToken.Type == TokenType.Number)
        {
            var value = CurrentToken.Value;
            Advance();
            return new NumberNode(value);
        }
        else if (CurrentToken.Type == TokenType.Identifier)
        {
            var name = CurrentToken.Value;
            Advance();
            return new IdentifierNode(name);
        }
        else if (CurrentToken.Type == TokenType.LeftParen)
        {
            Advance();
            var expression = ParseExpression();
            Expect(TokenType.RightParen, ")");
            return expression;
        }
        else
        {
            throw new Exception($"Unexpected token: {CurrentToken}");
        }
    }

    private Token Expect(TokenType type, string value = null)
    {
        if (CurrentToken.Type != type || (value != null && CurrentToken.Value != value))
        {
            throw new Exception($"Expected token: {type} {value}, but got {CurrentToken.Type} {CurrentToken.Value}");
        }

        var token = CurrentToken;
        Advance();
        return token;
    }
}