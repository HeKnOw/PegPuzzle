using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pegPuzzle
{
    public partial class Form1 : Form
    {
        public NumericUpDown numInput;
        public decimal n = 0;
        public int step = 0;
        NumericUpDown nudRow = new NumericUpDown();
        NumericUpDown nudPeg = new NumericUpDown();
        public Button action = new Button();
        Dictionary<int, List<int>> startState = new Dictionary<int, List<int>>();
        Dictionary<int, List<int>> endState = new Dictionary<int, List<int>>();
        Label rowLabel = new Label();
        Label pegLabel = new Label();
        Puzzle triangle;

        public Form1()
        {
            InitializeComponent();
            this.label2.MaximumSize = new Size(this.Width, 0);
            this.label2.AutoSize = true;
            this.label2.Text = "";
            this.instructionLabel.Text = "Pick how many rows should the triangle have, each row will have the respective amount of pegs";
            this.panel1.VerticalScroll.Enabled = false;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numInput = (NumericUpDown)sender;
            n = numInput.Value;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (step == 0)
            {
                step = 1;

                this.instructionLabel.Text = "Pick pegs to remove from triangle, starting from the top to the bottom of the triangle";
                this.label1.Text = "Add a starting state for your puzzle";
                this.numericUpDown1.Visible = false;
                rowLabel.Text = "row";
                pegLabel.Text = "peg";
                nudRow.Maximum = n;
                nudRow.Minimum = 1;
                nudPeg.Minimum = 1;
                nudPeg.Maximum = nudRow.Value;
                nudRow.ValueChanged += nudRow_ValueChanged;
                action.Text = "Remove";
                action.Left = this.numericUpDown1.Left;
                action.Top = this.numericUpDown1.Top;
                action.Click += remove_Click;
                nudRow.Top = this.numericUpDown1.Top;
                nudPeg.Top = this.numericUpDown1.Top;
                nudRow.Left = action.Left + action.Width + 5;
                nudPeg.Left = nudRow.Left + nudRow.Width + 5;
                rowLabel.Left = nudRow.Left;
                pegLabel.Left = nudPeg.Left;
                rowLabel.Top = nudRow.Top - nudRow.Height;
                pegLabel.Top = nudPeg.Top - nudPeg.Height;
                this.Controls.Add(action);
                this.Controls.Add(nudRow);
                this.Controls.Add(nudPeg);
                this.Controls.Add(rowLabel);
                this.Controls.Add(pegLabel);
                this.Controls.Remove(numericUpDown1);
                this.label2.Text = "Pegs to remove: ";

            }
            else if(step == 1)
            {
                step = 2;

                this.instructionLabel.Text = "Pick pegs you want remaining in triangle, starting from the top to the bottom of the triangle";
                this.label1.Text = "What is the finishing state for your puzzle";
                action.Text = "Add";
                //this.label2.Text = "Pegs remaining: ";
                triangle = new Puzzle((int)n, startState, this);
                this.label2.Text = triangle.printBoard(triangle.board);
                
            }
            else if(step == 2)
            {
                //heres where we run the program
                step = 3;

                this.label2.Text = "";
                this.Controls.Remove(action);
                this.Controls.Remove(nudPeg);
                this.Controls.Remove(nudRow);
                this.Controls.Remove(rowLabel);
                this.Controls.Remove(pegLabel);
                this.Controls.Remove(numericUpDown1);
                this.Controls.Remove(instructionLabel);

                this.label1.Text = "Finding an answer...";

                DFS(triangle.board, triangle.createBoard(endState), triangle.graph);

            }
            else
            {
                this.Close();
            }
        }

        void nudRow_ValueChanged(object sender, EventArgs e)
        {
            nudPeg.Maximum = nudRow.Value;
        }

        void remove_Click(object sender, EventArgs e)
        {
            if (step == 1)
            {
                int row, peg;
                row = (int)nudRow.Value;
                peg = (int)nudPeg.Value;

                if (startState.ContainsKey(row))
                {
                    List<int> existingList = startState[row];
                    existingList.Add(peg);
                }
                else
                {
                    List<int> pegList = new List<int>();
                    pegList.Add(peg);
                    startState.Add(row, pegList);
                }

                label2.Text += "( " + row + ", " + peg + " ), ";
            }
            else
            {
                int row, peg;
                row = (int)nudRow.Value;
                peg = (int)nudPeg.Value;

                if (endState.ContainsKey(row))
                {
                    List<int> existingList = endState[row];
                    existingList.Add(peg);
                }
                else
                {
                    List<int> pegList = new List<int>();
                    pegList.Add(peg);
                    endState.Add(row, pegList);
                }

                label2.Text += "( " + row + ", " + peg + " ), ";
            }
        }

        public void DFS( List<List<Peg>> start, List<List<Peg>> goal, Graph graph)
        {
            nodeComparer nComp = new nodeComparer();
            Dictionary<List<List<Peg>>, List<List<Peg>>> parent = new Dictionary<List<List<Peg>>, List<List<Peg>>>(nComp);
            Stack<List<List<Peg>>> stack = new Stack<List<List<Peg>>>();
            stack.Push(start);
            parent.Add(start, null);
            List<List<Peg>> current = null;
            while(stack.Count() != 0)
            {
                current = stack.Pop();
                if (current == goal || current.SequenceEqual(goal) || nComp.Equals(current,goal)) break;
                
                foreach (EdgeInfo ef in graph.nodes[current].adjacent)
                {
                    if (!parent.ContainsKey(ef.goesTo))
                    {
                        parent.Add(ef.goesTo, current);
                        stack.Push(ef.goesTo);
                    }
                }
            }
            if (current == goal || current.SequenceEqual(goal) || nComp.Equals(current, goal))
            {
                bool first = true;
                int left = 0;
                int top = 0;
                this.label1.Text = "Solution found!!";
                this.button1.Text = "Close";
                //goal
                this.label2.Text = triangle.printBoard(goal) + "<====";
                List<List<Peg>> from = parent[goal];

                while (from != null)
                {
                    Label nLabel = new Label();
                    nLabel.AutoSize = true;
                    if(first)
                    {
                        nLabel.Top = this.label2.Top;
                        nLabel.Left = this.label2.Left + this.label2.Width + 5;
                        first = false;
                    }
                    else
                    {
                        nLabel.Top = top;
                        nLabel.Left = left;
                    }
                    nLabel.Text += triangle.printBoard(from) + "<====";
                    this.panel1.Controls.Add(nLabel);
                    from = parent[from];
                    left = nLabel.Left + nLabel.Width + 5;
                    top = nLabel.Top;
                }
            }
            else
            {
                this.label2.Text = "goal not reachable";
            }
        }
    }
}
