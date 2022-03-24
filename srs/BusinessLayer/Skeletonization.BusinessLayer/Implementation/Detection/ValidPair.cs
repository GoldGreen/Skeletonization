namespace Skeletonization.BusinessLayer.Implementation.Detection
{
    internal struct ValidPair
    {
        public int Aid { get; set; }
        public int Bid { get; set; }
        public float Score { get; set; }

        public ValidPair(int aid, int bid, float score)
        {
            Aid = aid;
            Bid = bid;
            Score = score;
        }
    }
}
