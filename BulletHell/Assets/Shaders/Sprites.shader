Shader "Custom/Sprites"
{
	Properties
	{
		_Colour("Tint Colour", Color) = (1,1,1,1)
	}
		SubShader
	{
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" "CanUseSpriteAtlas" = "True"}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert nofog keepalpha
		#pragma multi_compile _ PIXELSNAP_ON
		#pragma shader_feature ETC1_EXTERNAL_ALPHA

		#include "UnityCG.cginc"

		fixed4 _Colour;

        struct Input
        {
			float2 uv_MainTex;
			fixed4 color;
        };

        void vert (inout appdata_full v, out Input o)
        {
			#if defined(PIXELSNAP_ON)
				v.vertex = UnityPixelSnap(v.vertex);
			#endif

			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = v.color * _Colour;
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
			o.Emission = IN.color;
        }
        ENDCG
    }
}
