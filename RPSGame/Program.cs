using RPSGame;
using System.Security.Cryptography;
using Spectre.Console;

class Program
{
    static void Main(string[] args)
    {
        RockPaperScissorsGame.Run(args);
    }

    class RockPaperScissorsGame
    {
        public static void Run(string[] args)
        {
            static bool AreAllDistinct(string[] array)
            {
                var set = new HashSet<string>();
                foreach (var item in array)
                {
                    if (!set.Add(item))
                    {
                        return false;
                    }
                }
                return true;
            }

            static byte[] GenerateRandomKey()
            {
                using (var rng = new RNGCryptoServiceProvider())
                {
                    byte[] key = new byte[32];
                    rng.GetBytes(key);
                    return key;
                }
            }

            if (args.Length < 3 || args.Length % 2 == 0 || !AreAllDistinct(args))
            {
                AnsiConsole.MarkupLine("Invalid input. Please provide an odd number >= 3 of non-repeating strings.");
                AnsiConsole.MarkupLine("Example: dotnet run Rock Paper Scissors Lizard Spock");
                return;
            }

            int numberOfMoves = args.Length;

            string[] moves = args;
            var randomKey = GenerateRandomKey();
            var hmacKey = new HMACKey(randomKey);
            var moveGenerator = new MoveGenerator(moves, hmacKey);
            var rules = new GameRules(moves);
            var helpTable = new HelpTable(moves, rules);

            AnsiConsole.MarkupLine("Generated HMAC key: " + BitConverter.ToString(randomKey).Replace("-", ""));
            AnsiConsole.MarkupLine("Your moves:");
            for (int i = 0; i < numberOfMoves; i++)
            {
                AnsiConsole.MarkupLine($"{i + 1} - {moves[i]}");
            }

            while (true)
            {
                AnsiConsole.MarkupLine("\nSelect your move:");
                for (int i = 0; i < numberOfMoves; i++)
                {
                    AnsiConsole.MarkupLine($"{i + 1} - {moves[i]}");
                }
                AnsiConsole.MarkupLine("0 - Exit");

                int userChoice;
                while (!int.TryParse(Console.ReadLine(), out userChoice) || userChoice < 0 || userChoice > numberOfMoves)
                {
                    AnsiConsole.MarkupLine("Invalid choice. Please select a valid move.");
                }

                if (userChoice == 0)
                {
                    break;
                }

                var userMove = moves[userChoice - 1];
                var computerMove = moveGenerator.GenerateMove();
                var result = rules.DetermineWinner(userMove, computerMove);

                AnsiConsole.MarkupLine($"Your move: {userMove}");
                AnsiConsole.MarkupLine($"Computer's move: {computerMove}");
                AnsiConsole.MarkupLine($"Result: {result}");
                AnsiConsole.MarkupLine($"HMAC: {moveGenerator.GetHMAC(userMove)}");

                helpTable.Display();
            }
        }
    }
}