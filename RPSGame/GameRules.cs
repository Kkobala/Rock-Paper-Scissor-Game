namespace RPSGame
{
    class GameRules
    {
        private readonly string[] _moves;
        private readonly Dictionary<string, HashSet<string>> _winningMoves;

        public GameRules(string[] moves)
        {
            _moves = moves;
            _winningMoves = new Dictionary<string, HashSet<string>>();
            InitializeWinningMoves();
        }

        private void InitializeWinningMoves()
        {
            int half = _moves.Length / 2;
            for (int i = 0; i < _moves.Length; i++)
            {
                string currentMove = _moves[i];
                HashSet<string> winsAgainst = new HashSet<string>();
                for (int j = i + 1; j <= i + half; j++)
                {
                    int index = j % _moves.Length;
                    winsAgainst.Add(_moves[index]);
                }
                _winningMoves[currentMove] = winsAgainst;
            }
        }

        public string DetermineWinner(string userMove, string computerMove)
        {
            if (userMove == computerMove)
            {
                return "Draw";
            }
            else if (_winningMoves[userMove].Contains(computerMove))
            {
                return "You Win!";
            }
            else
            {
                return "Computer Wins!";
            }
        }
    }
}
