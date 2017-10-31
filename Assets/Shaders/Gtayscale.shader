Shader "Custom/Gtayscale" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_ColorPow ("Color Power", Float) = 1

		_Normal ("Normal Map", 2D) = "bump" {}
		_NormalPow ("Normal Power", Range(-2,2)) = 1

		_EmissionCol ("Emission Color", Color) = (1,1,1,1)
		_EmissionTex ("Emission Texture", 2D) = "black" {}
		_EmissionPow ("Emission Power", Range(0,10)) = 0

		_NoiseTex ("Noise Texture", 2D) = "black" {}
		_NoisePow ("Noise Power", Range(0,10)) = 0

		_BurnTex ("Burn Texture", 2D) = "black" {}
		_BurnSize ("Burn Size", Range(0,1)) = 0
		_BurnPow ("Burn Power", Range(0,5)) = 0

//		_Position ("Mask Position", Vector) = (0,0,0,0)
//		_Radius ("Mask Radius", Float) = 0
//		_Softness ("Mask Softness", Float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0


		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		fixed4 _Color;
		sampler2D _MainTex;
		half _ColorPow;

		sampler2D _Normal;
		half _NormalPow;

		sampler2D _NoiseTex;
		half _NoisePow;

		sampler2D _BurnTex;
		half _BurnSize;
		half _BurnPow;

		fixed4 _EmissionCol;
		sampler2D _EmissionTex;
		half _EmissionPow;

		uniform float4 GRAYSCALE_Position;
		uniform half GRAYSCALE_Radius;
		uniform half GRAYSCALE_Softness;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// get textures color
			fixed4 col = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 emi = tex2D (_EmissionTex, IN.uv_MainTex) * _EmissionCol;
			fixed4 noise = tex2D (_NoiseTex, IN.uv_MainTex);

			// calculate lerp factor
			half dist = distance(IN.worldPos, GRAYSCALE_Position);
			half lerpFac = (dist - GRAYSCALE_Radius) / -GRAYSCALE_Softness;
			lerpFac -= noise * _NoisePow;
			lerpFac = saturate(lerpFac);

			// create grayscale color
			half gray = (col.r + col.g + col.b) * 0.333;
			fixed4 gc = fixed4(gray, gray, gray, col.a);

			// lerp textures
			fixed4 albedo = lerp(gc, col * _ColorPow, lerpFac);
			fixed4 emission = lerp(fixed4(0,0,0,0), emi * _EmissionPow, lerpFac);

			// apply albedo
			o.Albedo = albedo.rgb;

			if(lerpFac < _BurnSize && lerpFac > 0)
			{
				float2 uv_burn = float2(lerpFac * (1/_BurnSize), 0);
				fixed4 burnEmission = tex2D (_BurnTex, uv_burn);
				emission = lerp(emission, burnEmission * _BurnPow, lerpFac);
				o.Albedo *= emission;
			}

			o.Normal = normalize(UnpackScaleNormal(tex2D(_Normal, IN.uv_MainTex), _NormalPow));
			o.Emission = emission;
			o.Alpha = albedo.a;
		}
		ENDCG
	}
	CustomEditor "GrayscaleEditor"
	FallBack "Diffuse"
}
