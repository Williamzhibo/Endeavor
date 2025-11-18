using SDL2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

static partial class Engine
{
    private static IntPtr Window;
    private static bool Fullscreen;
    private static Texture RenderTarget;
    private static Game Game;

    /// <summary>
    /// The amount of time (in seconds) since the last frame.
    /// </summary>
    public static float TimeDelta { get; private set; }

    private static void Main(string[] args)
    {
        Start();
        Run();
    }

    private static void Start()
    {
        // ======================================================================================
        // Hide the console window as quickly as possible
        // ======================================================================================

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            ShowWindow(GetConsoleWindow(), 0);
        }

        // ======================================================================================
        // Copy assets and libraries into the working directory
        // ======================================================================================

        string targetBase = Directory.GetCurrentDirectory();
        string sourceBase = targetBase;
        
        while (true)
        {
            // Figure out if we're in the project's base directory:
            if (File.Exists(Path.Combine(sourceBase, "Game.sln")) &&
                Directory.Exists(Path.Combine(sourceBase, "Assets")) &&
                Directory.Exists(Path.Combine(sourceBase, "Libraries")))
            {
                MirrorDirectory(Path.Combine(sourceBase, "Assets"), Path.Combine(targetBase, "Assets"), true);
                MirrorDirectory(Path.Combine(sourceBase, "Libraries"), Path.Combine(targetBase, ""), false);
                break;
            }

            // Try again in the parent directory, stopping when we run out of places to look:
            DirectoryInfo parent = Directory.GetParent(sourceBase);
            if (parent == null)
            {
                break;
            }
            else
            {
                sourceBase = parent.FullName;
            }
        }

        // ======================================================================================
        // Initialize SDL
        // ======================================================================================

        if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) != 0)
        {
            throw new Exception("Failed to initialize SDL.");
        }

        if (SDL_ttf.TTF_Init() != 0)
        {
            throw new Exception("Failed to initialize SDL_ttf.");
        }

        SDL_mixer.MIX_InitFlags mixInitFlags = SDL_mixer.MIX_InitFlags.MIX_INIT_MP3 | SDL_mixer.MIX_InitFlags.MIX_INIT_OGG | SDL_mixer.MIX_InitFlags.MIX_INIT_FLAC;
        if (((SDL_mixer.MIX_InitFlags)SDL_mixer.Mix_Init(mixInitFlags) & mixInitFlags) != mixInitFlags)
        {
            throw new Exception("Failed to initialize SDL_mixer.");
        }

        if (SDL_mixer.Mix_OpenAudio(44100, SDL_mixer.MIX_DEFAULT_FORMAT, 2, 1024) != 0)
        {
            throw new Exception("Failed to initialize SDL_mixer.");
        }

        SDL_mixer.Mix_AllocateChannels(AudioChannelCount);

        Window = SDL.SDL_CreateWindow(
            Game.Title,
            SDL.SDL_WINDOWPOS_CENTERED_DISPLAY(0),
            SDL.SDL_WINDOWPOS_CENTERED_DISPLAY(0),
            (int)Game.Resolution.X,
            (int)Game.Resolution.Y,
            0);

        if (Window == IntPtr.Zero)
        {
            throw new Exception("Failed to create window.");
        }

        Renderer = SDL.SDL_CreateRenderer(Window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC | SDL.SDL_RendererFlags.SDL_RENDERER_TARGETTEXTURE);

        if (Renderer == IntPtr.Zero)
        {
            throw new Exception("Failed to create renderer.");
        }

        IntPtr renderTargetHandle = SDL.SDL_CreateTexture(Renderer, SDL.SDL_PIXELFORMAT_RGBA8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, (int)Game.Resolution.X, (int)Game.Resolution.Y);
        RenderTarget = new Texture(renderTargetHandle, (int)Game.Resolution.X, (int)Game.Resolution.Y);

        // ======================================================================================
        // Instantiate the game object
        // ======================================================================================

        Game = new Game();
    }

    private static void Run()
    {
        ulong lastFrameStartTime = SDL.SDL_GetPerformanceCounter();
        const int TARGET_FPS = 60;
        const float timePerFrame = 1.0f / TARGET_FPS;

        while (true)
        {
            ulong frameStartCounter = SDL.SDL_GetPerformanceCounter();
            TimeDelta = (frameStartCounter - lastFrameStartTime) / (float)SDL.SDL_GetPerformanceFrequency();
            lastFrameStartTime = frameStartCounter;

            // Process pre-update engine logic:
            PollEvents();

            // Toggle between windowed and fullscreen mode when Alt+Enter is pressed:
            if (GetKeyDown(Key.Return) && (GetKeyHeld(Key.LeftAlt) || GetKeyHeld(Key.RightAlt)))
            {
                Fullscreen = !Fullscreen;
                SDL.SDL_SetWindowFullscreen(Window, Fullscreen ? (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP : 0);
            }

            // Clear and start drawing into the render target:
            SDL.SDL_SetRenderTarget(Renderer, RenderTarget.Handle);
            SDL.SDL_SetRenderDrawColor(Renderer, 0, 0, 0, 255);
            SDL.SDL_RenderClear(Renderer);

            // Update game logic:
            Game.Update();

            // Draw game graphics:
            Game.Draw();

            // Copy the render target to the screen:
            SDL.SDL_SetRenderTarget(Renderer, IntPtr.Zero);
            SDL.SDL_RenderCopy(Renderer, RenderTarget.Handle, IntPtr.Zero, IntPtr.Zero);
            SDL.SDL_RenderPresent(Renderer);

            // Cap frame rate
            float frameTime = (SDL.SDL_GetPerformanceCounter() - frameStartCounter) / (float)SDL.SDL_GetPerformanceFrequency();
            if (frameTime < timePerFrame)
            {
                SDL.SDL_Delay((uint)((timePerFrame - frameTime) * 1000));
            }
        }
    }

    private static void MirrorDirectory(string sourceDirectory, string targetDirectory, bool deleteUnusedFiles)
    {
        // Ensure the target directory exists, as otherwise our attempt to enumerate its files will fail:
        Directory.CreateDirectory(targetDirectory);

        // Copy new files from source that don't exist or are out of date in target:
        foreach (string sourceFile in Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories))
        {
            string targetFile = sourceFile.Replace(sourceDirectory, targetDirectory);
            if (!File.Exists(targetFile) || File.GetLastWriteTime(targetFile) < File.GetLastWriteTime(sourceFile))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(targetFile));
                File.Copy(sourceFile, targetFile, true);
            }
        }

        // Delete old files in target that no longer exist in source:
        if (deleteUnusedFiles)
        {
            foreach (string targetFile in Directory.GetFiles(targetDirectory, "*", SearchOption.AllDirectories))
            {
                string sourceFile = targetFile.Replace(targetDirectory, sourceDirectory);
                if (!File.Exists(sourceFile))
                {
                    File.Delete(targetFile);
                }
            }
        }
    }

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
}
