Shader "Custom/Post Outline"
{
	Properties
	{
		_MainTex("Main Texture",2D) = "white"{}
	}
		SubShader
	{
	Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM

			sampler2D _MainTex;

			//<SamplerName>_TexelSize is a float2 that says how much screen space a texel occupies.
			float2 _MainTex_TexelSize;

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uvs : TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
				v2f o;

				//Despite the fact that we are only drawing a quad to the screen, Unity requires us to multiply vertices by our MVP matrix, presumably to keep things working when inexperienced people try copying code from other shaders.
				o.pos = UnityObjectToClipPos(v.vertex);

				//Also, we need to fix the UVs to match our screen space coordinates. There is a Unity define for this that should normally be used.
				o.uvs = o.pos.xy / 2 + 0.5;
				o.uvs.y = -o.uvs.y + 1;

				return o;
			}


			half4 frag(v2f i) : COLOR
			{
				//arbitrary number of iterations for now
				int NumberOfIterations = 5;

				//split texel size into smaller words
				float TX_x = _MainTex_TexelSize.x;
				float TX_y = _MainTex_TexelSize.y;

				//and a final intensity that increments based on surrounding intensities.
				float ColorIntensityInRadius;

				//if something already exists underneath the fragment, discard the fragment.
				if (tex2D(_MainTex,i.uvs.xy).r > 0)
				{
					discard;
				}

				//for every iteration we need to do horizontally
				for (int k = 0; k < NumberOfIterations; k += 1)
				{
					//for every iteration we need to do vertically
					for (int j = 0; j < NumberOfIterations; j += 1)
					{
						//increase our output color by the pixels in the area
						ColorIntensityInRadius += tex2D(
													  _MainTex,
													  i.uvs.xy + float2(
																		(k - NumberOfIterations / 2)*TX_x,
																		(j - NumberOfIterations / 2)*TX_y
																   )
													 ).r;
					}
				}

				//output some intensity of teal
				return ColorIntensityInRadius * half4(1,1,0,1);
		}

	ENDCG
}
//end pass
	}
		//end subshader
}
//end shader

/*
Blurred version (not working, not a priority atm)
Shader "Custom/Post Outline"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white"{}
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			sampler2D _MainTex;

			float2 _MainTex_TexelSize;

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uvs : TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);

				o.uvs = o.pos.xy / 2 + 0.5;
				o.uvs.y = -o.uvs.y + 1;

				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				int numIterations = 9;

				float TX_x = _MainTex_TexelSize.x;
				float TX_y = _MainTex_TexelSize.y;

				float colorIntensity;

				for (int k = 0; k < numIterations; k++)
				{
					colorIntensity += tex2D(_MainTex,
											i.uvs.xy + float2((k - numIterations / 2) * TX_x, 0)).r/numIterations;
				}

				return colorIntensity * half4(0, 1, 1, 1);
			}

				ENDCG
		}

		GrabPass{}

		Pass
		{
			CGPROGRAM
			sampler2D _MainTex;
			sampler2D _SceneTex;

			sampler2D _GrabTexture;

			float2 _GrabTexture_TexelSize;

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uvs : TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);

				o.uvs = o.pos.xy / 2 + 0.5;
				o.uvs.y = -o.uvs.y;

				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				int numIterations = 20;

				float TX_y = _GrabTexture_TexelSize.y;

				half colIntensity = 0;

				if (tex2D(_MainTex, i.uvs.xy).r > 0)
				{
					discard;
				}

				for (int j = 0; j < numIterations; j++)
				{
					colIntensity += tex2D(_GrabTexture,
						float2(i.uvs.x, 1 - i.uvs.y) + float2(0,
						(j - numIterations / 2)*TX_y)
					).r / numIterations;
				}
				half4 outcolor = colIntensity * half4(0, 1, 1, 1) * 2 + (1 - colIntensity)*tex2D(_SceneTex, float2(i.uvs.x, 1 - i.uvs.y));
				return outcolor;
			}
		ENDCG
		}
	}
}*/