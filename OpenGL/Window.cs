using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using System.IO;

namespace OpenGL
{
    class Window
    {
        public Vector2i size;
        public string title;

        public Window(Vector2i _size, string _title)
        {
            size = _size;
            title = _title;

            GameWindow window = ConstructWindow(size, title);
            window.VSync = VSyncMode.Off;

            Shader shader = new Shader("../../../../vertex_shader.glsl", "../../../../fragment_shader.glsl");
            
            Camera camera = new Camera(shader.shaderProgramID, shader.projID, shader.viewID, 70);

            window.CursorGrabbed = true;

            Vector3 cameraPosition = new Vector3(0, 0, -20.0f);
            Vector3 orientation = new Vector3(0, 0, 1.0f);
            float yaw = 0.0f;
            float pitch = 0.0f;

            bool _firstMove = true;
            Vector2 _lastPos = new Vector2(0, 0);
            Vector3 up = new Vector3(0, 1, 0);

            float counter = 0;
            window.RenderFrame += (FrameEventArgs _args) =>
            {
                // Counter
                counter += 100.0f * (float)window.RenderTime;

                // FPS
                //Console.WriteLine((1.0f / window.RenderTime).ToString());

                // Clear
                GL.ClearColor(0.3f, 0.3f, 0.3f, 0);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                GL.Viewport(0, 0, window.Size.X, window.Size.Y);

                orientation = camera.UpdateView(cameraPosition, orientation, pitch, yaw, window);

                for (int a = 5; a > 0; a--)
                {
                    Cube cube = new Cube(shader.modelID);
                    cube.Update(1.0f, new Vector3(a * 2, 0.0f, 0.0f), counter, counter, 0);

                    for (int b = 5; b > 0; b--)
                    {
                        Cube cube2 = new Cube(shader.modelID);
                        cube.Update(1.0f, new Vector3(a * 2, 0.0f, b * 2), counter, counter, 0);

                        for (int c = 5; c > 0; c--)
                        {
                            Cube cube3 = new Cube(shader.modelID);
                            cube.Update(1.0f, new Vector3(a * 2, c * 2, b * 2), counter, counter, 0);
                        }
                    }
                }

                window.SwapBuffers();
            };

            window.UpdateFrame += (FrameEventArgs _args) =>
            {
                if (_firstMove)
                {
                    _lastPos = new Vector2(window.MousePosition.X, window.MousePosition.Y);
                    _firstMove = false;
                }
                else
                {
                    var deltaX = window.MousePosition.X - _lastPos.X;
                    var deltaY = window.MousePosition.Y - _lastPos.Y;
                    _lastPos = new Vector2(window.MousePosition.X, window.MousePosition.Y);

                    yaw += deltaX * 0.001f;
                    pitch -= deltaY * 0.001f;
                }

                if (window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.W))
                {
                    cameraPosition += 5.0f * orientation * (float)window.UpdateTime;
                }
                else if (window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.S))
                {
                    cameraPosition -= 5.0f * orientation * (float)window.UpdateTime;
                }
                if (window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.A))
                {
                    cameraPosition -= Vector3.Normalize(Vector3.Cross(orientation, up)) * 5.0f * (float)window.UpdateTime;
                }
                else if (window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D))
                {
                    cameraPosition += Vector3.Normalize(Vector3.Cross(orientation, up)) * 5.0f * (float)window.UpdateTime;
                }
                else if (window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
                {
                    window.Close();
                }
            };

            window.Resize += (ResizeEventArgs _args) =>
            {
                camera.UpdateProj(window);
            };

            window.Run();
        }

        private GameWindow ConstructWindow(Vector2i _size, string _title)
        {
            GameWindowSettings gws = GameWindowSettings.Default;
            gws.IsMultiThreaded = true;

            NativeWindowSettings nws = NativeWindowSettings.Default;
            nws.Size = new Vector2i(_size.X, _size.Y);
            nws.Title = _title;
            nws.WindowBorder = WindowBorder.Resizable;

            return new GameWindow(gws, nws);
        }
    }
}
