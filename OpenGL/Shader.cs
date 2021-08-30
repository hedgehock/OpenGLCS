using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL
{
    class Shader
    {
        public int shaderProgramID;

        public int projID;
        public int viewID;
        public int modelID;

        public Shader(string _vertexShaderLocation, string _fragmentShaderLocation)
        {
            shaderProgramID = LoadShaderProgram(_vertexShaderLocation, _fragmentShaderLocation);

            projID = GL.GetUniformLocation(shaderProgramID, "proj");
            viewID = GL.GetUniformLocation(shaderProgramID, "view");
            modelID = GL.GetUniformLocation(shaderProgramID, "model");
        }

        private int LoadShader(string _shaderLocation, ShaderType _type)
        {
            int shaderID = GL.CreateShader(_type);
            GL.ShaderSource(shaderID, File.ReadAllText(_shaderLocation));
            GL.CompileShader(shaderID);
            string infoLog = GL.GetShaderInfoLog(shaderID);

            if (!string.IsNullOrEmpty(infoLog))
            {
                throw new Exception(infoLog);
            }

            return shaderID;
        }

        private int LoadShaderProgram(string _vertexShaderLocation, string _fragmentShaderLocation)
        {
            int shaderProgramID = GL.CreateProgram();

            int vertexShaderID = LoadShader(_vertexShaderLocation, ShaderType.VertexShader);
            int fragmentShaderID = LoadShader(_fragmentShaderLocation, ShaderType.FragmentShader);

            GL.AttachShader(shaderProgramID, vertexShaderID);
            GL.AttachShader(shaderProgramID, fragmentShaderID);
            GL.LinkProgram(shaderProgramID);
            GL.DetachShader(shaderProgramID, vertexShaderID);
            GL.DetachShader(shaderProgramID, fragmentShaderID);
            GL.DeleteShader(vertexShaderID);
            GL.DeleteShader(fragmentShaderID);

            string infoLog = GL.GetProgramInfoLog(shaderProgramID);

            if (!string.IsNullOrEmpty(infoLog))
            {
                throw new Exception(infoLog);
            }

            return shaderProgramID;
        }
    }
}
