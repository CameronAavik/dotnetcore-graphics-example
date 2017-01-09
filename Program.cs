using System;
using System.Runtime.InteropServices;
public class Program
{
    private const int GL_ARRAY_BUFFER = 0x8892;
    private const int GL_STATIC_DRAW = 0x88E4;
    private const int GL_FLOAT = 0x1406;
    private const int GL_TRIANGLES = 0x0004;
    private const int GL_COLOR_BUFFER_BIT = 0x4000;

    private static string OPENGL_DLL_NAME;

    // Triangle vertices
    private static float[] vertices = 
    {
        -0.5f, -0.5f,
        0.5f, -0.5f,
        0.0f,  0.5f,
    };

    // Load (almost) all the things
    static Program()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            LoadLibrary = WindowsLoader.LoadLibrary;
            GetProcAddress = WindowsLoader.GetProcAddress;
            FreeLibrary = WindowsLoader.FreeLibrary;
            OPENGL_DLL_NAME = "opengl32.dll";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            LoadLibrary = fileName => LinuxLoader.LoadLibrary(fileName, RTLD_NOW);
            GetProcAddress = LinuxLoader.GetProcAddress;
            FreeLibrary = LinuxLoader.FreeLibrary;
            OPENGL_DLL_NAME = "libGL.so.1";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            LoadLibrary = fileName => OSXLoader.LoadLibrary(fileName, RTLD_NOW);
            GetProcAddress = OSXLoader.GetProcAddress;
            FreeLibrary = OSXLoader.FreeLibrary;
            OPENGL_DLL_NAME = "/System/Library/Frameworks/OpenGL.framework/Versions/A/Libraries/libGL.dylib";
        } 
        else 
        {
            Console.Error.WriteLine("Incompatible Operating System");
            Environment.Exit(1);
        }
        IntPtr library = LoadLibrary(OPENGL_DLL_NAME);
        DrawArrays = GetMethod<glDrawArrays>(library);
        FreeLibrary(library);
    }

    public static void Main(string[] args)
    {
        Initialise();
        var window = CreateWindow(1024, 768, ".NET Core Graphics Example", IntPtr.Zero, IntPtr.Zero);
        MakeContextCurrent(window);
        GlfwGetProcedures();
        // Create the VAO
        uint vao = 0;
        GenVertexArrays(1, ref vao);
        BindVertexArray(vao);
        // Create the VBO
        uint vbo = 0;
        GenBuffers(1, ref vbo);
        BindBuffer(GL_ARRAY_BUFFER, vbo);
        BufferData(GL_ARRAY_BUFFER, new IntPtr(sizeof(float) * vertices.Length), vertices, GL_STATIC_DRAW);
        // Draw the Triangle
        EnableVertexAttribArray(0);
        VertexAttribPointer(0, 2, GL_FLOAT, false, 0, IntPtr.Zero);
        do
        {
            ClearColor(0.0F, 0.0F, 0.0F, 1.0F);
            Clear(GL_COLOR_BUFFER_BIT);
            DrawArrays(GL_TRIANGLES, 0, 3);
            SwapBuffers(window);
            PollEvents();
        } while (WindowShouldClose(window) == 0);
    }

    // OpenGL Bindings obtained through LoadLibrary/GetProcAddress
    private delegate void glDrawArrays(int mode, int first, int count);
    private static glDrawArrays DrawArrays;

    // OpenGL Bindings obtained through glfwGetProcAddress
    private delegate void glGenBuffers(int n, ref uint buffers);
    private delegate void glBindBuffer(uint target, uint buffer);
    private delegate void glBufferData(uint target, IntPtr size, float[] data, uint usage);
    private delegate void glEnableVertexAttribArray(uint index);
    private delegate void glVertexAttribPointer(uint indx, int size, uint type, bool normalized, int stride, IntPtr ptr);
    private delegate void glGenVertexArrays(int n, ref uint arrays);
    private delegate void glBindVertexArray(uint array);
    private delegate void glClearColor(float r, float g, float b, float a);
    private delegate void glClear(int mask);
    private static glGenBuffers GenBuffers;
    private static glBindBuffer BindBuffer;
    private static glBufferData BufferData;
    private static glEnableVertexAttribArray EnableVertexAttribArray;
    private static glVertexAttribPointer VertexAttribPointer;
    private static glGenVertexArrays GenVertexArrays;
    private static glBindVertexArray BindVertexArray;
    private static glClearColor ClearColor;
    private static glClear Clear;

    // GLFW Bindings
    [DllImport("glfw", EntryPoint = "glfwInit")] private static extern bool Initialise();
    [DllImport("glfw", EntryPoint = "glfwCreateWindow")] private static extern IntPtr CreateWindow(int width, int height, string title, IntPtr monitor, IntPtr share);
    [DllImport("glfw", EntryPoint = "glfwMakeContextCurrent")] private static extern void MakeContextCurrent(IntPtr window);
    [DllImport("glfw", EntryPoint = "glfwSwapBuffers")] private static extern void SwapBuffers(IntPtr window);
    [DllImport("glfw", EntryPoint = "glfwGetProcAddress")] private static extern IntPtr GetOpenGlFuncPtr(string procname);
    [DllImport("glfw", EntryPoint = "glfwPollEvents")] private static extern void PollEvents();
    [DllImport("glfw", EntryPoint = "glfwWindowShouldClose")] private static extern int WindowShouldClose(IntPtr window);

    private static T GlfwGetProcedure<T>()
    {
        var funcPtr = GetOpenGlFuncPtr(typeof(T).Name);
        if (funcPtr == IntPtr.Zero)
        {
            Console.WriteLine($"Unable to load Function Pointer: {typeof(T).Name}");
            return default(T);
        }
        return Marshal.GetDelegateForFunctionPointer<T>(funcPtr);
    }

    private static void GlfwGetProcedures()
    {
        GenBuffers = GlfwGetProcedure<glGenBuffers>();
        BindBuffer = GlfwGetProcedure<glBindBuffer>();
        BufferData = GlfwGetProcedure<glBufferData>();
        EnableVertexAttribArray = GlfwGetProcedure<glEnableVertexAttribArray>();
        VertexAttribPointer = GlfwGetProcedure<glVertexAttribPointer>();
        GenVertexArrays = GlfwGetProcedure<glGenVertexArrays>();
        BindVertexArray = GlfwGetProcedure<glBindVertexArray>();
        ClearColor = GlfwGetProcedure<glClearColor>();
        Clear = GlfwGetProcedure<glClear>();
    }

    private delegate IntPtr LoadLibraryDel(string fileName);
    private delegate IntPtr GetProcAddressDel(IntPtr library, string procName);
    private delegate int FreeLibraryDel(IntPtr library);
    private static LoadLibraryDel LoadLibrary;
    private static GetProcAddressDel GetProcAddress;
    private static FreeLibraryDel FreeLibrary;
    private const int RTLD_NOW = 0x002;

    private static T GetMethod<T>(IntPtr library)
    {
        var funcPtr = GetProcAddress(library, typeof(T).Name);
        if (funcPtr == IntPtr.Zero)
        {
            Console.WriteLine($"Unable to load Function Pointer: {typeof(T).Name}");
            return default(T);
        }
        return Marshal.GetDelegateForFunctionPointer<T>(funcPtr);
    }

    class WindowsLoader
    {
        [DllImport("kernel32")] public static extern IntPtr LoadLibrary(string fileName);
        [DllImport("kernel32")] public static extern IntPtr GetProcAddress(IntPtr library, string procName);
        [DllImport("kernel32")] public static extern int FreeLibrary(IntPtr library);
    }

    class LinuxLoader
    {
        [DllImport("libdl.so", EntryPoint = "dlopen")] public static extern IntPtr LoadLibrary(string fileName, int flags);
        [DllImport("libdl.so", EntryPoint = "dlsym")] public static extern IntPtr GetProcAddress(IntPtr library, string procName);
        [DllImport("libdl.so", EntryPoint = "dlclose")] public static extern int FreeLibrary(IntPtr library);
    }

    class OSXLoader
    {
        [DllImport("libSystem.dylib", EntryPoint = "dlopen")] public static extern IntPtr LoadLibrary(string fileName, int flags);
        [DllImport("libSystem.dylib", EntryPoint = "dlsym")] public static extern IntPtr GetProcAddress(IntPtr library, string procName);
        [DllImport("libSystem.dylib", EntryPoint = "dlclose")] public static extern int FreeLibrary(IntPtr library);
    }
}