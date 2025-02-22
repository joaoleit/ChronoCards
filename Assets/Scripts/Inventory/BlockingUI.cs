public static class BlockingUI
{
    public static bool IsBlocking { get; private set; }

    // Call this when your Canvas is enabled/disabled
    public static void SetBlocking(bool isBlocking)
    {
        IsBlocking = isBlocking;
    }
}