using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolishNotationV1
{
    class Program
    {

        static bool IsDigit(char c)
        {
            return (c >= '0' && c <= '9') ? true : false;
        }

        static bool Precedent(char op1,char op2)
        {
            if (op1 == '$')
                return true;
            if (op1 == '*' || op1 == '/')
                return (op2 == '$') ? false : true;
            if (op1 == '+' || op1 == '-')
                return (op2 == '*' || op2 == '/' || op2 == '$') ? false : true;
            return false;
        }

        static double Operate(double op1,double op2,char c)
        {
            switch (c)
            {
                case '+': return (op1 + op2);
                case '-': return (op1 - op2);
                case '*': return (op1 * op2);
                case '/': return (op1 / op2);
                case '$': return (Math.Pow(op1,op2));
            }

            return 0;
        }

        static double EvaluateExpr(string expr)
        {
            ArrayStack<double> doubleStack = new ArrayStack<double>(100);
            string[] items = expr.Split(' ');
            double val, op1, op2;
            char c;

            foreach (string i in items)
            {
                if (int.TryParse(i, out _))
                {
                    doubleStack.Push(Convert.ToDouble(i));
                }
                else
                {
                    op2 = doubleStack.Pop();
                    op1 = doubleStack.Pop();
                    c = Convert.ToChar(i);
                    val = Operate(op1, op2, c);
                    doubleStack.Push(val);
                }
            }
            val = doubleStack.Pop();
            return val;
        }

        static string InfixToPosFix(string infix)
        {
            string postfix = "";
            string help = "";
            char op;
            ArrayStack<char> stack = new ArrayStack<char>(20);
            ArrayStack<double> numStack = new ArrayStack<double>(20);
            foreach(char c in infix)
            {
                if (IsDigit(c))
                {
                    //postfix += c;
                    help += c;
                }else
                {
                    numStack.Push(Convert.ToDouble(help));
                    help = "";
                    postfix += numStack.Pop();
                    postfix += " ";
                    if (!stack.StackEmpty())
                    {
                        op = stack.Peek();
                        while(!stack.StackEmpty() && Precedent(op, c))
                        {
                            op = stack.Pop();
                            postfix += op;
                            postfix += " ";
                            if (!stack.StackEmpty())
                                op = stack.Peek();
                        }
                    }
                    stack.Push(c);
                }
            }
            postfix += help;
            postfix += " ";
            while (!stack.StackEmpty())
            {
                op = stack.Pop();
                postfix += op;
                postfix += " ";

            }


            return postfix;
        }
        static void Main(string[] args)
        {
            string postfix = InfixToPosFix("1+2*3$4/2").TrimEnd();
            double val = EvaluateExpr(postfix);

            Console.WriteLine(val);
        }
    }
}
