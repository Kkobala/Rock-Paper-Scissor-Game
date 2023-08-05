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
                Console.WriteLine("Invalid input. Please provide an odd number >= 3 of non-repeating strings.");
                Console.WriteLine("Example: dotnet run Rock Paper Scissors Lizard Spock");
                return;
            }

            int numberOfMoves = args.Length;

            string[] moves = args;
            var randomKey = GenerateRandomKey();
            var hmacKey = new HMACKey(randomKey);
            var moveGenerator = new MoveGenerator(moves, hmacKey);
            var rules = new GameRules(moves);
            var helpTable = new HelpTable(moves, rules);

            Console.WriteLine("Generated HMAC key: " + BitConverter.ToString(randomKey).Replace("-", ""));
            Console.WriteLine("Your moves:");
            for (int i = 0; i < numberOfMoves; i++)
            {
                Console.WriteLine($"{i + 1} - {moves[i]}");
            }

            var table = new Table();
            table.AddColumn("User Move");
            table.AddColumn("Computer Move");
            table.AddColumn("Result");
            table.AddColumn("HMAC");

            while (true)
            {
                Console.WriteLine("\nSelect your move:");
                for (int i = 0; i < numberOfMoves; i++)
                {
                    Console.WriteLine($"{i + 1} - {moves[i]}");
                }
                Console.WriteLine("0 - Exit");

                int userChoice;
                while (!int.TryParse(Console.ReadLine(), out userChoice) || userChoice < 0 || userChoice > numberOfMoves)
                {
                    Console.WriteLine("Invalid choice. Please select a valid move.");
                }

                if (userChoice == 0)
                {
                    break;
                }

                var userMove = moves[userChoice - 1];
                var computerMove = moveGenerator.GenerateMove();
                var result = rules.DetermineWinner(userMove, computerMove);

                Console.WriteLine($"Your move: {userMove}");
                Console.WriteLine($"Computer's move: {computerMove}");
                Console.WriteLine($"Result: {result}");
                Console.WriteLine($"HMAC: {moveGenerator.GetHMAC(userMove)}");

                table.Rows.Clear();

                table.AddRow(userMove, computerMove, result, moveGenerator.GetHMAC(userMove));

                AnsiConsole.Render(table);

                helpTable.Display();
            }
        }
    }
}