using OpenTK.Mathematics;

namespace Afes2D.Gfx.Shader {
    public sealed class TexturedBatchProgram : ShaderProgram {

        public static readonly string ViewMatrixName = "view";

        public static readonly string ProjectionMatrixName = "projection";

        public TexturedBatchProgram() : base(new Sources(

           // DEFAULT VERTEX SHADER
           @"
            #version 330 core
            
            layout (location = 0) in vec2 vertexPosition;
            layout (location = 1) in vec2 vertexTexture;
            layout (location = 2) in mat4 instModelMatrix;
            layout (location = 6) in vec4 instTint;
            layout (location = 7) in float instSpriteIndex;
            layout (location = 8) in float instLightingInfo;

            uniform mat4 view;
            uniform mat4 projection;

            out vec2 textureCoordinates;
            out vec2 fragmentWorldPosition;
            out vec4 tint;

            flat out float spriteIndex;
            flat out float lightingInfo;

            void main() {

                vec4 vertexWorldPosition = instModelMatrix * vec4(vertexPosition.xy, 0.0f, 1.0f);

                gl_Position = projection * view * vertexWorldPosition;

                textureCoordinates = vertexTexture;
                tint = instTint;
                spriteIndex = instSpriteIndex;

                fragmentWorldPosition = vec2(vertexWorldPosition.xy);

            }",

            // DEFAULT FRAGMENT SHADER
            @"
            #version 330 core
            
            out vec4 fragmentColor;

            uniform sampler2DArray spriteSheet;

            in vec2 textureCoordinates;
            in vec2 fragmentWorldPosition;
            in vec4 tint;

            flat in float spriteIndex;
            flat in float lightingInfo;

            void main() {
                
                vec4 texel = texture(spriteSheet, vec3(textureCoordinates.st, spriteIndex));
                if (texel.a < 0.5f) {
                    discard;
                }

                fragmentColor = tint * texel;

            }
            "

        )) {}

        public void SetCamera(Camera camera) => Mat4(ViewMatrixName, camera.View);
        public void SetProjection(float canvasWidth, float canvasHeight) => 
            Mat4(ProjectionMatrixName, Matrix4.CreateOrthographicOffCenter(0.0f, canvasWidth, canvasHeight, 0.0f, 0.0f, 10.0f));

    }
}
