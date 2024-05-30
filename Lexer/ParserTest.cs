using System.Collections.Generic;
using UnityEngine;

public class ParserTest : MonoBehaviour
{
    void Start()
    {
        string code = @"
            var x = 10;
            
              x = x + 1;
            
        ";

        Lexer lexer = new Lexer(code);
        List<Token> tokens = lexer.Tokenize();

        Parser parser = new Parser(tokens);
        ProgramNode program = parser.Parse();

        PrintAst(program, "");
    }

    void PrintAst(AstNode node, string indent)
    {
        switch (node)
        {
            case ProgramNode programNode:
                Debug.Log($"{indent}Program");
                foreach (var statement in programNode.Statements)
                {
                    PrintAst(statement, indent + "  ");
                }
                break;
            case VarDeclNode varDeclNode:
                Debug.Log($"{indent}VarDecl: {varDeclNode.Identifier}");
                PrintAst(varDeclNode.Expression, indent + "  ");
                break;
            case AssignmentNode assignmentNode:
                Debug.Log($"{indent}Assignment: {assignmentNode.Identifier}");
                PrintAst(assignmentNode.Expression, indent + "  ");
                break;
            case IfNode ifNode:
                Debug.Log($"{indent}If");
                PrintAst(ifNode.Condition, indent + "  ");
                PrintAst(ifNode.Statement, indent + "  ");
                break;
            case WhileNode whileNode:
                Debug.Log($"{indent}While");
                PrintAst(whileNode.Condition, indent + "  ");
                PrintAst(whileNode.Statement, indent + "  ");
                break;
            case BlockNode blockNode:
                Debug.Log($"{indent}Block");
                foreach (var statement in blockNode.Statements)
                {
                    PrintAst(statement, indent + "  ");
                }
                break;
            case BinaryOpNode binaryOpNode:
                Debug.Log($"{indent}BinaryOp: {binaryOpNode.Operator}");
                PrintAst(binaryOpNode.Left, indent + "  ");
                PrintAst(binaryOpNode.Right, indent + "  ");
                break;
            case NumberNode numberNode:
                Debug.Log($"{indent}Number: {numberNode.Value}");
                break;
            case IdentifierNode identifierNode:
                Debug.Log($"{indent}Identifier: {identifierNode.Name}");
                break;
            default:
                Debug.LogError($"{indent}Unknown node type");
                break;
        }
    }
}