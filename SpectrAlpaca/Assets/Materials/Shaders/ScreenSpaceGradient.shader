Shader "Custom/ScreenSpaceGradient" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_HueMap ("Hue Map", 2D) = "gray" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_HueAmount ("Hue Amount", Range(0,1)) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _HueMap;

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};

		half _Glossiness;
		half _HueAmount;
		fixed4 _Color;

		// START borrowed code
        float Epsilon = 1e-10;

        float3 rgb2hcv(in float3 RGB) {
            // Based on work by Sam Hocevar and Emil Persson
            float4 P = lerp(float4(RGB.bg, -1.0, 2.0/3.0), float4(RGB.gb, 0.0, -1.0/3.0), step(RGB.b, RGB.g));
            float4 Q = lerp(float4(P.xyw, RGB.r), float4(RGB.r, P.yzx), step(P.x, RGB.r));
            float C = Q.x - min(Q.w, Q.y);
            float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);
            return float3(H, C, Q.x);
        }

        float3 rgb2hsl(in float3 RGB) {
            float3 HCV = rgb2hcv(RGB);
            float L = HCV.z - HCV.y * 0.5;
            float S = HCV.y / (1 - abs(L * 2 - 1) + Epsilon);
            return float3(HCV.x, S, L);
        }

        float3 hsl2rgb(float3 c) {
            c = float3(frac(c.x), clamp(c.yz, 0.0, 1.0));
            float3 rgb = clamp(abs(fmod(c.x * 6.0 + float3(0.0, 4.0, 2.0), 6.0) - 3.0) - 1.0, 0.0, 1.0);
            return c.z + c.y * (rgb - 0.5) * (1.0 - abs(2.0 * c.z - 1.0));
        }
        // END borrowed code

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float3 hsl = rgb2hsl(c.rgb);

			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			float3 mapHsl = rgb2hsl(tex2D (_HueMap, screenUV).rgb);

			hsl.x = lerp(hsl.x, mapHsl.x, _HueAmount);

			o.Albedo = hsl2rgb(hsl);

			o.Smoothness = _Glossiness;

			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
