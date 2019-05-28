// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DrawSimple"
{
	SubShader
	{
		ZWrite Off
		ZTest Always
		Lighting Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct v2f
			{
				float4 pos : POSITION;
			};

			v2f vert(v2f i)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(i.pos);
				return o;
			}

			half4 frag() : COLOR0
			{
				return half4(1,1,1,1);
			}

			ENDCG
		}
	}
}