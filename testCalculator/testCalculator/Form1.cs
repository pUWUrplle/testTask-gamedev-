using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testCalculator
{
    public partial class Form1 : Form
    {
        MathLogic MathLogicObj=new MathLogic();
        public Form1()
        {
            InitializeComponent();
        }

        private void CalculateResult(object sender, EventArgs e)
        {
            MathLogicObj.performCalculation();
            inputFuild.Text = MathLogicObj.input;
            reversePolishNotation.Text = MathLogicObj.reversePolishNotation;
        }

        private void SymbolInput(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            MathLogicObj.addSymbol(button.Text);
            inputFuild.Text = MathLogicObj.input;
        }

        private void clearInput(object sender, EventArgs e)
        {
            MathLogicObj.input = "";
            inputFuild.Text = MathLogicObj.input;
        }

        private void deleteSymbol(object sender, EventArgs e)
        {
            if (MathLogicObj.input.Length>0)
                MathLogicObj.input = MathLogicObj.input.Substring(0, MathLogicObj.input.Length - 1);
            inputFuild.Text = MathLogicObj.input;
        }
    }
}
