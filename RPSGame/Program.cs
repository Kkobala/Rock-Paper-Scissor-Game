using RPSGame;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {
        RockPaperScissorsGame.Run();
    }

    class RockPaperScissorsGame
    {
        public static void Run()
        {
            string[] moves = { "Rock", "Paper", "Scissors", "Spock", "Lizard" };

            var randomKey = GenerateRandomKey();
            var hmacKey = new HMACKey(randomKey);
            var moveGenerator = new MoveGenerator(moves, hmacKey);
            var rules = new GameRules(moves);
            var helpTable = new HelpTable(moves, rules);

            Console.WriteLine("Generated HMAC key: " + BitConverter.ToString(randomKey).Replace("-", ""));
            Console.WriteLine("Your moves:");
            for (int i = 0; i < moves.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {moves[i]}");
            }

            while (true)
            {
                Console.WriteLine("\nSelect your move:");
                for (int i = 0; i < moves.Length; i++)
                {
                    Console.WriteLine($"{i + 1} - {moves[i]}");
                }
                Console.WriteLine("0 - Exit");

                int userChoice;
                while (!int.TryParse(Console.ReadLine(), out userChoice) || userChoice < 0 || userChoice > moves.Length)
                {
                    Console.WriteLine("Invalid choice. Please select a valid move.");
                }

                if (userChoice == 0)
                {
                    Console.WriteLine("Thanks for playing!");
                    break;
                }

                var userMove = moves[userChoice - 1];
                var computerMove = moveGenerator.GenerateMove();
                var result = rules.DetermineWinner(userMove, computerMove);

                Console.WriteLine($"Your move: {userMove}");
                Console.WriteLine($"Computer's move: {computerMove}");
                Console.WriteLine($"Result: {result}");
                Console.WriteLine($"HMAC: {moveGenerator.GetHMAC(userMove)}");

                helpTable.Display();
            }
        }

        private static bool AreAllDistinct(string[] array)
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

        private static byte[] GenerateRandomKey()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[32]; // 256 bits
                rng.GetBytes(key);
                return key;
            }
        }
    }
}
