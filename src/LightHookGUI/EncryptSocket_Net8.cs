using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System;

public class EncBinder
{
    Socket binder;
    Thread at;
    Action<EncSocket> callback;
    public static Action BadConnectionCallback;

    public static void FxSend(Socket s, byte[] data, byte flag)
    {
        using (var ms = new MemoryStream())
        {
            ms.WriteByte(flag);
            ms.Write(BitConverter.GetBytes(data.Length), 0, 4);
            ms.Write(data, 0, data.Length);
            byte[] k = ms.ToArray();
            s.Send(k, 0, k.Length, SocketFlags.None);
        }
    }
    public static byte[] FxRecv(Socket s, ref byte flag,uint packLimit = 0)
    {
        int size;
        byte[] dxbuf = new byte[5];
        s.Receive(dxbuf, 0, 5, SocketFlags.None);
        flag = dxbuf[0];
        size = BitConverter.ToInt32(dxbuf, 1);
        if(packLimit > 0 && size > packLimit) //如果超过最大限制
        {
            s.Close();
            if(!(BadConnectionCallback is null))
            {
                BadConnectionCallback.Invoke();
            }
            return null;
        }
        byte[] ret = new byte[size];
        int recved = 0;
        while (recved < size)
        {
            int len = s.Receive(ret, recved, size - recved, SocketFlags.None);
            recved += len;
        }
        return ret;
    }
    public static string str(byte[] input)
    {
        return Encoding.Unicode.GetString(input);
    }
    public static byte[] strToBytes(string input)
    {
        return Encoding.Unicode.GetBytes(input);
    }


    public EncBinder(IPEndPoint endpoint, Action<EncSocket> acceptcb)
    {
        binder = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        binder.Bind(endpoint);
        binder.Listen(20);
        callback = acceptcb;
        at = new Thread(accept);
        at.IsBackground = true;
        at.Start();
    }
    Thread StartThread(ThreadStart ts)
    {
        var t = new Thread(ts);
        t.IsBackground = true;
        t.Start();
        return t;
    }

    public void StopListen()
    {
        ListenWork = false; 
        binder.Dispose();
    }
    bool ListenWork = true;
    void accept()
    {
        while (ListenWork)
        {
            var nw = binder.Accept();
            StartThread(() =>  //监听到新连接传入，开始进行密钥握手
            {
                try
                {
                    var ret = new EncSocket(nw);
                    callback.Invoke(ret);
                }
                catch
                {

                }
            });
        }
    }
}

public class EncSocket
{
    public Socket pSocket;
    bool isRecv = false;

    public EncSocket(Socket sock)
    {
        pSocket = sock;
        isRecv = true;
    }
    public EncSocket()
    {
        pSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public static void FxSend(Socket s, byte[] data, byte flag)
    {
        using (var ms = new MemoryStream())
        {
            ms.WriteByte(flag);
            ms.Write(BitConverter.GetBytes(data.Length), 0, 4);
            ms.Write(data, 0, data.Length);
            byte[] k = ms.ToArray();
            s.Send(k, 0, k.Length, SocketFlags.None);
        }
    }
    public static byte[] FxRecv(Socket s, ref byte flag,Action<float> progressCallback = null)
    {
        int size;
        byte[] dxbuf = new byte[5];
        s.Receive(dxbuf, 0, 5, SocketFlags.None);
        flag = dxbuf[0];
        size = BitConverter.ToInt32(dxbuf, 1);
        if (size == 0)
        {
            throw new Exception();
        }
        byte[] ret = new byte[size];
        int recved = 0;
        bool callback = !(progressCallback is null);
        while (recved < size)
        {
            int len = s.Receive(ret, recved, size - recved, SocketFlags.None);
            recved += len;
            if(callback) //如果调用方需要回调进度，那就计算
            {
                progressCallback.Invoke((float)recved / size);
            }
        }
        return ret;
    }
    public static string str(byte[] input)
    {
        return Encoding.Unicode.GetString(input);
    }
    public static byte[] strToBytes(string input)
    {
        return Encoding.Unicode.GetBytes(input);
    }

    public void Connect(IPEndPoint endp)
    {
        if (isRecv)
        {
            throw new Exception("已经通过Accept实例化，无法再次进行连接");
        }

        pSocket.Connect(endp);
        byte f = 0;
    }

    public void Close()
    {
        try
        {
            pSocket.Close();
        }
        catch { }
        try
        {
            pSocket.Dispose();
        }
        catch { }
    }

    public void Send(byte[] data, byte flag)
    {
        lock (this)
        {
            FxSend(pSocket, data, flag);
        }
    }
    public byte[] Recv(ref byte flag, Action<float> progressCallback = null)
    {
        byte f = 0;
        var rc = FxRecv(pSocket, ref f, progressCallback);
        flag = f;
        return rc;
    }
}

