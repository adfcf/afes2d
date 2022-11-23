using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Afes2D.Gfx.Model {
    internal sealed class RectBatch {

        readonly int BaseDataElements = 4;
        readonly int InstanceDataElements = 22;

        int vao;

        int baseVbo;
        int instanceVbo;

        public int Capacity { get; }

        readonly float[] instanceData;

        int currentIndex;
        int pushedElements;
        int toDraw;

        public RectBatch(int capacity) {
            Capacity = capacity;
            instanceData = new float[capacity * InstanceDataElements];
        }

        public void Init() {

            // XXYY-UUVV
            float[] vertices = {

                0.0f, 1.0f, 0.0f, 0.0f, // 0
			    1.0f, 1.0f, 1.0f, 0.0f, // 1
			    0.0f, 0.0f, 0.0f, 1.0f, // 2
			    1.0f, 0.0f, 1.0f, 1.0f  // 3

            };

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            baseVbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, baseVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, BaseDataElements * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, BaseDataElements * sizeof(float), 2 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            instanceVbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, instanceVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, InstanceDataElements * Capacity * sizeof(float), IntPtr.Zero, BufferUsageHint.DynamicDraw);

            // instance data - model matrix (row 0)
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, InstanceDataElements * sizeof(float), 0 * sizeof(float));
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribDivisor(2, 1);

            // instance data - model matrix (row 1)
            GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, InstanceDataElements * sizeof(float), 4 * sizeof(float));
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribDivisor(3, 1);

            // instance data - model matrix (row 2)
            GL.VertexAttribPointer(4, 4, VertexAttribPointerType.Float, false, InstanceDataElements * sizeof(float), 8 * sizeof(float));
            GL.EnableVertexAttribArray(4);
            GL.VertexAttribDivisor(4, 1);

            // instance data - model matrix (row 3)
            GL.VertexAttribPointer(5, 4, VertexAttribPointerType.Float, false, InstanceDataElements * sizeof(float), 12 * sizeof(float));
            GL.EnableVertexAttribArray(5);
            GL.VertexAttribDivisor(5, 1);

            // instance data - tint
            GL.VertexAttribPointer(6, 4, VertexAttribPointerType.Float, false, InstanceDataElements * sizeof(float), 16 * sizeof(float));
            GL.EnableVertexAttribArray(6);
            GL.VertexAttribDivisor(6, 1);

            // instance data - spriteIndex
            GL.VertexAttribPointer(7, 1, VertexAttribPointerType.Float, false, InstanceDataElements * sizeof(float), 20 * sizeof(float));
            GL.EnableVertexAttribArray(7);
            GL.VertexAttribDivisor(7, 1);

            // instance data - lightingInfo
            GL.VertexAttribPointer(8, 1, VertexAttribPointerType.Float, false, InstanceDataElements * sizeof(float), 21 * sizeof(float));
            GL.EnableVertexAttribArray(8);
            GL.VertexAttribDivisor(8, 1);

            GL.BindVertexArray(0);

        }

        public void Push(Matrix4 transform, Vector4 tint, int spriteIndex, int lightingInfo) {

            if (pushedElements >= Capacity) 
                throw new Exception("Pushing overflow.");

            for (int i = 0; i < 16; ++i) {
                instanceData[currentIndex + i] = transform[i / 4, i % 4];
            }
            currentIndex += 16;

            instanceData[currentIndex++] = tint.X;
            instanceData[currentIndex++] = tint.Y;
            instanceData[currentIndex++] = tint.Z;
            instanceData[currentIndex++] = tint.W;

            instanceData[currentIndex++] = spriteIndex;
            instanceData[currentIndex++] = lightingInfo;

            ++pushedElements;

        }

        public void Flush() {

            if (pushedElements == 0)
                return;

            GL.BindBuffer(BufferTarget.ArrayBuffer, instanceVbo);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, pushedElements * InstanceDataElements * sizeof(float), instanceData);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            toDraw = pushedElements;

            currentIndex = 0;
            pushedElements = 0;

        }

        public void Draw() {
            GL.BindVertexArray(vao);
            GL.DrawArraysInstanced(PrimitiveType.TriangleStrip, 0, 4, toDraw);
        }

    }
}
