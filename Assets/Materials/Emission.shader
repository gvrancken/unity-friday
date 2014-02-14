	Shader "Custom/Emission" {
	Properties {
		_ColorTint("Color Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_EmColor("Emission Color", Color) = (1, 1, 1, 1)
		_EmPower("Emission Power", Range (0.00,1.00)) = 0.50
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		#pragma surface surf Lambert

		struct Input {
			float4 color : Color;
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
		};
		
		float4 _ColorTint;
		sampler2D _MainTex;
		sampler2D _BumpMap;
		float4 _EmColor;
		float _EmPower;

		void surf (Input IN, inout SurfaceOutput o) {
			IN.color = _ColorTint;
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * IN.color;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Emission = _EmPower*_EmColor.rgb;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
