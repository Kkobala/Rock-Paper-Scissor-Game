namespace RPSGame
{
    class HelpTable
    {
        private readonly string[] _moves;
        private readonly GameRules _rules;

        public HelpTable(string[] moves, GameRules rules)
        {
            _moves = moves;
            _rules = rules;
        }

        public void Display()
        {
            Console.WriteLine("\nHelp Table:");
            Console.Write("  ");
            foreach (var move in _moves)
            {
                Console.Write($"  {move}");
            }
            Console.WriteLine();

            foreach (var move1 in _moves)
            {
                Console.Write($"{move1} ");
                foreach (var move2 in _moves)
                {
                    var result = GetResultSymbol(move1, move2);
                    Console.Write($"  {result}");
                }
                Console.WriteLine();
            }
        }

        private string GetResultSymbol(string move1, string move2)
        {
            if (move1 == move2)
            {
                return "Draw";
            }
            else if (_rules.DetermineWinner(move1, move2) == "You Win!")
            {
                return "Win";
            }
            else
            {
                return "Lose";
            }
        }
    }
}
