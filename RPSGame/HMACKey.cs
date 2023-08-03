namespace RPSGame
{
    class HMACKey
    {
        public HMACKey(byte[] key)
        {
            Key = key;
        }

        public byte[] Key { get; }
    }
}
