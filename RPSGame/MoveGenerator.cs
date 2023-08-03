using System.Security.Cryptography;
using System.Text;

namespace RPSGame
{
    class MoveGenerator
    {
        private readonly string[] _moves;
        private readonly HMACKey _hmacKey;

        public MoveGenerator(string[] moves, HMACKey hmacKey)
        {
            _moves = moves;
            _hmacKey = hmacKey;
        }

        public string GenerateMove()
        {
            var random = new Random();
            var index = random.Next(0, _moves.Length);
            return _moves[index];
        }

        public string GetHMAC(string move)
        {
            using (var hmac = new HMACSHA256(_hmacKey.Key))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(move));
                return BitConverter.ToString(hashBytes).Replace("-", "");
            }
        }
    }
}
