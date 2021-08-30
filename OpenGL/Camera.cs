using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL
{
    class Camera
    {
        public int fov;

        private int uniformProj;
        private int uniformView;

        public Camera(int _shaderProgram, int _uniformProj, int _uniformView, int _fov)
        {
            uniformProj = _uniformProj;
            uniformView = _uniformView;
            fov = _fov;

            GL.UseProgram(_shaderProgram);
            GL.Enable(EnableCap.DepthTest);
        }

        public void UpdateProj(GameWindow _window)
        {
            Matrix4 projMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), (float)_window.Size.X / (float)_window.Size.Y, 0.1f, 1000.0f);
            GL.UniformMatrix4(uniformProj, false, ref projMatrix);
        }

        public Vector3 UpdateView(Vector3 _position, Vector3 _orientation, float _pitch, float _yaw, GameWindow _window)
        {
            _pitch = MathHelper.Clamp(_pitch, MathF.PI / 180 * -89, MathF.PI / 180 * 89);

            _orientation.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _orientation.Y = MathF.Sin(_pitch);
            _orientation.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

            _orientation = Vector3.Normalize(_orientation);

            Matrix4 view = Matrix4.LookAt(_position, _position + _orientation, new Vector3(0, 1, 0));
            GL.UniformMatrix4(uniformView, false, ref view);

            return _orientation;
        }
    }
}
