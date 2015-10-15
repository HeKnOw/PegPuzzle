using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pegPuzzle
{
    class Puzzle
    {
        nodeComparer nComparer = new nodeComparer();
        public List<List<Peg>> board;
        Form1 form;
        public Graph graph = new Graph();
        private int n;
        private Dictionary<int, List<int>> pegsToRemove;
        public Task graphIt;

        public Puzzle(int n, Dictionary<int, List<int>> pegsRemoved, Form1 form)
        {
            graph.nodes = new Dictionary<List<List<Peg>>, GraphNode>(nComparer);
            board = new List<List<Peg>>();
            this.n = n;
            this.form = form;
            this.pegsToRemove = pegsRemoved;
            //start a thread that will create graph
            //this.form.action.Visible = false;
            //graphIt = new Task(new Action(createGraph));
            //graphIt.Start();
            createGraph();
            //this.form.progressLable.Text = "Creating Nodes...\nNodes created: " + graph.nodes.Count.ToString();
        }

        //after this is done we will have a full state of a triangle with all pegs 
        private void createGraph()
        {
            bool added;
            //pegsRemoved
            for (int i = 0; i < this.n; i++)
            {
                List<Peg> row = new List<Peg>();
                board.Add(row);
                for (int j = 0; j < i + 1; j++)
                {
                    added = false;
                    if (pegsToRemove.ContainsKey(i + 1))
                    {
                        //check each peg in that row
                        foreach (int peg in pegsToRemove[i + 1])
                        {
                            if (j + 1 == peg)
                            {
                                row.Add(new Peg(false));
                                pegsToRemove[i + 1].Remove(j + 1);
                                added = true;
                                break;
                            }
                        }
                        if (!added)
                        {
                            row.Add(new Peg(true));
                        }
                    }
                    else
                    {
                        row.Add(new Peg(true));
                    }
                }
            }
            //board has been constructed
            //board = this will be our base node
            //the rest of our graphs will come from this one  

            //check for moves one empty peg at a time

            //foreach emptypeg in board
            //  foreach 6 possible moves
            //      if left up 
            //          new board = enter and make move
            //          board.adjacent = new board
            //          new board.adjacent = recurse(new board)
            //      if right up
            //          same
            //      ....... do for all 6
            //      at the end just add an adjacent = null to later discard
            //

            populateGraph(board);
            printBoard(board);
            //this.form.action.Visible = true;
            //this.form.label2.Text = printBoard(board);
        }

        public List<List<Peg>> createBoard( Dictionary<int, List<int>> state)
        {
            bool added;
            List<List<Peg>> _board = new List<List<Peg>>();

            //pegsRemoved
            for (int i = 0; i < this.n; i++)
            {
                List<Peg> row = new List<Peg>();
                _board.Add(row);
                for (int j = 0; j < i + 1; j++)
                {
                    added = false;
                    if (state.ContainsKey(i + 1))
                    {
                        //check each peg in that row
                        foreach (int peg in state[i + 1])
                        {
                            if (j + 1 == peg)
                            {
                                row.Add(new Peg(true));
                                state[i + 1].Remove(j + 1);
                                added = true;
                                break;
                            }
                        }
                        if (!added)
                        {
                            row.Add(new Peg(false));
                        }
                    }
                    else
                    {
                        row.Add(new Peg(false));
                    }
                }
            }

            return _board;
        }

        private void populateGraph(List<List<Peg>> board)
        {
            //this.form.Invalidate();
            if (!graph.nodes.ContainsKey(board))
            {
                GraphNode gNode = new GraphNode();
                gNode.adjacent = new List<EdgeInfo>();
                graph.nodes.Add(board, gNode);
                int rows = board.Count;

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < board[i].Count; j++)
                    {
                        if (board[i][j].Status)
                        {
                            //peg is in do nothing
                        }
                        else
                        {
                            //peg is out start
                            if (checkLeft(board, i, j))
                            {
                                //move available do it and get new board
                                List<List<Peg>> nBoard = new List<List<Peg>>();
                                nBoard = makeMoveGetNewBoard(board, "L", i, j);
                                //create a new edge
                                // ->
                                EdgeInfo edge = new EdgeInfo();
                                //initialize it to have the new board as its node
                                // -> o
                                edge.goesTo = nBoard;
                                //add the new edge and its corresponding new node to the current list of edges that belong to the current node
                                // o ( -> o), ( -> o)
                                graph.nodes[board].adjacent.Add(edge);

                                populateGraph(nBoard);

                            }
                            if (checkRight(board, i, j))
                            {
                                //move available do it and get new board
                                List<List<Peg>> nBoard = new List<List<Peg>>();
                                nBoard = makeMoveGetNewBoard(board, "R", i, j);
                                //create a new edge
                                // ->
                                EdgeInfo edge = new EdgeInfo();
                                //initialize it to have the new board as its node
                                // -> o
                                edge.goesTo = nBoard;
                                //add the new edge and its corresponding new node to the current list of edges that belong to the current node
                                // o ( -> o), ( -> o)
                                graph.nodes[board].adjacent.Add(edge);

                                populateGraph(nBoard);

                            }
                            if (checkUpLeft(board, i, j))
                            {
                                //move available do it and get new board
                                List<List<Peg>> nBoard = new List<List<Peg>>();
                                nBoard = makeMoveGetNewBoard(board, "UL", i, j);
                                //create a new edge
                                // ->
                                EdgeInfo edge = new EdgeInfo();
                                //initialize it to have the new board as its node
                                // -> o
                                edge.goesTo = nBoard;
                                //add the new edge and its corresponding new node to the current list of edges that belong to the current node
                                // o ( -> o), ( -> o)
                                graph.nodes[board].adjacent.Add(edge);

                                populateGraph(nBoard);

                            }
                            if (checkUpRight(board, i, j))
                            {
                                //move available do it and get new board
                                List<List<Peg>> nBoard = new List<List<Peg>>();
                                nBoard = makeMoveGetNewBoard(board, "UR", i, j);
                                //create a new edge
                                // ->
                                EdgeInfo edge = new EdgeInfo();
                                //initialize it to have the new board as its node
                                // -> o
                                edge.goesTo = nBoard;
                                //add the new edge and its corresponding new node to the current list of edges that belong to the current node
                                // o ( -> o), ( -> o)
                                graph.nodes[board].adjacent.Add(edge);

                                populateGraph(nBoard);

                            }
                            if (checkDownLeft(board, i, j))
                            {
                                //move available do it and get new board
                                List<List<Peg>> nBoard = new List<List<Peg>>();
                                nBoard = makeMoveGetNewBoard(board, "DL", i, j);
                                //create a new edge
                                // ->
                                EdgeInfo edge = new EdgeInfo();
                                //initialize it to have the new board as its node
                                // -> o
                                edge.goesTo = nBoard;
                                //add the new edge and its corresponding new node to the current list of edges that belong to the current node
                                // o ( -> o), ( -> o)
                                graph.nodes[board].adjacent.Add(edge);

                                populateGraph(nBoard);

                            }
                            if (checkDownRight(board, i, j))
                            {
                                //move available do it and get new board
                                List<List<Peg>> nBoard = new List<List<Peg>>();
                                nBoard = makeMoveGetNewBoard(board, "DR", i, j);
                                //create a new edge
                                // ->
                                EdgeInfo edge = new EdgeInfo();
                                //initialize it to have the new board as its node
                                // -> o
                                edge.goesTo = nBoard;
                                //add the new edge and its corresponding new node to the current list of edges that belong to the current node
                                // o ( -> o), ( -> o)
                                graph.nodes[board].adjacent.Add(edge);

                                populateGraph(nBoard);

                            }
                        }
                    }
                }
            }
        }

        private bool checkUpLeft(List<List<Peg>> board, int row, int column)
        {
            if (row < 2 || column < 2)
            {
                //first 2 rows or first 2 columns
                return false;
            }
            else
            {
                if (board[row - 1][column - 1].Status)
                {
                    //peg to jump exists
                    if (board[row - 2][column - 2].Status)
                    {
                        //peg that jumps is there
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        private bool checkUpRight(List<List<Peg>> board, int row, int column)
        {
            if (row < 2 || column > row - 2)
            {
                //first 2 rows or last 2 columns
                return false;
            }
            else
            {
                if (board[row - 1][column].Status)
                {
                    //peg to jump exists
                    if (board[row - 2][column].Status)
                    {
                        //peg that jumps is there
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        private bool checkLeft(List<List<Peg>> board, int row, int column)
        {
            if (column < 2)
            {
                //first 2 columns
                return false;
            }
            else
            {
                if (board[row][column - 1].Status)
                {
                    //peg to jump exists
                    if (board[row][column - 2].Status)
                    {
                        //peg that jumps is there
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        private bool checkRight(List<List<Peg>> board, int row, int column)
        {
            if (column > row - 2)
            {
                //last 2 columns
                return false;
            }
            else
            {
                if (board[row][column + 1].Status)
                {
                    //peg to jump exists
                    if (board[row][column + 2].Status)
                    {
                        //peg that jumps is there
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        private bool checkDownLeft(List<List<Peg>> board, int row, int column)
        {
            if (row > board.Count - 3)
            {
                //last 2 rows or first 2 columns
                return false;
            }
            else
            {
                if (board[row + 1][column].Status)
                {
                    //peg to jump exists
                    if (board[row + 2][column].Status)
                    {
                        //peg that jumps is there
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        private bool checkDownRight(List<List<Peg>> board, int row, int column)
        {
            if (row > board.Count - 3)
            {
                //last 2 rows or last 2 columns
                return false;
            }
            else
            {
                if (board[row + 1][column + 1].Status)
                {
                    //peg to jump exists
                    if (board[row + 2][column + 2].Status)
                    {
                        //peg that jumps is there
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        private List<List<Peg>> makeMoveGetNewBoard(List<List<Peg>> board, string move, int row, int column)
        {
            //new board
            List<List<Peg>> nBoard = board.Select(x => new List<Peg>(x.Select(y => new Peg(y.Status)).ToList())).ToList();

            if (move == "L")
            {
                nBoard[row][column].Status = true;
                nBoard[row][column - 1].Status = false;
                nBoard[row][column - 2].Status = false;

                return nBoard;
            }
            else if (move == "R")
            {
                nBoard[row][column].Status = true;
                nBoard[row][column + 1].Status = false;
                nBoard[row][column + 2].Status = false;

                return nBoard;
            }
            else if (move == "UR")
            {
                nBoard[row][column].Status = true;
                nBoard[row - 1][column].Status = false;
                nBoard[row - 2][column].Status = false;

                return nBoard;
            }
            else if (move == "UL")
            {
                nBoard[row][column].Status = true;
                nBoard[row - 1][column - 1].Status = false;
                nBoard[row - 2][column - 2].Status = false;

                return nBoard;
            }
            else if (move == "DR")
            {
                nBoard[row][column].Status = true;
                nBoard[row + 1][column + 1].Status = false;
                nBoard[row + 2][column + 2].Status = false;

                return nBoard;
            }
            else if (move == "DL")
            {
                nBoard[row][column].Status = true;
                nBoard[row + 1][column].Status = false;
                nBoard[row + 2][column].Status = false;

                return nBoard;
            }
            else
            {
                return null;
            }
        }

        public string printBoard(List<List<Peg>> board)
        {
            //      *
            //     * *
            //    o * o
            //   * * * *
            //  * * * * *
            // * * * * * *
            //* * * * * * *
            // * will be pegs that are in
            // o will be pegs that are out
            string result = "";

            for (int i = 0; i < this.n; i++)
            {
                //add the spaces before the pegs
                for (int k = i; k < this.n - 1; k++)
                {
                    result += " ";
                }
                //go row by row first
                List<Peg> row = board[i];

                //go peg by peg
                for (int j = 0; j < i + 1; j++)
                {
                    //peg is in
                    if (row[j].Status)
                    {
                        result += "* ";
                    }
                    //peg is out
                    else
                    {
                        result += "o ";
                    }
                }
                //next row
                result += "\n";
            }

            return result;
        }
    }
}
