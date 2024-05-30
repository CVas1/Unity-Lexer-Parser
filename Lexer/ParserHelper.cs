using System.Collections.Generic;

public abstract class AstNode
{
    // Base class for all AST nodes
}

public class ProgramNode : AstNode
{
    public List<AstNode> Statements { get; } = new List<AstNode>();
}

public class VarDeclNode : AstNode
{
    public string Identifier { get; }
    public AstNode Expression { get; }

    public VarDeclNode(string identifier, AstNode expression)
    {
        Identifier = identifier;
        Expression = expression;
    }
}

public class AssignmentNode : AstNode
{
    public string Identifier { get; }
    public AstNode Expression { get; }

    public AssignmentNode(string identifier, AstNode expression)
    {
        Identifier = identifier;
        Expression = expression;
    }
}

public class IfNode : AstNode
{
    public AstNode Condition { get; }
    public AstNode Statement { get; }

    public IfNode(AstNode condition, AstNode statement)
    {
        Condition = condition;
        Statement = statement;
    }
}

public class WhileNode : AstNode
{
    public AstNode Condition { get; }
    public AstNode Statement { get; }

    public WhileNode(AstNode condition, AstNode statement)
    {
        Condition = condition;
        Statement = statement;
    }
}

public class BlockNode : AstNode
{
    public List<AstNode> Statements { get; } = new List<AstNode>();
}

public class BinaryOpNode : AstNode
{
    public AstNode Left { get; }
    public string Operator { get; }
    public AstNode Right { get; }

    public BinaryOpNode(AstNode left, string op, AstNode right)
    {
        Left = left;
        Operator = op;
        Right = right;
    }
}

public class NumberNode : AstNode
{
    public string Value { get; }

    public NumberNode(string value)
    {
        Value = value;
    }
}

public class IdentifierNode : AstNode
{
    public string Name { get; }

    public IdentifierNode(string name)
    {
        Name = name;
    }
}