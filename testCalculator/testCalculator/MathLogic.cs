using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace testCalculator
{
    internal class MathLogic
    {
        public string input = "";
        public string reversePolishNotation = "";
        private Stack operandStack = new Stack();


        public void addSymbol(string symbol)
        {
            //checks what an inputed value is and if it's an operand (and the last symbol was an operand too) replace the last operand with the new one
            if (input.Length > 0)
                if (!(Char.IsDigit(char.Parse(symbol)) || Char.IsDigit(input[input.Length - 1])))
                {
                    string tempInput = input;
                    if(input.Length > 0)
                        input = input.Substring(0, input.Length - 1);
                    input += symbol;
                }
                else
                {
                    input += symbol;
                }
            else
            {
                input += symbol;
            }
        }
        public void performCalculation()
        {
            Queue reversePolishNotationQueue = new Queue();
            //error cases
            if (input.Length == 0)
            {
                input = "0";
                reversePolishNotation = "0";
                return;
            }
            if (!(Char.IsDigit(input[0])) && input[0] != '+' && input[0] != '-')
            {
                input = "error";
                reversePolishNotation = "error";
                return;
            }
            if (!(Char.IsDigit(input[input.Length - 1])))
            {
                input = "error";
                reversePolishNotation = "error";
                return;
            }
            if (input[0] == '+' || input[0] == '-')
            { 
                string input0 = "0";
                input0 += input;
                input = input0;
            }
            //parser
            string newOperand = "";
            string currentType = "none";
            Queue parsedQueue = new Queue();

            for (int i = 0; i < (input.Length); i++)
            {
                if (currentType == "none")
                {
                    if (Char.IsDigit(input[i]))
                    {
                        newOperand = "";
                        newOperand += input[i];
                        currentType = "number";
                    }
                    else
                    {
                        if (input[i] == '.')
                        {
                            input = "error";
                            reversePolishNotation = "error";
                            return;
                        }
                        parsedQueue.Enqueue(input[i]);
                    }
                }
                else
                {
                    if (Char.IsDigit(input[i]) || input[i] == '.')
                    {
                        newOperand += input[i];
                    }
                    else
                    {
                        parsedQueue.Enqueue(newOperand);//(Convert.ToDouble(newOperand));
                        newOperand = "";
                        currentType = "none";
                        parsedQueue.Enqueue(input[i].ToString());
                    }
                }
            }
            if (currentType != "none")
            {
                parsedQueue.Enqueue(newOperand);
            }
            int operandCount = parsedQueue.Count;

            //polish notation logic
            input = "";
            reversePolishNotation = "";
            for (int i = 0; i < operandCount; i++)
            {
                string currentOperandStringified = "";
                currentOperandStringified += parsedQueue.Dequeue();
                if (double.TryParse(currentOperandStringified, out _))//int currentOperand))
                {
                    reversePolishNotation += currentOperandStringified;
                    reversePolishNotation += " ";
                    reversePolishNotationQueue.Enqueue(currentOperandStringified);
                }
                else
                {
                    while (operandStack.Count > 0 && CheckPrecedence(currentOperandStringified) <= CheckPrecedence(operandStack.Peek().ToString()))
                    {
                        reversePolishNotation += operandStack.Peek();
                        reversePolishNotation += " ";
                        reversePolishNotationQueue.Enqueue(operandStack.Pop());

                    }
                    operandStack.Push(currentOperandStringified);
                }
            }
            while (operandStack.Count > 0)
            {
                reversePolishNotation += operandStack.Peek();
                reversePolishNotation += " ";
                reversePolishNotationQueue.Enqueue(operandStack.Pop());
            }

            //math part
            newOperand = "";
            double operator2;
            double operator1;
            while (reversePolishNotationQueue.Count > 0)
            {
                newOperand = reversePolishNotationQueue.Dequeue().ToString();
                if (double.TryParse(newOperand, out _))
                {
                    operandStack.Push(newOperand);
                }
                else 
                {
                    double.TryParse(operandStack.Pop().ToString(), out operator2);
                    double.TryParse(operandStack.Pop().ToString(), out operator1);
                    switch (newOperand)
                    {
                        case "+":
                            newOperand = (operator1 + operator2).ToString();
                            break;
                        case "-":
                            newOperand = (operator1 - operator2).ToString();
                            break;
                        case "/":
                            newOperand = (operator1 / operator2).ToString();
                            break;
                        case "*":
                            newOperand = (operator1 * operator2).ToString();
                            break;
                        case "^":
                            newOperand = (Math.Pow(operator1, operator2)).ToString();
                            break;
                        case "%":
                            newOperand = ((operator2 / operator1)*100).ToString();
                            break;
                        case "√":
                            newOperand = (Math.Pow(operator2, 1.0 / operator1)).ToString();
                            break;
                    }
                    operandStack.Push(newOperand);
                    /* logic for handling "(" and ")":
                    if "(" push it in stack and continue as normal
                    if ")" pop and process operands until you pop a "("*/
                }
            }
            input = operandStack.Pop().ToString();
        }
        int CheckPrecedence(string input)
        {
            if (input == "+" || input == "-")
                return 0;
            if (input == "*" || input == "/")
                return 1;
            if (input == "%" || input == "^" || input == "√")
                return 2;
            if (input == "(" || input == ")")
                return 3;
            return 0;
        }
    }
}
