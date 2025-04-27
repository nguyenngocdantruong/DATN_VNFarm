namespace VNFarm.Enums
{
    public enum ChatRoomStatus
    {
        All = -999,
        InProgress = 0,
        Closed = 1,
        Solved = 2
    }
    public enum ChatRoomType
    {
        All = -999,
        DisputeByShipping = 0,
        ChatNormal = 1,
        DisputeByProduct = 2,
        DisputeByPayment = 3,
        DisputeByOrder = 4,
        DisputeByRefund = 5,
        DisputeByOther = 6
    }
    public enum ChatMessageType
    {
        Text = 0,
        Image = 1
    }
}
