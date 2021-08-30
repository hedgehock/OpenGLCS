#version 330

layout(location=0) in vec3 vPosition;
layout(location=1) in vec3 vColors;

out vec3 color_in;

uniform mat4 proj;
uniform mat4 view;

uniform mat4 model;

void main()
{
	color_in = vColors;
	gl_Position = proj * view * model * vec4(vPosition.x, vPosition.y + (gl_InstanceID * 2), vPosition.z, 1.0);
}
