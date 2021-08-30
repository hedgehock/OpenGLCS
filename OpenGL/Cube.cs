using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL
{
    class Cube
    {
        float[] verts =
            {
                    // Front
                    -0.5f,-0.5f,0.5f,
                    -0.5f, 0.5f,0.5f,
                    0.5f,-0.5f,0.5f,
                    0.5f,-0.5f,0.5f,
                    -0.5f, 0.5f,0.5f,
                    0.5f, 0.5f,0.5f,

                    // Left
                    -0.5f,-0.5f, 0.5f,
                    -0.5f, 0.5f, 0.5f,
                    -0.5f,-0.5f, -0.5f,
                    -0.5f,-0.5f, -0.5f,
                    -0.5f, 0.5f, 0.5f,
                    -0.5f, 0.5f, -0.5f,

                    // Right
                    0.5f,-0.5f, 0.5f,
                    0.5f, 0.5f, 0.5f,
                    0.5f,-0.5f, -0.5f,
                    0.5f,-0.5f, -0.5f,
                    0.5f, 0.5f, 0.5f,
                    0.5f, 0.5f, -0.5f,

                    // Back
                    -0.5f,-0.5f,-0.5f,
                    -0.5f, 0.5f,-0.5f,
                    0.5f,-0.5f,-0.5f,
                    0.5f,-0.5f,-0.5f,
                    -0.5f, 0.5f,-0.5f,
                    0.5f, 0.5f,-0.5f,

                    // Top
                    -0.5f, 0.5f, 0.5f,
                    0.5f, 0.5f, 0.5f,
                    -0.5f, 0.5f, -0.5f,
                    -0.5f, 0.5f, -0.5f,
                    0.5f, 0.5f, 0.5f,
                    0.5f, 0.5f, -0.5f,

                    // Bottom
                    -0.5f, -0.5f, 0.5f,
                    0.5f, -0.5f, 0.5f,
                    -0.5f, -0.5f, -0.5f,
                    -0.5f, -0.5f, -0.5f,
                    0.5f, -0.5f, 0.5f,
                    0.5f, -0.5f, -0.5f,
                };

        float[] color =
        {
                    // Front
                    1.0f, 0.0f, 0.0f,
                    1.0f, 0.0f, 0.0f,
                    1.0f, 0.0f, 0.0f,
                    1.0f, 0.0f, 0.0f,
                    1.0f, 0.0f, 0.0f,
                    1.0f, 0.0f, 0.0f,

                    // Left
                    0.0f, 1.0f, 0.0f,
                    0.0f, 1.0f, 0.0f,
                    0.0f, 1.0f, 0.0f,
                    0.0f, 1.0f, 0.0f,
                    0.0f, 1.0f, 0.0f,
                    0.0f, 1.0f, 0.0f,

                    // Right
                    1.0f, 1.0f, 0.0f,
                    1.0f, 1.0f, 0.0f,
                    1.0f, 1.0f, 0.0f,
                    1.0f, 1.0f, 0.0f,
                    1.0f, 1.0f, 0.0f,
                    1.0f, 1.0f, 0.0f,

                    // Back
                    0.0f, 0.0f, 1.0f,
                    0.0f, 0.0f, 1.0f,
                    0.0f, 0.0f, 1.0f,
                    0.0f, 0.0f, 1.0f,
                    0.0f, 0.0f, 1.0f,
                    0.0f, 0.0f, 1.0f,

                    // Top
                    0.0f, 1.0f, 1.0f,
                    0.0f, 1.0f, 1.0f,
                    0.0f, 1.0f, 1.0f,
                    0.0f, 1.0f, 1.0f,
                    0.0f, 1.0f, 1.0f,
                    0.0f, 1.0f, 1.0f,

                    // Bottom
                    1.0f, 1.0f, 1.0f,
                    1.0f, 1.0f, 1.0f,
                    1.0f, 1.0f, 1.0f,
                    1.0f, 1.0f, 1.0f,
                    1.0f, 1.0f, 1.0f,
                    1.0f, 1.0f, 1.0f,
                };

        int vao = GL.GenVertexArray();
        int vertices = GL.GenBuffer();
        int colors = GL.GenBuffer();

        int uniformModel;

        public Cube(int _uniformModel)
        {
            uniformModel = _uniformModel;
        }

        public void Update(float _scale, Vector3 _position, float _rotX, float _rotY, float _rotZ)
        {
            // *********** Uniforms **********
            // ***** Model ***** 
            Matrix4 pos, scale, rotX, rotY, rotZ, final;
            // Position
            Matrix4.CreateTranslation(_position, out pos);
            // Scale
            Matrix4.CreateScale(_scale, out scale);
            // Rotation
            Matrix4.CreateRotationX((MathF.PI / 180.0f) * _rotX, out rotX);
            Matrix4.CreateRotationY((MathF.PI / 180.0f) * _rotY, out rotY);
            Matrix4.CreateRotationZ((MathF.PI / 180.0f) * _rotZ, out rotZ);
            final = rotX * rotY * rotZ * pos * scale;
            GL.UniformMatrix4(uniformModel, false, ref final);

            GL.BindVertexArray(vao);

            // Verts
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertices);
            GL.BufferData(BufferTarget.ArrayBuffer, verts.Length * sizeof(float), verts, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            // Colors
            GL.BindBuffer(BufferTarget.ArrayBuffer, colors);
            GL.BufferData(BufferTarget.ArrayBuffer, color.Length * sizeof(float), color, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            // Cleanup
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(vertices);
            GL.DeleteBuffer(colors);
        }
    }
}
