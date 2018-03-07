Shader "Custom/WorldSpaceColour" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf Lambert 

		sampler2D _MainTex;

	struct Input {
		float3 worldPos;
	};

	float _CycleSpeed;
	fixed4 _Color;


	void surf(Input IN, inout SurfaceOutput o) {
		float _myTimeX = _Time.y + _SinTime.z;
		float _myTimeY = _Time.y + _SinTime.w;
		float _myTimeZ = _Time.y + _SinTime.z;

		o.Albedo = fixed4(
			fmod(abs(IN.worldPos.x + _myTimeY), 3+_SinTime.w),
			fmod(abs(IN.worldPos.x + _myTimeX), 3+_SinTime.z),
			1,
			1)*_Color;


	}
	ENDCG
	}
		FallBack "Diffuse"
}