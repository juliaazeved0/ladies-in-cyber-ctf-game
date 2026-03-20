public static class SafeBase
{
    private static int key = 14;

    public static string ViewBase(int[] baseHiden)
    {
        string baseClean = "";
        for (int i = 0; i < baseHiden.Length; i++)
        {
            baseClean += (char)(baseHiden[i] - key);
        }
        return baseClean;
    }
    
    public static readonly int[] flag_0 = new int[] { 90, 63, 81, 137, 97, 65, 88, 79, 109, 80, 65, 91, 109, 100, 63, 92, 82, 66, 139, };
    public static readonly int[] flag_1 = new int[] { 90, 63, 81, 137, 94, 128, 65, 129, 129, 66, 62, 109, 83, 129, 130, 66, 112, 63, 122, 63, 136, 66, 114, 66, 139, };
    public static readonly int[] flag_2 = new int[] { 90, 63, 81, 137, 84, 119, 127, 131, 115, 79, 130, 115, 124, 130, 111, 79, 87, 124, 116, 125, 128, 123, 111, 113, 125, 115, 129, 83, 129, 113, 125, 124, 114, 119, 114, 111, 129, 139, };
    public static readonly int[] flag_3 = new int[] { 90, 63, 81, 137, 81, 118, 115, 113, 121, 109, 94, 118, 135, 129, 63, 113, 111, 122, 109, 90, 111, 135, 65, 128, 139, };
    public static readonly int[] flag_4 = new int[] { 90, 63, 81, 137, 82, 62, 123, 111, 63, 124, 109, 80, 122, 62, 113, 121, 65, 114, 109, 97, 131, 113, 113, 65, 129, 129, 139, };
    public static readonly int[] flag_5 = new int[] { 90, 63, 81, 137, 62, 97, 63, 92, 69, 109, 92, 66, 109, 96, 65, 81, 65, 63, 98, 66, 109, 82, 65, 109, 94, 99, 82, 63, 91, 139, };
    public static readonly int[] flag_6 = new int[] { 90, 63, 81, 137, 130, 65, 113, 122, 66, 114, 62, 63, 124, 130, 131, 63, 130, 63, 132, 62, 139, };
    public static readonly int[] flag_7 = new int[] { 90, 63, 81, 137, 81, 79, 94, 87, 100, 79, 96, 79, 81, 96, 87, 94, 98, 93, 139, };
    public static readonly int[] flag_8 = new int[] { 90, 63, 81, 137, 84, 90, 66, 85, 109, 84, 63, 92, 66, 90, 109, 94, 66, 96, 66, 80, 65, 92, 97, 139, };

}