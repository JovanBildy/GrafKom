using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Pertemuan1
{
    internal class Asset2d
    {
        float[] _vertices =
        {
            -0.5f, -0.5f, 0.0f,
            0.5f, -0.5f, 0.0f,
            0.0f, 0.5f, 0.0f
        };

        //float[] _vertices =
        //{
        //    -0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 0.0f, //v1 merah
        //    0.5f, -0.5f, 0.0f,   0.0f, 1.0f, 0.0f, //v2 hijau
        //    0.0f, 0.5f, 0.0f,    0.0f, 0.0f, 1.0f, //v3 biru
        //};


        //float[] _vertices =
        //{
        //    0.5f, 0.5f, 0.0f, //top right
        //    0.5f, -0.5f, 0.0f, // bot right
        //    -0.5f, -0.5f, 0.0f, // bot left
        //    -0.5f, 0.5f, 0.0f // top left
        //};

        //uint[] _indices =
        //{
        //    0, 1, 3, // triangle 1
        //    1, 2, 3 // triangle 2
        //};

        uint[] _indices =
        {
        };


        int _vertexBufferObject;
        int _elementBufferObject;
        int _vertexArrayObject;
        Shader _shader;
        int index;
        int[] _pascal;

        public Asset2d(float[] vertices, uint[] indices)
        {
            _vertices = vertices;
            _indices = indices;
            index = 0;
        }

        public void load(string shaderVert, string shaderFrag)
        {
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
                BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float,
            //    false, 3 * sizeof(float), 0);

            //parameter 1 --> variable _vertices nya itu disimpan di shader index
            //keberapa?
            //parameter 2 --> didalam variable _vertices, ada berapa vertex?
            //paramter 3  --> jenis vertex yang dikirim typenya apa?
            //parameter 4 --> datanya perlu dinormalisasi ndak?
            //parameter 5 --> dalam 1 vertex/1 baris itu mengandung berapa banyak
            //titik?
            //parameter 6 --> data yang mau diolah mulai dari brp?


            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float,
                false, 3 * sizeof(float), 0);

            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float,
            //    false, 6 * sizeof(float), 0);

            //GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float,
            //    false, 6 * sizeof(float), 3 * sizeof(float));

            if (_indices.Length != 0)
            {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length
                    * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            }

            GL.EnableVertexAttribArray(0);
            //GL.EnableVertexAttribArray(1);

            //_shader = new Shader("C:/Users/USER/Documents/KULIAH/Ms Visual Studio/Pertemuan 1/Shaders/shader.vert", "C:/Users/USER/Documents/KULIAH/Ms Visual Studio/Pertemuan 1/Shaders/shader.frag");
            _shader = new Shader(shaderVert, shaderFrag);
            _shader.Use();
        }
        
        public void render(int pilihan)
        {
            _shader.Use();
            GL.BindVertexArray(_vertexArrayObject);

            //int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");
            //GL.Uniform4(vertexColorLocation, 0.0f, 0.1f, 0.0f, 0.1f);

            if (_indices.Length != 0)
            {
                GL.DrawElements(PrimitiveType.Triangles, _indices.Length,
                DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                if(pilihan == 0)
                {
                    GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
                }
                else if(pilihan == 1)
                {
                    GL.DrawArrays(PrimitiveType.TriangleFan, 0, (_vertices.Length + 1)/3);
                }
                else if(pilihan == 2)
                {
                    GL.DrawArrays(PrimitiveType.LineStrip, 0, index);
                }
                else if (pilihan == 3)
                {
                    GL.DrawArrays(PrimitiveType.LineStrip, 0, (_vertices.Length-1)/3);
                }
            }
        }

        public void createCircle(float center_X, float center_y, float radius)
        {
            _vertices = new float[1080];

            for (int i = 0; i < 360; i++)
            {
                double degInRad = i * Math.PI / 180;

                //x
                _vertices[i * 3] = radius * (float)Math.Cos(degInRad);
                //y
                _vertices[i * 3 + 1] = radius * (float)Math.Sin(degInRad);
                //z
                _vertices[i * 3 + 2] = 0;
            }
        }

        public void createElips(float center_X, float center_y, float radius_x, float radius_y)
        {
            _vertices = new float[1080];

            for (int i = 0; i < 360; i++)
            {
                double degInRad = i * Math.PI / 180;

                //x
                _vertices[i * 3] = radius_x * (float)Math.Cos(degInRad);
                //y
                _vertices[i * 3 + 1] = radius_y * (float)Math.Sin(degInRad);
                //z
                _vertices[i * 3 + 2] = 0;
            }
        }

        public void updateMousePosition(float _x, float _y)
        {
            //x
            _vertices[index * 3] = _x;
            //y
            _vertices[index * 3 + 1] = _y;
            //z
            _vertices[index * 3 + 2] = 0;

            index++;

            // load
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
                BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float,
                false, 3 * sizeof(float), 0);
        }

        public List<int> getRow(int rowIndex)
        {
            List<int> currow = new List<int>();
            //------
            currow.Add(1);
            if (rowIndex == 0)
            {
                return currow;
            }
            //-----
            List<int> prev = getRow(rowIndex - 1);
            for (int i = 1; i < prev.Count; i++)
            {
                int curr = prev[i - 1] + prev[i];
                currow.Add(curr);
            }
            currow.Add(1);
            return currow;
        }

        public List<float> createCurveBezier()
        {
            List<float> _vertices_bezier = new List<float>();
            List<int> pascal = getRow(index - 1);
            _pascal = pascal.ToArray();

            for(float t=0; t<=1; t+=0.001f)
            {
                Vector2 p = getP(index, t);
                _vertices_bezier.Add(p.X);
                _vertices_bezier.Add(p.Y);
                _vertices_bezier.Add(0);
            }

            return _vertices_bezier;
        }

        public Vector2 getP(int n, float t)
        {
            Vector2 p = new Vector2(0, 0);

            float k;
            for (int i = 0; i < n; i++)
            {
                k = (float)Math.Pow((1 - t), n - 1 - i) * (float)Math.Pow(t, i) * _pascal[i];
                p.X += k * _vertices[i * 3];
                p.Y += k * _vertices[i * 3 + 1];
            }
            return p;
        }

        public bool getVerticesLength()
        {
            if(_vertices[0] == 0)
            {
                return false;
            }
           
            if ((_vertices.Length+1)/3 > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void setVertices(float[] _temp)
        {
            _vertices = _temp;
        }
    }
}
