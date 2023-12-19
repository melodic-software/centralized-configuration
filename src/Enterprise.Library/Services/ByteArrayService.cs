namespace Enterprise.Library.Services;

public static class ByteArrayService
{
    public static byte[] Combine(byte[] a, byte[] b)
    {
        byte[] dst = new byte[a.Length + b.Length];
        Buffer.BlockCopy(a, 0, dst, 0, a.Length);
        Buffer.BlockCopy(b, 0, dst, a.Length, b.Length);
        return dst;
    }
}